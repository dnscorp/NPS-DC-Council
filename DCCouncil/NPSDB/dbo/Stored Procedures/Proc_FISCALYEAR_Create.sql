CREATE PROCEDURE [dbo].[Proc_FISCALYEAR_Create]
(
@Name [nvarchar] (500),
@Year [int],
@StartDate [datetime],
@EndDate [datetime]
)

AS
BEGIN

    INSERT INTO [dbo].[FiscalYears]([Name],[Year],[StartDate],[EndDate],[CreatedDate],[UpdatedDate])
    VALUES (@Name,@Year,@StartDate,@EndDate,GETUTCDATE(),NULL)

END



