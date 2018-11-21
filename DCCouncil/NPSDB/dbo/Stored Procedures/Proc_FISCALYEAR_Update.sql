
CREATE PROCEDURE Proc_FISCALYEAR_Update  
(  
    @FiscalYearId [int],  
    @Name [nvarchar](500),      
    @Year [int],  
    @StartDate [datetime],  
    @EndDate datetime  
)  
AS  
BEGIN  
    UPDATE [dbo].[FiscalYears]  
    SET  
        [Name] = @Name,  
        [Year] = @Year,  
        [StartDate] = @StartDate,  
        [EndDate] = @EndDate , 
        UpdatedDate = GETDATE()
    WHERE [FiscalYearID] = @FiscalYearId          
END
