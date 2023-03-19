create database QuanLyQuanCafe
go

use QuanLyQuanCafe
go

-- Food
-- Table
-- FoodCategory
-- Account
-- Bill
-- BillInfo

create table TableFood
(
	id int identity primary key,
	name nvarchar(100) not null default N'Bàn chưa có tên',
	status nvarchar(100) default N'Trống' not null
)
go

create table Account
(
	UserName nvarchar(100) primary key,
	DisplayName nvarchar(100) not null default N'User',
	Password nvarchar(1000) not null default 0,
	Type int not null default 0
)
go

create table FoodCategory
(
	id int identity primary key,
	name nvarchar(100) not null default N'Chưa đặt tên'
)
go

create table Food
(
	id int identity primary key,
	name nvarchar(100)not null default N'Chưa đặt tên',
	idCategory int not null,
	price float not null default 0

	foreign key (idCategory) references dbo.FoodCategory(id)
)
go

create table Bill
(
	id int identity primary key,
	DateCheckIn Date not null default getdate(),
	DateCheckOut Date,
	idTable int not null,
	status int not null default 0

	foreign key (idTable) references dbo.TableFood(id)
)
go

create table BillInfo
(
	id int identity primary key,
	idBill int not null,
	idFood int not null,
	count int not null default 0
	
	foreign key (idBill) references dbo.Bill(id),
	foreign key (idFood) references dbo.Food(id)
)
go


create procedure USP_GetAccountByUserName
@userName nvarchar(100)
as
begin
	select * from dbo.Account where UserName = @userName
end
go

create proc usp_gettablelist
as select * from dbo.TableFood
go

update dbo.TableFood set status = 'Có người' where id = 9;
go

create proc usp_InsertBill
@idTable int
as
begin
	insert Bill
	(DateCheckIn,
	DateCheckOut,
	idTable,
	status,
	discount
	)
	values
	(getdate(),
	null,
	@idTable,
	0,
	0
	)
end
go


create proc USP_InsertBillInfo
@idBill int, @idFood int, @count int
as
begin
	declare @isExitsBillInfo int ;
	declare @foodCount int = 1;

	select @isExitsBillInfo = id, @foodCount = b.count 
	from dbo.BillInfo as b 
	where idBill=@idBill and idFood=@idFood

	if(@isExitsBillInfo > 0)
	begin
		declare @newCount int = @foodCount + @count
		if(@newCount > 0)
			update BillInfo set count = @foodCount + @count where idFood=@idFood
		else
			delete BillInfo where idBill = @idBill and idFood = @idFood
	end
	else
	begin
		insert dbo.BillInfo
			(idBill, idFood, count)
		values(@idBill,
			@idFood,
			@count
			)
	end
end
go

create trigger UTG_UpdateBillInfo
on BillInfo for insert, update
as
begin
	declare @idBill int

	select @idBill=idBill from Inserted

	declare @idTable int

	select @idTable = idTable from Bill where id=@idBill and status =0

	update TableFood set status = N'Có người' where id = @idTable
end
go

create trigger UTG_UpdateBill
on Bill for update
as
begin
	declare @idBill int

	select @idBill = id from Inserted

	declare @idTable int

	select @idTable = idTable from Bill where id=@idBill

	declare @count int = 0

	select @count = count(*) from Bill where idTable = @idTable and status = 0

	if(@count = 0)
		update TableFood set status = N'Trống' where id = @idTable
end
go

alter table Bill
add totalPrice float

alter table Bill
add discount int

create proc USP_GetListBillByDate
@checkIn date, @checkOut date
as
begin
	select t.name as [Tên bàn], b.totalPrice as [Tổng tiền], DateCheckIn as [Ngày vào], DateCheckOut as [Ngày ra], discount as [Giảm giá]
	from Bill as b, TableFood as t
	where DateCheckIn >= @checkIn and DateCheckOut <= @checkOut and b.status=1
	and t.id=b.idTable
end
go

create proc USP_UpdateAccount
@userName nvarchar(100), @password nvarchar(100), @newPassword nvarchar(100)
as
begin
	declare @isRightPass int

	select @isRightPass = count(*) from Account where UserName = @userName and Password = @password

	if(@isRightPass = 1)
	begin
		if(@newPassword is not null or @newPassword <> '')
		begin
			update Account set Password = @newPassword where UserName = @userName
		end
	end
end
go

create trigger UTG_DeleteBillInfo
on BillInfo for delete
as
begin
	declare @idBillInfo int
	declare @idBill int
	select @idBillInfo = id, @idBill = deleted.idBill from deleted

	declare @idTable int
	select @idTable = idTable from Bill where id = @idBill

	declare @count int = 0

	select @count = count(*) from BillInfo as bi, Bill as b where b.id = bi.idBill and b.id = @idBill and b.status = 0;

	if(@count = 0 )
		update TableFood set status = N'Trống' where id = @idTable
end
go

select * from Food