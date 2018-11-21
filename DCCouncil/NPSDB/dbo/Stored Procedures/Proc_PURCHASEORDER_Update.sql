CREATE PROCEDURE [dbo].[Proc_PURCHASEORDER_Update]
(
    @PurchaseOrderId bigint,
    @VendorName nvarchar(500) ,
	@OBJCode nvarchar(50) ,
	@DateOfTransaction datetime ,
	@PONumber nvarchar(50) ,
	@POAmtSum float ,
	@POAdjAmtSum float ,
	@VoucherAmtSum float ,
	@POBalSum float ,
	@FiscalYearID bigint ,
	@BudgetID bigint ,
	@IsDeleted bit ,
	@ImportID bigint,
    @Description [nvarchar](MAX), 
	@AlternateOfficeID int null	
)
AS
BEGIN
 UPDATE [dbo].[PurchaseOrders] SET
 
 [VendorName]=@VendorName,
 [OBJCode]=@OBJCode,
 [DateOfTransaction]=@DateOfTransaction,
 [PONumber]=@PONumber,
 [POAmtSum]=@POAmtSum,
 [POAdjAmtSum]=@POAdjAmtSum,
 [VoucherAmtSum]=@VoucherAmtSum,
 [POBalSum]=@POBalSum,
 [FiscalYearID]=@FiscalYearID,
 [BudgetID]=@BudgetID,
 [IsDeleted]=@IsDeleted,
 [ImportID]=@ImportID,
 [UpdatedDate]=GETDATE()
 
 WHERE [PurchaseOrderID]=@PurchaseOrderId
    
 IF EXISTS(SELECT * FROM [dbo].[PurchaseOrderDescriptions] WHERE [dbo].[PurchaseOrderDescriptions].[PONumber] = @PONumber)
 BEGIN
    UPDATE [dbo].[PurchaseOrderDescriptions] SET [DescriptionText] = @Description
    WHERE [PONumber] = @PONumber
 END
 ELSE
 BEGIN
    INSERT INTO [dbo].[PurchaseOrderDescriptions] ([PONumber], [DescriptionText], [CreatedDate], [UpdatedDate])
    VALUES(@PONumber,@Description,GETDATE(),NULL)
 END

 if @AlternateOfficeID<>0 
 begin

	  IF EXISTS(SELECT * FROM [dbo].PurchaseOrdersAlternateOffice WHERE [dbo].PurchaseOrdersAlternateOffice.[PONumber] = @PONumber)
	 BEGIN
		UPDATE [dbo].PurchaseOrdersAlternateOffice SET AlternateOfficeID = @AlternateOfficeID, [UpdatedDate]=getdate()
		WHERE [PONumber] = @PONumber
	 END
	 ELSE
	 BEGIN
		INSERT INTO [dbo].PurchaseOrdersAlternateOffice ([PONumber], AlternateOfficeID, [CreatedDate], [UpdatedDate])
		VALUES(@PONumber,@AlternateOfficeID,GETDATE(),getdate())
	 END
 end
 else
 begin
	delete from PurchaseOrdersAlternateOffice where [PONumber]=@PONumber
 end

END


