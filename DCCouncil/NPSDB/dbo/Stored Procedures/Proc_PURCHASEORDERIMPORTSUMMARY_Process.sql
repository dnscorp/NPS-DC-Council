CREATE PROCEDURE [dbo].[Proc_PURCHASEORDERIMPORTSUMMARY_Process]
(
    @ImportGUID [uniqueidentifier],
    @SelectedSummaryIds [xml]    
)
AS
BEGIN
    --DECLARE @SelectedSummaryIds [xml]
    --SET @SelectedSummaryIds = '<purchaseorderimportsummaries><purchaseorderimportsummaryid>1257</purchaseorderimportsummaryid><purchaseorderimportsummaryid>1261</purchaseorderimportsummaryid><purchaseorderimportsummaryid>1256</purchaseorderimportsummaryid><purchaseorderimportsummaryid>1252</purchaseorderimportsummaryid></purchaseorderimportsummaries>'
    Declare @XmlDataHandle int
	Exec sp_xml_preparedocument @XmlDataHandle OUTPUT,@SelectedSummaryIds
    
    SELECT
        purchaseorderimportsummaryid
    INTO #TmpPurchaseOrderImportSummaryIds
    FROM OpenXml(@XmlDataHandle,'/purchaseorderimportsummaries/purchaseorderimportsummaryid',2)
    WITH
    (
        purchaseorderimportsummaryid bigint 'text()'
    );
    declare @OfficeID bigint
    DECLARE CUR_SUMMARY_IDS CURSOR FOR    
    SELECT purchaseorderimportsummaryid FROM #TmpPurchaseOrderImportSummaryIds
    
    DECLARE @PurchaseOrderImportSummaryId [bigint]
    OPEN CUR_SUMMARY_IDS
    FETCH NEXT FROM CUR_SUMMARY_IDS INTO @PurchaseOrderImportSummaryId
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        DECLARE @PONumber nvarchar(50)
        SELECT @PONumber = PONumber, @OfficeID = OfficeID  FROM [dbo].[PurchaseOrderImportSummary] WHERE [PurchaseOrderImportSummaryID] = @PurchaseOrderImportSummaryId
        IF NOT EXISTS(SELECT PONumber FROM [dbo].[PurchaseOrders] WHERE [dbo].[PurchaseOrders].[PONumber] = @PONumber and OfficeID=@OfficeID )
            BEGIN
                INSERT INTO [dbo].[PurchaseOrders] ([VendorName], [OBJCode], [DateOfTransaction], [PONumber], [POAmtSum], [POAdjAmtSum], [VoucherAmtSum], [POBalSum], [OfficeID], [FiscalYearID], [BudgetID], [IsDeleted], [ImportID], [CreatedDate], [UpdatedDate])                
                SELECT 
                    [POIS].[VendorName],
                    [POIS].[OBJCode],
                    [POIS].[DateOfTransaction],
                    [POIS].[PONumber],
                    [POIS].[POAmtSum],
                    [POIS].[POAdjAmtSum],
                    [POIS].[VoucherAmtSum],
                    [POIS].[POBalSum],
                    [POIS].[OfficeID],
                    [POIS].[FiscalYearID],
                    [POIS].[BudgetID],
                    [POIS].[IsDeleted],
                    [POIS].[ImportID],
                    GETDATE() AS CreatedDate,
                    NULL AS UpdatedDate                
                FROM [dbo].[PurchaseOrderImportSummary] POIS
                WHERE [POIS].[PurchaseOrderImportSummaryID] = @PurchaseOrderImportSummaryId
                
                UPDATE [dbo].[PurchaseOrderImportSummary]
                SET [ImportStatus] = 1
                WHERE [PurchaseOrderImportSummaryID] = @PurchaseOrderImportSummaryId
            END
        ELSE
            BEGIN
                DELETE FROM [dbo].[PurchaseOrders] WHERE [dbo].[PurchaseOrders].[PONumber] = @PONumber and OfficeID=@OfficeID 
                
                INSERT INTO [dbo].[PurchaseOrders] ([VendorName], [OBJCode], [DateOfTransaction], [PONumber], [POAmtSum], [POAdjAmtSum], [VoucherAmtSum], [POBalSum], [OfficeID], [FiscalYearID], [BudgetID], [IsDeleted], [ImportID], [CreatedDate], [UpdatedDate])                
                SELECT 
                    [POIS].[VendorName],
                    [POIS].[OBJCode],
                    [POIS].[DateOfTransaction],
                    [POIS].[PONumber],
                    [POIS].[POAmtSum],
                    [POIS].[POAdjAmtSum],
                    [POIS].[VoucherAmtSum],
                    [POIS].[POBalSum],
                    [POIS].[OfficeID],
                    [POIS].[FiscalYearID],
                    [POIS].[BudgetID],
                    [POIS].[IsDeleted],
                    [POIS].[ImportID],
                    GETDATE() AS CreatedDate,
                    NULL AS UpdatedDate                
                FROM [dbo].[PurchaseOrderImportSummary] POIS
                WHERE [POIS].[PurchaseOrderImportSummaryID] = @PurchaseOrderImportSummaryId
                
                UPDATE [dbo].[PurchaseOrderImportSummary]
                SET [ImportStatus] = 2
                WHERE [PurchaseOrderImportSummaryID] = @PurchaseOrderImportSummaryId
            END
        
        IF NOT EXISTS(SELECT * FROM [dbo].[Vendors] V                            
                    INNER JOIN [dbo].[PurchaseOrderImportSummary] POIS ON [POIS].[OfficeID] = [V].[OfficeID]
                    AND POIS.[VendorName] = [V].[Name]
                    AND POIS.[FiscalYearID] = [V].[FiscalYearID]
                    AND POIS.[PurchaseOrderImportSummaryID] = @PurchaseOrderImportSummaryId
                    AND [V].[IsDeleted] = 0)
        BEGIN
            INSERT INTO [dbo].[Vendors] ([Name], [DefaultDescription], [OfficeID], [FiscalYearID], [IsRolledUp], [IsDeleted], [CreatedDate], [UpdatedDate])
             SELECT 
                [POIS].[VendorName],
                NULL,
                [POIS].[OfficeID],
                [POIS].[FiscalYearID],
                0,
                0,                
                GETDATE(),
                NULL 
            FROM [dbo].[PurchaseOrderImportSummary] POIS
            WHERE [POIS].[PurchaseOrderImportSummaryID] = @PurchaseOrderImportSummaryId
        END                   
        
        FETCH NEXT FROM CUR_SUMMARY_IDS INTO @PurchaseOrderImportSummaryId
    END
    
    CLOSE CUR_SUMMARY_IDS
    DEALLOCATE CUR_SUMMARY_IDS
    
    DROP TABLE #TmpPurchaseOrderImportSummaryIds
	exec Proc_PURCHASEORDER_RemoveInvalidRecords @ImportGUID       
END
