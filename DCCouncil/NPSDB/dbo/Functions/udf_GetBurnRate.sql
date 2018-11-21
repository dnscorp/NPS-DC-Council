CREATE FUNCTION udf_GetBurnRate
(
    @TotalExpenditureAmount float,
    @TotalBudgetAmount float
)
RETURNS [float]
AS
BEGIN
    DECLARE @ReturnValue [float]
    IF @TotalBudgetAmount IS NULL OR @TotalBudgetAmount = 0
    BEGIN
        SET @ReturnValue = 0;
    END
    ELSE
    BEGIN
        SET @ReturnValue = (@TotalExpenditureAmount/@TotalBudgetAmount)*100
    END
    RETURN @ReturnValue
END
