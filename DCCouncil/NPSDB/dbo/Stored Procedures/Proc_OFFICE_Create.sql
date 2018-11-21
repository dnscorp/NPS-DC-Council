
CREATE PROCEDURE Proc_OFFICE_Create  
    (  
      @Name [nvarchar](500) ,  
      @ActiveFrom [datetime] ,  
      @ActiveTo [datetime] ,  
      @PCA [nvarchar](50) ,  
      @PCATitle [nvarchar](250) ,  
      @IndexCode [nvarchar](50) ,  
      @IndexTitle [nvarchar](250) ,  
      @IsDeleted [bit] ,
      @CompCode [nvarchar](Max)   
  
    )  
AS   
    BEGIN  
  
        INSERT  INTO [dbo].[Offices]  
                ( [Name] ,  
                  [ActiveFrom] ,  
                  [ActiveTo] ,  
                  [PCA] ,  
                  [PCATitle] ,  
                  [IndexCode] ,  
                  [IndexTitle] ,  
                  [IsDeleted] ,  
                  [CreatedDate] ,  
                  [UpdatedDate],
                  [CompCode] 
                )  
        VALUES  ( @Name ,  
                  @ActiveFrom ,  
                  @ActiveTo ,  
                  @PCA ,  
                  @PCATitle ,  
                  @IndexCode ,  
                  @IndexTitle ,  
                  @IsDeleted ,  
                  GETDATE() ,  
                  NULL ,
                  @CompCode 
                )  
  
  
        DECLARE @OfficeId [bigint]  
        SET @OfficeId = @@IDENTITY  
  
        INSERT  INTO [dbo].[OfficeAttributes]  
                ( [OfficeAttributeLookupID] ,  
                  [OfficeID] ,  
                  [AttributeValue] ,  
                  [CreatedDate] ,  
                  [UpdatedDate]  
                )  
                SELECT  [OAL].[OfficeAttributeLookupID] ,  
                        @OfficeID ,  
                        '' ,  
                        GETDATE() ,  
                        NULL  
                FROM    [dbo].[OfficeAttributeLookups] OAL  
  
  
    END
