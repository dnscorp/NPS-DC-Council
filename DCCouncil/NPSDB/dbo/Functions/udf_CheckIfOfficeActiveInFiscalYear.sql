CREATE FUNCTION udf_CheckIfOfficeActiveInFiscalYear
(
    @OfficeId [bigint],
    @FiscalYearId [bigint]
)
RETURNS [bit]
AS
BEGIN
    
    DECLARE @OfficeActiveFrom [datetime]
    DECLARE @OfficeActiveTo [datetime]
    
    SELECT @OfficeActiveFrom = [O].[ActiveFrom],@OfficeActiveTo = [O].[ActiveTo]
    FROM [dbo].[Offices] O
    WHERE [O].[OfficeID] = @OfficeId
    
--    DECLARE @Year [int]
--    DECLARE @Month [int]
--    
--    SELECT @Year = [FY].[Year], @Month = [FY].[StartMonth]
--    FROM [dbo].[FiscalYears] FY
--    WHERE [FY].[FiscalYearID] = @FiscalYearId
    
    DECLARE @YearStartingDate [datetime]    
--    SET @YearStartingDate =  CAST(dateadd(yy,(@Year-1900),0) + dateadd(mm,@Month-1,0) + 0 AS DATETIME)
    
    DECLARE @YearEndingDate [datetime]
--    SET @YearEndingDate = DATEADD(day,-1,DATEADD(YEAR,1,@YearStartingDate))
    
    SELECT @YearStartingDate=StartDate,@YearEndingDate=[EndDate]
    FROM [dbo].[FiscalYears]
    WHERE [FiscalYearID] = @FiscalYearId
    
    DECLARE @ReturnValue [bit]
    
    IF(@OfficeActiveTo IS NOT NULL)
    BEGIN
		IF ((@OfficeActiveFrom < @YearStartingDate AND @OfficeActiveTo < @YearStartingDate)
       OR 
       (@OfficeActiveFrom > @YearEndingDate AND @OfficeActiveTo > @YearEndingDate ))
		BEGIN
			SET @ReturnValue = 0
		END
		ELSE        
		BEGIN        
			SET @ReturnValue = 1
		END
    END
    ELSE
    BEGIN
		IF (@OfficeActiveFrom < @YearEndingDate)
		BEGIN
			SET @ReturnValue = 1
		END
		ELSE        
		BEGIN        
			SET @ReturnValue = 0
		END
    END    
    
    RETURN @ReturnValue
END
