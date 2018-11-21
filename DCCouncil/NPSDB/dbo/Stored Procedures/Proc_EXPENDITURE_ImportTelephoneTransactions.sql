--select * from TelephoneTransactionImportItems order by itemid desc    
    
CREATE PROCEDURE [dbo].[Proc_EXPENDITURE_ImportTelephoneTransactions]    
(          
  @ImportGuid uniqueidentifier,      
  @FiscalYearID bigint,  
  @CommentTxtForImport nvarchar(50)     
)      
AS      
BEGIN       
       
 -- Inserting the imported items to a temp table for that instance      
 SELECT ItemID,ImportGuid,FoundationAccount,BillingAccount,WirelessNumber as ImportWirelessNumber,UserName,MarketCycleEndDate,TotalKBUSage,TotalNumberOfEvents,TotalMOUUsage,TotalCurrentCharges,ImportStatus,CreatedDate as ImportDate,UpdatedDate as ImportUpdatedDate     
 INTO #tmpTelephoneTransactionTable FROM TelephoneTransactionImportItems WHERE ImportGuid = @ImportGuid      
       
       
 DECLARE  @COUNT INT      
 DECLARE @ExpCategory nvarchar(100)      
 DECLARE @ExpCategoryId bigint      
 DECLARE @VendorName nvarchar(100)      
 DECLARE @Description nvarchar(250)      
 DECLARE @OBJCode nvarchar(100)      
 DECLARE @ExpenditureID int      
 DECLARE @FiscalYearStartDate datetime      
 DECLARE @FiscalYearEndDate datetime      
 DECLARE @MarketCycleEndDate datetime      
      
 select @ExpCategory=name,@ExpCategoryId=ExpenditureCategoryID from expenditurecategories where Code = 'TC'      
 select @VendorName =AttributeValue from ExpenditureCategoryAttributes WHERE ExpenditureCategoryId =@ExpCategoryId AND      
 ExpenditureCategoryAttributeLookupID = (SELECT ExpenditureCategoryAttributeLookupID from ExpenditureCategoryAttributeLookups WHERE Name = 'FixedVendorName')      
 select @Description =AttributeValue from ExpenditureCategoryAttributes WHERE ExpenditureCategoryId =@ExpCategoryId AND      
 ExpenditureCategoryAttributeLookupID = (SELECT ExpenditureCategoryAttributeLookupID from ExpenditureCategoryAttributeLookups WHERE Name = 'FixedDescription')      
 select @OBJCode =AttributeValue from ExpenditureCategoryAttributes WHERE ExpenditureCategoryId =@ExpCategoryId AND      
 ExpenditureCategoryAttributeLookupID = (SELECT ExpenditureCategoryAttributeLookupID from ExpenditureCategoryAttributeLookups WHERE Name = 'FixedOBJCode')      
 SELECT @FiscalYearStartDate=StartDate,@FiscalYearEndDate = EndDate From FiscalYears where FiscalYearID=@FiscalYearID      
       
