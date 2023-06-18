-- brakujące wpisy w airly."AveragedValues"


SELECT "AveragedValuesId", 

DATE_PART( 'hour' , "FromDateTime") AS missing_hour,
DATE_PART( 'day' , "FromDateTime") AS missing_day,

"MeasurementEntityFk", "AveragedValueTimeType", "FromDateTime", "TillDateTime"
FROM airly."AveragedValues" as AveragedValues

WHERE
	AveragedValues."AveragedValueTimeType" = 2 AND
 	AveragedValues."FromDateTime"  <= '2023-04-12'
ORDER BY
DATE_PART( 'hour' , "FromDateTime"),
DATE_PART( 'day' , "FromDateTime")

-- proponowane nowe wartości


SELECT "AveragedValuesId", 

DATE_PART( 'hour' , "FromDateTime") AS missing_hour,
DATE_PART( 'day' , "FromDateTime") AS missing_day,

 missingWind."WIND_BEARING" AS wind_bearing,
 missingWind."WIND_SPEED" AS wind_speed,

"MeasurementEntityFk", "AveragedValueTimeType", "FromDateTime", "TillDateTime"
FROM airly."AveragedValues" AS AveragedValues
JOIN airly."MissingWind" missingWind ON
	missingWind."Day" = DATE_PART( 'day' , "FromDateTime") 
	AND	missingWind."Month" = DATE_PART( 'month' , "FromDateTime")
    AND missingWind."Hour" = DATE_PART( 'hour' , "FromDateTime")

WHERE
	AveragedValues."AveragedValueTimeType" = 2 AND
 	AveragedValues."FromDateTime"  <= '2023-04-12'
ORDER BY
DATE_PART( 'hour' , "FromDateTime"),
DATE_PART( 'day' , "FromDateTime")


-- uzupełnienie

BEGIN TRANSACTION;

CREATE TEMPORARY TABLE TempTable AS
SELECT "AveragedValuesId", 

DATE_PART( 'hour' , "FromDateTime") AS missing_hour,
DATE_PART( 'day' , "FromDateTime") AS missing_day,

 missingWind."WIND_BEARING" AS wind_bearing,
 missingWind."WIND_SPEED" AS wind_speed,

"MeasurementEntityFk", "AveragedValueTimeType", "FromDateTime", "TillDateTime"
FROM airly."AveragedValues" AS AveragedValues
JOIN airly."MissingWind" missingWind ON
	missingWind."Day" = DATE_PART( 'day' , "FromDateTime") 
	AND	missingWind."Month" = DATE_PART( 'month' , "FromDateTime")
    AND missingWind."Hour" = DATE_PART( 'hour' , "FromDateTime")

WHERE
	AveragedValues."AveragedValueTimeType" in (0, 2) AND
 	AveragedValues."FromDateTime"  <= '2023-04-12'
ORDER BY
DATE_PART( 'hour' , "FromDateTime"),
DATE_PART( 'day' , "FromDateTime");

SELECT * FROM TempTable;

INSERT INTO airly."ValuePairs"("AveragedValueEntityFk", "Name", "Value")
SELECT "AveragedValuesId" AS "AveragedValueEntityFk", 'WIND_BEARING' AS "Name", "wind_bearing" AS "Value" FROM TempTable;

INSERT INTO airly."ValuePairs"("AveragedValueEntityFk", "Name", "Value")
SELECT "AveragedValuesId" AS "AveragedValueEntityFk", 'WIND_SPEED' AS "Name", "wind_speed" AS "Value" FROM TempTable;


SELECT * FROM airly."JoinAllWitoutStandard" WHERE
"RequestDate"  <= '2023-04-12' AND
"valuespairsname" in ('WIND_BEARING', 'WIND_SPEED');

ROLLBACK;
COMMIT;