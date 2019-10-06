CREATE PROCEDURE [dbo].[GetStateWiseCityTemp]
	@StateName NVARCHAR(50)
AS

	;WITH CityTemp AS (
		SELECT [State], City, FLOOR(RAND( CAST(NEWID() AS varbinary))*(100-20)+20) AS CityTemp
		FROM dbo.Station
		Where [State] = @StateName
	)

	SELECT [State], City, CityTemp, CASE 
		WHEN CityTemp < 40 THEN 'Snow'
		WHEN CityTemp >= 40 AND CityTemp < 60 THEN 'Cold'
		WHEN CityTemp >= 60 AND CityTemp < 80 THEN 'Warm'
		WHEN CityTemp >= 80 THEN 'Hot'
		END AS CityTempDesc
	FROM CityTemp
	Order By [City]
	FOR JSON AUTO;
RETURN 0
