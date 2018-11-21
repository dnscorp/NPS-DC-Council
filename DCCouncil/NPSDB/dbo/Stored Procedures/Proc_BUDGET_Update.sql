CREATE PROCEDURE Proc_BUDGET_Update
(
    @BudgetId [bigint],    
    @Name [nvarchar] (250),
    @Amount [float],
    @FiscalYearId [bigint],    
    @IsDefault [bit],
    @IsDeleted [bit]
)
AS
BEGIN
    UPDATE [dbo].[Budgets]
    SET [Amount] = @Amount,
        [Name] = @Name,
        [FiscalYearID] = @FiscalYearId,
        [IsDefault] = @IsDefault,
        [IsDeleted] = @IsDeleted,
        [UpdatedDate] = GETUTCDATE()
    WHERE [BudgetID] = @BudgetId
        
END
