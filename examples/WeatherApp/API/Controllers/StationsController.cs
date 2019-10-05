using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly IDbConnection _connection;

        public StationsController(IDbConnection connection)
        {
            _connection = connection;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            _connection.Open();
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "GetStates";
            cmd.CommandType = CommandType.StoredProcedure;
            var data = cmd.ExecuteScalar().ToString();
            _connection.Close();
            return data;
        }
    }
}
