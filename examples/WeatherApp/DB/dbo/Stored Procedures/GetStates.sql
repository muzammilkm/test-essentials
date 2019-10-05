CREATE PROCEDURE [dbo].[GetStates]
AS
	
	SELECT [State], Count(1) AS CityCount
	FROM dbo.Station
	Group By [State]
	Order By [State]
	FOR JSON AUTO;

RETURN 0
