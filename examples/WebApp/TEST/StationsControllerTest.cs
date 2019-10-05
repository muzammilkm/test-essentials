using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using Xunit;

namespace TEST
{
    public class StationsControllerTest : IClassFixture<TestFixture>
    {
        private readonly HttpClient _client;

        public StationsControllerTest(TestFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async void Get_Should_Return_Proper_Station_List()
        {
            // Arrange
            var url = "api/stations";
            var expectedStatesWithOrder = new[] { "Andhra Pradesh", "Karnataka", "Kerala", "Tamil Nadu", "Telangana" };
            var expectedCityCountWithOrder = new[] { 6, 3, 3, 4, 6 };
            var i = 0;

            // Act
            var response = await _client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            var stations = JArray.Parse(responseContent);

            // Assert

            stations.Should().NotBeNull();
            stations.Count.Should().Be(5);

            foreach(var station in stations.Children())
            {
                station["State"].Type.Should().Be(JTokenType.String);
                station["CityCount"].Type.Should().Be(JTokenType.Integer);

                station["State"].Value<string>().Should().Be(expectedStatesWithOrder[i]);
                station["CityCount"].Value<int>().Should().Be(expectedCityCountWithOrder[i]);

                i++;
            }
        }
    }
}
