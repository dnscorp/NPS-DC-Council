CREATE PROCEDURE Proc_TelephoneTransactionImportItems_GetAllByGuid      
(      
 @ImportGUID [uniqueidentifier]   ,    
 @ImportStatus   int    
)      
AS      
BEGIN      
 SELECT       
ItemID ,      
ImportGuid ,      
FoundationAccount ,      
BillingAccount ,      
WirelessNumber ,      
UserName ,      
MarketCycleEndDate ,      
TotalKBUsage,      
TotalNumberofEvents ,      
TotalMOUUsage ,      
TotalCurrentCharges ,      
ImportStatus ,      
CreatedDate ,      
UpdatedDate        
      
FROM TelephoneTransactionImportItems     
WHERE ImportGuid = @ImportGUID      
AND Importstatus= (CASE WHEN @ImportStatus IS NULL THEN Importstatus ELSE @ImportStatus END)  
ORDER BY  Importstatus  
      
      
END
