CREATE PROCEDURE [dbo].[Proc_USER_GetByUserId]
(
    @UserId bigint
)
AS
BEGIN
    SELECT 
            [U].[UserID],
            [U].[UserGuid],
            [U].[Username],
            [U].[PasswordHash],
            [U].[IsActive],
            [U].[IsDeleted],
            [U].[LastFiscalYearSelectedID] AS FiscalYearID,
            [FY].[CreatedDate] AS FiscalYearCreatedDate,
            [FY].[Name] AS FiscalYearName,
            [FY].[StartDate] AS FiscalYearStartDate,
            [FY].[EndDate] AS FiscalYearEndDate,
            [FY].[UpdatedDate] AS FiscalYearUpdatedDate,
            [FY].[Year] AS FiscalYearYear,
            [U].[CreatedDate],
            [U].[UpdatedDate],
            [UP].[UserProfileID],
            [UP].[FirstName] AS UserProfileFirstName,
            [UP].[LastName] AS UserProfileLastName,
            [UP].[CreatedDate] AS UserProfileCreatedDate,
            [UP].[UpdatedDate] AS UserProfileUpdatedDate
    FROM Users U
    INNER JOIN [dbo].[UserProfiles] UP ON [UP].[UserID] = [U].[UserID]
    LEFT JOIN [dbo].[FiscalYears] FY ON [U].[LastFiscalYearSelectedID] = [FY].[FiscalYearID]
    WHERE [U].[UserID] = @UserId
END





