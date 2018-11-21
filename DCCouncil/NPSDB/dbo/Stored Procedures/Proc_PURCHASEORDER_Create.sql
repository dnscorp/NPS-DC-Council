CREATE PROCEDURE [dbo].[Proc_PURCHASEORDER_Create]
(  
    @VendorName nvarchar(500) ,
	@OBJCode nvarchar(50) ,
	@DateOfTransaction datetime ,
	@PONumber nvarchar(50) ,
	@POAmtSum float ,
	@POAdjAmtSum float ,
	@VoucherAmtSum float ,
	@POBalSum float ,
	@OfficeID bigint ,
	@FiscalYearID bigint ,
	@BudgetID bigint ,
	@IsDeleted bit ,
	@ImportID bigint 
	
)  
AS  
BEGIN  
 
 INSERT INTO [dbo].[PurchaseOrders] 
    ([VendorName],
     [OBJCode], 
     [DateOfTransaction],
     [PONumber], 
     [POAmtSum],
     [POAdjAmtSum],
     [VoucherAmtSum],
     [POBalSum], 
     [OfficeID], 
     [FiscalYearID], 
     [BudgetID], 
     [IsDeleted],
     [ImportID], 
     [CreatedDate],
     [UpdatedDate])
 
 
 VALUES
      (@VendorName,
      @OBJCode,
      @DateOfTransaction,
      @PONumber,
      @POAmtSum,
      @POAdjAmtSum,
      @VoucherAmtSum,
      @POBalSum,
      @OfficeID,
      @FiscalYearID,
      @BudgetID,
      @IsDeleted,
      @ImportID,
      GETDATE(),
      null)

END

