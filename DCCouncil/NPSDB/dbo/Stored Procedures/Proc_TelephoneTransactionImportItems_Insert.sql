CREATE PROCEDURE Proc_TelephoneTransactionImportItems_Insert  
(  
@ImportGUID uniqueidentifier,  
@StrImportXML xml,  
@ImportStatus int  
)  
AS  
BEGIN  
 DECLARE @CreatedDate [datetime]    
    SET @CreatedDate = GETDATE();   
      
    DECLARE @Guid uniqueIdentifier   
    SET @Guid=@ImportGUID  
      
    Declare @XmlDataHandle int    
 Exec sp_xml_preparedocument @XmlDataHandle OUTPUT,@StrImportXML            
        
   
-- Execute a SELECT statement using OPENXML rowset provider.  
SELECT * INTO #tmpTelephoneTrasactionImportTable  
FROM OPENXML (@XmlDataHandle, '/TelephoneTransactions/TelephoneTransaction',2)  
WITH TelephoneTransactionImportItems  
  
 UPDATE #tmpTelephoneTrasactionImportTable    
    SET ImportStatus = 0,CreatedDate = GETDATE() ,ImportGuid = @Guid   
    
INSERT INTO [dbo].TelephoneTransactionImportItems    
select * from #tmpTelephoneTrasactionImportTable  
  
DROP TABLE #tmpTelephoneTrasactionImportTable  
   
  
END
