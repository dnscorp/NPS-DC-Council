create view vw_PurchaseOrders
as
select 
po.PurchaseOrderID	,
po.VendorName	,
po.OBJCode	,
po.DateOfTransaction	,
po.PONumber	,
po.POAmtSum	,
po.POAdjAmtSum	,
po.VoucherAmtSum	,
po.POBalSum	,
isnull(poao.AlternateOfficeID,po.OfficeID) 'OfficeID',
po.FiscalYearID	,
po.BudgetID	,
po.IsDeleted	,
po.ImportID	,
po.CreatedDate	,
po.UpdatedDate	
from PurchaseOrders PO
left outer join PurchaseOrdersAlternateOffice POAO on POAO.PONumber=po.PONumber