-- to remove      
      
    
-- IMPORTSTATUS in TelephoneTransactionImportItems      
--* not imported = 0*      
--* imported = 1*      
--* user does not exist = 2*      
--* more than one user exist with same username= 3*      
--*not in current fiscal year= 4*      
--*Budget not specified= 5*      
--*Import date not month end= 6*      
       
 DECLARE @WirelessNumber Nvarchar(250)      
 DECLARE @ImportWirelessNumber CURSOR      
 SET @ImportWirelessNumber = CURSOR FOR      
 SELECT ImportWirelessNumber      
 FROM #tmpTelephoneTransactionTable      
 OPEN @ImportWirelessNumber      
 FETCH NEXT      
 FROM @ImportWirelessNumber INTO @WirelessNumber      
 WHILE @@FETCH_STATUS = 0      
 BEGIN      
  IF OBJECT_ID('tempdb..#tmpStaffTable') is not null      
   DROP TABLE #tmpStaffTable      
   SELECT * into #tmpStaffTable from STAffs S INNER JOIN  #tmpTelephoneTransactionTable TT ON S.WirelessNumber= TT.ImportWirelessNumber     
    WHERE S.WirelessNumber= @WirelessNumber AND S.IsDeleted =0 AND S.HasStaffLevelExpenditures =1       
     --AND TT.MarketCycleEndDate BETWEEN S.ActiveFrom AND S.ActiveTo        
   --select * from #tmpStaffTable      
         
   SELECT @COUNT = COUNT(FirstName) FROM #tmpStaffTable       
   --select @COUNT    
 IF @COUNT <> 1      
  BEGIN       
   IF @COUNT = 0      
    BEGIN      
   UPDATE TelephoneTransactionImportItems SET Importstatus = 2 WHERE WirelessNumber = @WirelessNumber  AND ImportGuid = @ImportGuid        
   FETCH NEXT      
   FROM @ImportWirelessNumber INTO @WirelessNumber      
   CONTINUE      
    END       
   ELSE      
    BEGIN      
   UPDATE TelephoneTransactionImportItems SET Importstatus = 3 WHERE WirelessNumber = @WirelessNumber  AND ImportGuid = @ImportGuid        
   FETCH NEXT      
   FROM @ImportWirelessNumber INTO @WirelessNumber      
   CONTINUE      
   END      
  END      
 ELSE      
  BEGIN       
   SELECT  @MarketCycleEndDate = MarketCycleEndDate from #tmpStaffTable T WHERE T.WirelessNumber = @WirelessNumber      
  IF (@MarketCycleEndDate BETWEEN @FiscalYearStartDate AND @FiscalYearEndDate)      
  BEGIN      
        
   declare @DATE datetime      
   declare @DATEEND datetime      
   set @DATE = @MarketCycleEndDate      
   set @DATEEND= (Select dateadd(day, 0 - day(dateadd(month, 1 , @DATE)), dateadd(month, 1 , @DATE)) )      
      
   IF @DATEEND <> @DATE      
   UPDATE TelephoneTransactionImportItems SET Importstatus = 6 WHERE WirelessNumber = @WirelessNumber AND ImportGuid = @ImportGuid         
   ELSE      
   BEGIN      
         
   SELECT @COUNT = COUNT(DateOfTransaction) FROM expenditures E INNER JOIN #tmpStaffTable T ON E.OfficeID = T.OfficeID AND E.DateOfTransaction = T.MarketCycleEndDate AND E.ExpenditureCategoryID = @ExpCategoryId AND T.WirelessNumber = @WirelessNumber and 
   
   
   E.IsDeleted =0      
   SELECT @COUNT as TotalCount      
   IF (@COUNT <> 0)      
    BEGIN      
          
    -- to remove      
    --SELECT 'Count not 0' as CountStatus      
     DECLARE @StaffLevelAmount float      
     DECLARE @ExpenditureAmount float      
     DECLARE @CurrentAmount float      
            
     SELECT @ExpenditureID = expenditureid from Expenditures E INNER JOIN #tmpStaffTable T ON E.OfficeID = T.OfficeID  AND E.DateOfTransaction = T.MarketCycleEndDate AND E.ExpenditureCategoryID = @ExpCategoryId AND T.WirelessNumber = @WirelessNumber     
     and E.IsDeleted = 0      
       
              
        IF EXISTS(SELECT S.STaffID FROM StaffLevelExpenditures S INNER JOIN #tmpStaffTable T  ON ExpenditureID = @ExpenditureID AND S.StaffID = T.StaffID)      
      BEGIN      
       -- to remove      
      --SELECT 'StaffExist' as StaffStatus      
      --SELECT * FROM #tmpStaffTable      
            
      select @ExpenditureAmount =  Amount from Expenditures WHERE expenditureId =@ExpenditureID      
      select @StaffLevelAmount = amount from StaffLevelExpenditures S INNER JOIN  #tmpStaffTable T  ON ExpenditureID = @ExpenditureID AND S.StaffID = T.StaffID      
    ---- to remove      
      --SELECT @ExpenditureAmount as EXPAMount      
      --SELECT @StaffLevelAmount as STaffAmount      
            
      UPDATE StaffLevelExpenditures SET Amount = T.TotalCurrentCharges,UpdatedDate = GETDATE() FROM #tmpStaffTable T    
       WHERE StaffLevelExpenditureID IN (SELECT StaffLevelExpenditureID FROM StaffLevelExpenditures S INNER JOIN  #tmpStaffTable T ON S.ExpenditureID = @ExpenditureID AND S.StaffID = T.StaffID)      
      --select @CurrentAmount = TotalCurrentCharges from #tmpStaffTable      
      select @CurrentAmount = SUM(AMOUNT) from StaffLevelExpenditures  WHERE expenditureId =@ExpenditureID      
         
      SELECT @CurrentAmount as Currentamount      
            
      UPDATE Expenditures SET Amount = @CurrentAmount,UPDATEDDATE = GETDATE() WHERE expenditureId =@ExpenditureID      
            
      --UPDATE Expenditures SET Amount =(@ExpenditureAmount - @StaffLevelAmount + @CurrentAmount )  WHERE expenditureId =@ExpenditureID      
      END      
     ELSE      
      BEGIN      
      select @ExpenditureAmount =  Amount from Expenditures WHERE expenditureId =@ExpenditureID      
      select @CurrentAmount = TotalCurrentCharges from #tmpStaffTable      
    -- to remove      
      --SELECT @ExpenditureAmount as EXPAMount      
      --SELECT @CurrentAmount as Currentamount      
      --SELECT * FROM #tmpStaffTable      
      --SELECT 'yoyo'      
      INSERT INTO StaffLevelExpenditures ([ExpenditureID], [StaffID], [Amount], [CreatedDate], [UpdatedDate])        
      SELECT @ExpenditureID,T.StaffID,@CurrentAmount,GETDATE(),NULL       
      FROM #tmpStaffTable T       
            
      UPDATE Expenditures SET Amount =(@ExpenditureAmount +@CurrentAmount ),UpdatedDate = GETDATE()  WHERE expenditureId =@ExpenditureID      
    END       
           
      select @WirelessNumber as WirelessNumber    
    UPDATE TelephoneTransactionImportItems SET IMportstatus =1 WHERE WirelessNumber = @WirelessNumber  AND ImportGuid = @ImportGuid        
          
    END      
   ELSE      
    BEGIN      
  -- to remove      
    --SELECT 2      
    --select @FiscalYearID as FISCALYEarID      
    DECLARE @BudgetId [bigint]      
       SELECT @BudgetId = Budgetid from budgets B where Officeid = (SELECT OFFICEID From  #tmpStaffTable T ) and IsDefault =1 and B.FiscalYearID = @FiscalYearID      
       IF EXISTS(SELECT Budgetid from budgets B where Officeid = (SELECT OFFICEID From  #tmpStaffTable T ) and IsDefault =1 and B.FiscalYearID = @FiscalYearID)       
       BEGIN      
       INSERT INTO Expenditures       
       SELECT @ExpCategoryId,@VendorName,@Description,@OBJCode,T.MarketCycleEndDate,T.TotalCurrentCharges,T.OfficeId,@CommentTxtForImport,@FiscalYearID,@BudgetId,0,GETDATE(),Null      
       FROM #tmpStaffTable T       
            
    SET @ExpenditureId = @@IDENTITY       
    INSERT INTO StaffLevelExpenditures SELECT @ExpenditureID,T.StaffID,T.TotalCurrentCharges,GETDATE(),NULL      
    FROM #tmpStaffTable T       
    UPDATE TelephoneTransactionImportItems SET IMportstatus =1 WHERE WirelessNumber = @WirelessNumber AND ImportGuid = @ImportGuid       
    END      
    ELSE      
    BEGIN      
    UPDATE TelephoneTransactionImportItems SET IMportstatus =5 WHERE WirelessNumber = @WirelessNumber AND ImportGuid = @ImportGuid      
          
    END      
    END      
    END      
   END       
  ELSE      
  UPDATE TelephoneTransactionImportItems SET Importstatus = 4 WHERE WirelessNumber = @WirelessNumber AND ImportGuid = @ImportGuid       
 END      
        
       
       
 FETCH NEXT      
 FROM @ImportWirelessNumber INTO @WirelessNumber      
 END      
 CLOSE @ImportWirelessNumber      
 DEALLOCATE @ImportWirelessNumber      
       
END 