create proc Proc_PURCHASEORDER_RemoveInvalidRecords(@ImportGUID [uniqueidentifier])

as begin




	Declare @ImportID bigint

	Declare @OfficeID bigint

	Declare @ActiveFrom datetime

	Declare @ActiveTo datetime




	select @ImportID=ImportID 

	from PurchaseOrderImports 

	where ImportGUID=@ImportGUID




	Declare cursor_office cursor for select OfficeID, ActiveFrom, ActiveTo from Offices

	open cursor_office

	fetch next from cursor_office

	into @OfficeID, @ActiveFrom, @ActiveTo

	WHILE @@FETCH_STATUS = 0

	BEGIN




	delete from PurchaseOrders where importid=@ImportID and Officeid=@OfficeID and DateOfTransaction<@ActiveFrom




	if @ActiveTo is not null

	delete from PurchaseOrders where importid=@ImportID and Officeid=@OfficeID and DateOfTransaction>@ActiveTo




	fetch next from cursor_office

	into @OfficeID, @ActiveFrom, @ActiveTo




	END







	close cursor_office

	deallocate cursor_office

end