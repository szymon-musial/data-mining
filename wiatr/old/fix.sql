 -- Missing values
 SELECT
    valuespairs."Value",
	measurements."MeasurementId",
    measurements."RequestDate",
    measurements."RequestTime",   
    measurements."StationName",
  	averagedvalues."FromDateTime",
    valuespairs."AveragedValueEntityFk" AS valuepairaveragedvalueentityfk,
    valuespairs."Name" AS valuespairsname
   FROM airly."Measurements" measurements
     JOIN airly."AveragedValues" averagedvalues ON averagedvalues."MeasurementEntityFk" = measurements."MeasurementId"
     JOIN airly."ValuePairs" valuespairs ON valuespairs."AveragedValueEntityFk" = averagedvalues."AveragedValuesId"
	 
	 WHERE 
	 measurements."RequestDate"  <= '2023-04-12' AND
	 valuespairs."Name" in ('WIND_BEARING', 'WIND_SPEED');


-- 8, 9 21
BEGIN TRANSACTION;

CREATE TEMPORARY TABLE TempTable AS
SELECT
  DISTINCT valuespairs."AveragedValueEntityFk" AS  missing_value_with_given_fk, -- actual wather

  DATE_PART( 'day' , measurements."RequestDate") AS missig_day,
  missingWind."Day" AS filled_day,

  DATE_PART( 'hour' , measurements."RequestTime") AS missing_hour,
  missingWind."Hour" AS filled_hour,

  missingWind."WIND_BEARING" AS wind_bearing,
  missingWind."WIND_SPEED" AS wind_speed,

  measurements."MeasurementId",
  measurements."RequestDate",
  measurements."RequestTime",
  measurements."StationName",
  averagedvalues."FromDateTime"

  FROM airly."Measurements" measurements
  JOIN airly."AveragedValues" averagedvalues ON averagedvalues."MeasurementEntityFk" = measurements."MeasurementId"
  JOIN airly."ValuePairs" valuespairs ON valuespairs."AveragedValueEntityFk" = averagedvalues."AveragedValuesId"
  JOIN airly."MissingWind" missingWind ON 
	 	missingWind."Day" = DATE_PART( 'day' , measurements."RequestDate")
    AND
    (missingWind."Hour" = DATE_PART( 'hour' , measurements."RequestTime") OR    -- 9	AM
    missingWind."Hour"  = (DATE_PART( 'hour' , measurements."RequestTime") +1)  -- 8  AM
			 )
WHERE
  AveragedValues."AveragedValueTimeType" = 0 AND
  measurements."RequestDate"  <= '2023-04-12'

ORDER BY missing_value_with_given_fk;

--SELECT * FROM TempTable;

INSERT INTO airly."ValuePairs"("AveragedValueEntityFk", "Name", "Value")
SELECT missing_value_with_given_fk AS "AveragedValueEntityFk", 'WIND_BEARING' AS "Name", "wind_bearing" AS "Value" FROM TempTable;

INSERT INTO airly."ValuePairs"("AveragedValueEntityFk", "Name", "Value")
SELECT missing_value_with_given_fk AS "AveragedValueEntityFk", 'WIND_SPEED' AS "Name", "wind_speed" AS "Value" FROM TempTable;


ROLLBACK;

SELECT * FROM airly."JoinAllWitoutStandard" WHERE
"RequestDate"  <= '2023-04-12' AND
"valuespairsname" in ('WIND_BEARING', 'WIND_SPEED');