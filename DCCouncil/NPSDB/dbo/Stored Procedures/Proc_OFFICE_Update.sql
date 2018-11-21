
CREATE PROCEDURE Proc_OFFICE_Update  
@OfficeId [BIGINT], @Name NVARCHAR (500), @ActiveFrom DATETIME, @ActiveTo DATETIME, @PCA NVARCHAR (50), @PCATitle NVARCHAR (250), @IndexCode NVARCHAR (50), @IndexTitle NVARCHAR (250), @IsDeleted BIT ,@CompCode nvarchar(Max) 
AS  
BEGIN  
    UPDATE [dbo].[Offices]  
    SET    [Name]        = @Name,  
           [ActiveFrom]  = @ActiveFrom,  
           [ActiveTo]    = @ActiveTo,  
           [PCA]         = @PCA,  
           [PCATitle]    = @PCATitle,  
           [IndexCode]   = @IndexCode,  
           [IndexTitle]  = @IndexTitle,  
           [IsDeleted]   = @IsDeleted,  
           [UpdatedDate] = GETDATE(),
           CompCode = @CompCode  
    WHERE  [OfficeID] = @OfficeId;  
END
