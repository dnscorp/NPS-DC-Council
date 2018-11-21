CREATE PROCEDURE Proc_PURCHASEORDERIMPORT_Insert
(
    @ImportGUID [uniqueidentifier],
    @FileName nvarchar(500),
    @ItemsXml [xml],
    @ImportFile varbinary(MAX)    
)
AS
BEGIN

    DECLARE @CreatedDate [datetime]
    SET @CreatedDate = GETDATE();
    
    INSERT INTO [dbo].[PurchaseOrderImports] ([ImportGUID], [FileName],[ImportFile], [CreatedDate], [UpdatedDate])
    VALUES(@ImportGUID,@FileName,@ImportFile,@CreatedDate,NULL)     
    
    DECLARE @ImportId [bigint]
    SET @ImportId = @@IDENTITY
    
    Declare @XmlDataHandle int
	Exec sp_xml_preparedocument @XmlDataHandle OUTPUT,@ItemsXml        
    
    SELECT *
	INTO #TmpPurchaseOrderImportItems
	FROM OpenXml(@XmlDataHandle,'/purchaseorderimport/purchaseorderimportitem',2)
    WITH
    [dbo].[PurchaseOrderImportItems]
    
    UPDATE #TmpPurchaseOrderImportItems
    SET ImportID = @ImportId,CreatedDate = @CreatedDate
    
    INSERT INTO [dbo].[PurchaseOrderImportItems]
    SELECT * FROM #TmpPurchaseOrderImportItems
	
END
