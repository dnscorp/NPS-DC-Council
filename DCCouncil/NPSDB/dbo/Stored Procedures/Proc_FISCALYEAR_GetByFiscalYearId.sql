CREATE PROCEDURE [dbo].[Proc_FISCALYEAR_GetByFiscalYearId]
(
    @FiscalYearId bigint
)
AS
BEGIN
    SELECT
    	[FY].[CreatedDate],
        [FY].[FiscalYearID],        
        [FY].[Name],
        [FY].[StartDate],
        [FY].[EndDate],
        [FY].[UpdatedDate],
        [FY].[Year]
    FROM [dbo].[FiscalYears] FY
    WHERE [FY].[FiscalYearID] = @FiscalYearId
END




