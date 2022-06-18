select * from YieldDB.dbo.[AspNetUserLogins]

select * from YieldDB.[dbo].[AspNetUsers]

select * from YieldDB.[dbo].[UserAccount]

delete YieldDB.[dbo].[UserAccount]

/******************************************************/

/*****************************************************************************
MVC Account Tables
******************************************************************************/
/*
drop table [dbo].[AspNetRoles]
drop table [dbo].[AspNetUserClaims]
drop table [dbo].[AspNetUserLogins]
drop table [dbo].[AspNetUserRoles]
drop table [dbo].[AspNetUsers]
*/

/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 1/12/2019 8:38:50 PM ******/
USE [YieldDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 1/12/2019 8:37:32 PM ******/
USE [YieldDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 1/12/2019 8:37:55 PM ******/
USE [YieldDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO

/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 1/12/2019 8:38:13 PM ******/
USE [YieldDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO

/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 1/12/2019 8:38:29 PM ******/
USE [YieldDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO


/*****************************************************************************
SY Tables
******************************************************************************/
/*
USE [YieldDB]
GO

drop table FAQ
drop table AccountInvestment
drop table AccountTransaction
drop table InvestmentHistory
drop table ContactUs
drop table UserMsg
drop table Investment
drop table InvestmentType
drop table UserAccount
drop table PaymentMethod

delete from AspNetUsers
truncate table FAQ
truncate table AccountInvestment
truncate table AccountTransaction
truncate table InvestmentHistory
truncate table ContactUs
truncate table UserMsg
truncate table Investment
truncate table InvestmentType
truncate table UserAccount
truncate table PaymentMethod

*/

USE [YieldDB]
GO

create table InvestmentType
(
[Id] int identity primary key,
[Title] varchar(100),
[Description] varchar(500)
)
GO

create table TransactionType
(
[Id] int identity primary key,
[Title] varchar(100),
[Description] varchar(500)
)
GO

create table PaymentMethod
(
[Id] int identity primary key,
[Title] varchar(100),
[Description] varchar(500)
)
GO

create table AccountStatus
(
[Id] int identity primary key,
[Title] varchar(100),
[Description] varchar(500)
)
GO

update UserAccount set [Status]=1  
select * from AccountStatus
--alter table UserAccount add [Status] int default 1
create table UserAccount --need account for incoming deposit
(
[Id] int identity primary key,
[Type] int default 1,  --1 = user, 2 = admin
[UserId] nvarchar(128),
[FirstName] varchar(128),
[MiddleName] varchar(128),
[LastName] varchar(128),
[Company] varchar(128),
[Addr1] varchar(128),
[Addr2] varchar(128),
[City] varchar(128),
[State] char(2),
[Zip] varchar(12),
[Country] varchar(50),
[Email] varchar(100),
[Phone1] varchar(15),
[Phone2] varchar(15),
[PaymentMethod] int foreign key references PaymentMethod(Id),
[BankRouting] varchar(15),
[BankAccount] varchar(50),
[Status] int default 1,
[Deleted] bit not null default 0,
[CreateUser] int foreign key references UserAccount(Id),
[CreateDate] datetime default getdate(),
[UpdateUser] int foreign key references UserAccount(Id),
[UpdateDate] datetime
)
GO

create table Investment  --one investment must be cash/settlement, another for incoming deposit
(
[Id] int identity primary key,
[Type] int foreign key references InvestmentType(Id),
[Title] varchar(100),
[Description] varchar(2500),
[IRR] decimal(20,5),
[InceptionDate] datetime,
[LockPeriodDays] int,
[Status] int default 0,  --1=active
[Deleted] bit not null default 0,
[CreateUser] int foreign key references UserAccount(Id),
[CreateDate] datetime default getdate(),
[UpdateUser] int foreign key references UserAccount(Id),
[UpdateDate] datetime
)
GO

select * from Investment
update Investment set [status]=1 where [status] is null
alter table Investment add [Status] int default 0

create table FAQ
(
[Id] int identity primary key,
[Question] varchar(100),
[Answer] varchar(500),
[Deleted] bit not null default 0,
[CreateUser] int foreign key references UserAccount(Id),
[CreateDate] datetime default getdate(),
[UpdateUser] int foreign key references UserAccount(Id),
[UpdateDate] datetime
)
GO

select * from AccountInvestment
update AccountInvestment set [Discount] = 10
update AccountInvestment set [Value] = 55.55, ValueDate = getdate(), discount=0 where [Value] is null

alter table AccountInvestment add [Discount] decimal(5,2) default 0
alter table AccountInvestment add [LockStartDate] datetime

create table AccountInvestment
(
[Id] int identity primary key,
[AccountId] int foreign key references UserAccount(Id),
[InvestmentId] int foreign key references Investment(Id),
[BuyAmt] decimal(20,5),
[BuyDate] datetime,
[SellDate] datetime,
[Value] decimal(20,5),
[ValueDate] datetime,
[LockStartDate] datetime,
[ForSale] bit default 0,
[Discount] decimal(5,2) default 0,
[Deleted] bit not null default 0,
[CreateUser] int foreign key references UserAccount(Id),
[CreateDate] datetime default getdate(),
[UpdateUser] int foreign key references UserAccount(Id),
[UpdateDate] datetime
)
GO
select * from USerAccount
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'',)

create table AccountValueHistory
(
[Id] int identity primary key,
[AccountId] int foreign key references UserAccount(Id),
[Value] decimal(20,5),
[ValueDate] datetime,
[Deleted] bit not null default 0,
[CreateUser] int foreign key references UserAccount(Id),
[CreateDate] datetime default getdate(),
[UpdateUser] int foreign key references UserAccount(Id),
[UpdateDate] datetime
)


update AccountTransaction set [Cost] = Amount

alter table AccountTransaction add [Cost] decimal(20,5)
alter table AccountTransaction add [Discount] decimal(20,5)

create table AccountTransaction
(
[Id] int identity primary key,
[Type] int foreign key references TransactionType(Id),
[Amount] decimal(20,5),
[Discount] decimal(5,2),
[Cost] decimal(20,5),
[FromAccount] int foreign key references UserAccount(Id),
[FromInvestment] int foreign key references Investment(Id),
[ToAccount] int foreign key references UserAccount(Id),
[ToInvestment] int foreign key references Investment(Id),
[Deleted] bit not null default 0,
[CreateUser] int foreign key references UserAccount(Id),
[CreateDate] datetime default getdate(),
[UpdateUser] int foreign key references UserAccount(Id),
[UpdateDate] datetime
)
GO

create table InvestmentHistory
(
[Id] int identity primary key,
[InvestmentId] int foreign key references Investment(Id),
[InvestmentValue] decimal(20,5),
[TotalInvestment] decimal(20,5),
[InvestorCount] int,
[ValueDate] datetime,
[Deleted] bit not null default 0,
[CreateUser] int foreign key references UserAccount(Id),
[CreateDate] datetime default getdate(),
[UpdateUser] int foreign key references UserAccount(Id),
[UpdateDate] datetime
)
GO

create table ContactUs
(
[Id] int identity primary key,
[UserId] int foreign key references UserAccount(Id),
[FirstName] varchar(128),
[LastName] varchar(128),
[Email] varchar(128),
[Phone] varchar(128),
[Title] varchar(128),
[Msg] varchar(1000),
[Deleted] bit not null default 0
)
GO

create table UserMsg
(
[Id] int identity primary key,
[UserId] nvarchar(128) foreign key references AspNetUsers(Id),
[Title] varchar(128),
[Msg] varchar(1000),
[Deleted] bit not null default 0,
[CreateUser] int foreign key references UserAccount(Id),
[CreateDate] datetime default getdate(),
[UpdateUser] int foreign key references UserAccount(Id),
[UpdateDate] datetime
)



alter table ContactUs add [Deleted] bit not null default 0
alter table UserMsg add [Deleted] bit not null default 0
alter table InvestmentHistory add [Deleted] bit not null default 0
alter table AccountInvestment add [Deleted] bit not null default 0
alter table TransactionType add [Deleted] bit not null default 0
alter table AccountTransaction add [Deleted] bit not null default 0
alter table AccountStatus add [Deleted] bit not null default 0
alter table InvestmentType add [Deleted] bit not null default 0
alter table PaymentMethod add [Deleted] bit not null default 0
alter table UserAccount add [Deleted] bit not null default 0
alter table Investment add [Deleted] bit not null default 0
alter table FAQ add [Deleted] bit not null default 0

use master
GRANT VIEW ANY DATABASE TO SYWeb; --can't deny

use YieldDB
GRANT SELECT TO SYWeb
GRANT INSERT TO SYWeb
GRANT UPDATE TO SYWeb

use master
GRANT VIEW ANY DATABASE TO SYConsole;  --can't deny

use YieldDB
GRANT SELECT TO SYConsole
GRANT INSERT TO SYConsole
GRANT UPDATE TO SYConsole

create table tmpx ( xxx int )


use master

select * from sys.databases

select * from InvestmentHistory where InvestmentId=2
select * from AccountInvestment
select * from Investment

update InvestmentHistory set InvestmentValue=100 where InvestmentId=2

select * from sysobjects where xtype='U'

/*
create table Fund
(
[Id] int identity primary key,
[UserId] nvarchar(128) foreign key references AspNetUsers(Id),
[PaymentMethod] int foreign key references PaymentMethod(Id),
[BankRouting] varchar(15),
[BankAccount] varchar(50),
[CCNumber] varchar(50),
[CreateUser] int foreign key references UserAccount(Id),
[CreateDate] datetime default getdate(),
[UpdateUser] int foreign key references UserAccount(Id),
[UpdateDate] datetime
)
*/
update UserAccount set [Type] = 1 where [Type] = 0
/**** SEED DATA****/

insert AccountStatus ([Title], [Description]) values
('Active', 'Active'), --1
('Suspended', 'Suspended'), --2
('Locked', 'Locked'), --3
('Closed', 'Closed') --4

insert TransactionType ([Title], [Description]) values
('Deposit', 'Deposit To Settlement Account'), --1
('Deposit Request', 'Request Deposit To Settlement Account'), --2
('Withdraw', 'Withdraw From Settlement Account'), --3
('Withdraw Request', 'Request Withdraw From Settlement Account'), --4
('Transfer', 'Transfer Investment'), --5
('Post Sell', 'Post Investment For Sale'), --6
('Cancel Sell', 'Cancel For Sale') --7,
('Adjustment', 'Admin adjustment') --8

insert InvestmentType ([Title], [Description]) values
('Internal', 'Deposits and Settlement Accounts'),  --1
('Business Loans', 'Loans to small businesses'),  --2
('Pre-Settlement', 'Pre-Settlement Funding'), --3
('Real Estate', 'Real Estate Investment'), --4
('Shipping', 'Commercial Vessels'), --5
('Finance', 'Alternate Financial Vehicles'), --6
('Insurance', 'Insurance based investments'), --7
('Other', 'Other') --8

insert PaymentMethod ([Title], [Description]) values
('Bank Deposit', 'Provide Bank routing and account number'),
('Cash', 'Small non-sequencial bills preferred'),
('Gold Bullion', 'Any type of unmarked gold. Must be at least 99.5% pure'),
('Bitcoin', 'Good luck with this'),
('Credit Card', 'Not real money')

insert Investment ([Type], [Title], [Description], [IRR], [InceptionDate], [LockPeriodDays], [Closed]) values
(1, 'Deposit', 'Incoming Deposit', 0, null, 0, 0),  --1
(1, 'Settlement (Cash)', 'Settlement Account', 0, null, 0, 0),  --2
(3, 'Cash4Accident', 'Pre-settlement funding', 13, getdate(), 500, 0), --3
(4, 'Moon Condos', 'Building condos on the moon', 25, getdate(), 600, 0),
(4, 'Aqua basements', 'Specializing in house boat basements', 25, getdate(), 200, 0),
(5, 'Heroin Imports', 'Smuggling narcotics from South America to USA', 150, getdate(), 140, 0),
(6, 'Failing Banks', 'Buy failing bank stock and wait for a government bailout', 200, getdate(), 123, 0),
(7, 'Protection', 'Mob protection in major US cities', 25, getdate(), 50, 0),
(8, 'Piracy', 'Financing Somalia pirates in exchange for a portion of the booty', 35, getdate(), 55, 0)

update Investment set [Type] = 3, Title = 'Litigation', Description = 'Pre-settlement funding' where Id = 3
update Investment set [Type] = 4, Title = 'Real Estate', Description = 'Commercial and residential real estate' where Id = 4
update Investment set [Type] = 2, Title = 'Lending', Description = 'Merchant and Securities based lending' where Id = 5
update Investment set [Type] = 2, Title = 'Retail & Franchise', Description = 'Retail stores and franchises' where Id = 6
update Investment set [Type] = 6, Title = 'Factoring', Description = 'Purchasing debt from other companies' where Id = 7
update Investment set [Type] = 6, Title = 'Opportunity Fund', Description = 'Exploring new investments' where Id = 8



insert FAQ ([Question], [Answer]) values 
('Is my principal investment safe?', 'Yes. Some of your investment is safe - The part we don''t lose.'),
('What is the minimum investment amount?', '27¢ is all it takes to open an account. With proper investments, that can turn into a full dollar in 20 short years.'),
('What payment types do you accept?', 'Super Yield accepts most currencies including USD, EUR, JPY, Gold Krugerrand, Cowrie shells, Rai stones, Disney dollars, and animal pelts.'),
('Some of these investments seem illegal. If I invest, will I go to prison?', 'A prison sentence is possible, but we guarantee your sentence will be less than 2 years. In addition, while you''re in the bighouse taking it in the ass, you can find comfort knowing your investment is safe and growing.')

sp_password NULL,'superyield','sa'

select * from FAQ
select * from InvestmentHistory

delete InvestmentHistory where ValueDate > '2019-4-10'

select * from [dbo].[Investment]
select * from [dbo].[InvestmentHistory]
select * from [dbo].[AccountInvestment]
select * from [dbo].[AccountTransaction]
delete [AccountInvestment] where buyamt=0
select * from ContactUs

update [Investment] set Title = 'Settlement (Cash)' where Title = 'Settlement'

select * from [dbo].[InvestmentType]
select * from [dbo].[TransactionType]

select * from PaymentMethod
select * from FAQ

delete [AccountInvestment] where AccountId = 4
delete [UserAccount] where UserId is null

--update [UserAccount] set UserId= '9d59c258-167c-496a-9e6a-2309aad0b911' where UserId is null
update [UserAccount] set Type=2, Status=1 where Id=1

select * from [dbo].[AspNetUsers]

select * from [dbo].[UserAccount]

delete from [dbo].[UserAccount] where Id = 2

set identity_insert [UserAccount] off
update [UserAccount] set Id=1

DBCC CHECKIDENT('UserAccount', RESEED, 0)

update [AccountInvestment] set BuyAmt = 10000, Value = 20000 where id=60

delete [AccountInvestment]
-- settlement
insert [dbo].[AccountInvestment] (AccountId,InvestmentId,BuyAmt,BuyDate,SellDate,ForSale)
values (1,2,25000,'2019-02-08',null,0)
-- type 3
insert [dbo].[AccountInvestment] (AccountId,InvestmentId,BuyAmt,BuyDate,SellDate,ForSale)
values (1,3,10000,'2019-02-08',null,0)
-- type 4
insert [dbo].[AccountInvestment] (AccountId,InvestmentId,BuyAmt,BuyDate,SellDate,ForSale)
values (1,4,15000,'2019-02-08',null,0)


-- settlement - user1
insert [dbo].[AccountInvestment] (AccountId,InvestmentId,BuyAmt,BuyDate,SellDate,ForSale)
values (3,2,25000,'2019-02-08',null,0)

INSERT INTO [dbo].[AccountTransaction] ([Type],[Amount],[FromAccount],[FromInvestment],[ToAccount],[ToInvestment])
     VALUES
	 (1,100000,1,1,1,2),
	 (1,25000 ,3,1,3,2)
GO

select * from [dbo].[AspNetUsers]

/*
Azure DB
syadmin \ SuperYield1

Data Source=tcp:yielddb.database.windows.net,1433;Initial Catalog=yielddb;User Id=syadmin@yielddb.database.windows.net;Password=SuperYield1;
*/

update [InvestmentHistory] set [InvestmentValue] = 100 where [InvestmentId]=2

insert [dbo].[InvestmentHistory] ([InvestmentId],[InvestmentValue],[InvestorCount],[ValueDate])
values 
(2,100,1,'2019-01-01'),	(3,200,1,'2019-01-01'),	(4,300,1,'2019-01-01'),	(5,400,1,'2019-01-01'),
(2,101,1,'2019-01-02'),	(3,201,1,'2019-01-02'),	(4,301,1,'2019-01-02'),	(5,401,1,'2019-01-02'),
(2,102,1,'2019-01-03'),	(3,202,1,'2019-01-03'),	(4,302,1,'2019-01-03'),	(5,402,1,'2019-01-03'),
(2,103,1,'2019-01-04'),	(3,203,1,'2019-01-04'),	(4,303,1,'2019-01-04'),	(5,403,1,'2019-01-04'),
(2,104,1,'2019-01-05'),	(3,204,1,'2019-01-05'),	(4,304,1,'2019-01-05'),	(5,404,1,'2019-01-05'),
(2,105,1,'2019-01-06'),	(3,205,1,'2019-01-06'),	(4,305,1,'2019-01-06'),	(5,405,1,'2019-01-06'),
(2,106,1,'2019-01-07'),	(3,206,1,'2019-01-07'),	(4,306,1,'2019-01-07'),	(5,406,1,'2019-01-07'),
(2,107,1,'2019-01-08'),	(3,207,1,'2019-01-08'),	(4,307,1,'2019-01-08'),	(5,407,1,'2019-01-08'),
(2,108,1,'2019-01-09'),	(3,208,1,'2019-01-09'),	(4,308,1,'2019-01-09'),	(5,408,1,'2019-01-09'),
(2,109,1,'2019-01-10'),	(3,209,1,'2019-01-10'),	(4,309,1,'2019-01-10'),	(5,409,1,'2019-01-10'),
(2,110,1,'2019-01-11'),	(3,210,1,'2019-01-11'),	(4,310,1,'2019-01-11'),	(5,410,1,'2019-01-11'),
(2,111,1,'2019-01-12'),	(3,211,1,'2019-01-12'),	(4,311,1,'2019-01-12'),	(5,411,1,'2019-01-12'),
(2,112,1,'2019-01-13'),	(3,212,1,'2019-01-13'),	(4,312,1,'2019-01-13'),	(5,412,1,'2019-01-13'),
(2,113,1,'2019-01-14'),	(3,213,1,'2019-01-14'),	(4,313,1,'2019-01-14'),	(5,413,1,'2019-01-14'),
(2,114,1,'2019-01-15'),	(3,214,1,'2019-01-15'),	(4,314,1,'2019-01-15'),	(5,414,1,'2019-01-15'),
(2,115,1,'2019-01-16'),	(3,215,1,'2019-01-16'),	(4,315,1,'2019-01-16'),	(5,415,1,'2019-01-16'),
(2,116,1,'2019-01-17'),	(3,216,1,'2019-01-17'),	(4,316,1,'2019-01-17'),	(5,416,1,'2019-01-17'),
(2,117,1,'2019-01-18'),	(3,217,1,'2019-01-18'),	(4,317,1,'2019-01-18'),	(5,417,1,'2019-01-18'),
(2,118,1,'2019-01-19'),	(3,218,1,'2019-01-19'),	(4,318,1,'2019-01-19'),	(5,418,1,'2019-01-19'),
(2,119,1,'2019-01-20'),	(3,219,1,'2019-01-20'),	(4,319,1,'2019-01-20'),	(5,419,1,'2019-01-20'),
(2,120,1,'2019-01-21'),	(3,220,1,'2019-01-21'),	(4,320,1,'2019-01-21'),	(5,420,1,'2019-01-21'),
(2,121,1,'2019-01-22'),	(3,221,1,'2019-01-22'),	(4,321,1,'2019-01-22'),	(5,421,1,'2019-01-22'),
(2,122,1,'2019-01-23'),	(3,222,1,'2019-01-23'),	(4,322,1,'2019-01-23'),	(5,422,1,'2019-01-23'),
(2,123,1,'2019-01-24'),	(3,223,1,'2019-01-24'),	(4,323,1,'2019-01-24'),	(5,423,1,'2019-01-24'),
(2,124,1,'2019-01-25'),	(3,224,1,'2019-01-25'),	(4,324,1,'2019-01-25'),	(5,424,1,'2019-01-25'),
(2,125,1,'2019-01-26'),	(3,225,1,'2019-01-26'),	(4,325,1,'2019-01-26'),	(5,425,1,'2019-01-26'),
(2,126,1,'2019-01-27'),	(3,226,1,'2019-01-27'),	(4,326,1,'2019-01-27'),	(5,426,1,'2019-01-27'),
(2,127,1,'2019-01-28'),	(3,227,1,'2019-01-28'),	(4,327,1,'2019-01-28'),	(5,427,1,'2019-01-28'),
(2,128,1,'2019-01-29'),	(3,228,1,'2019-01-29'),	(4,328,1,'2019-01-29'),	(5,428,1,'2019-01-29'),
(2,129,1,'2019-01-30'),	(3,229,1,'2019-01-30'),	(4,329,1,'2019-01-30'),	(5,429,1,'2019-01-30'),
(2,130,1,'2019-01-31'),	(3,230,1,'2019-01-31'),	(4,330,1,'2019-01-31'),	(5,430,1,'2019-01-31'),
(2,131,1,'2019-02-01'),	(3,231,1,'2019-02-01'),	(4,331,1,'2019-02-01'),	(5,431,1,'2019-02-01'),
(2,132,1,'2019-02-02'),	(3,232,1,'2019-02-02'),	(4,332,1,'2019-02-02'),	(5,432,1,'2019-02-02'),
(2,133,1,'2019-02-03'),	(3,233,1,'2019-02-03'),	(4,333,1,'2019-02-03'),	(5,433,1,'2019-02-03'),
(2,134,1,'2019-02-04'),	(3,234,1,'2019-02-04'),	(4,334,1,'2019-02-04'),	(5,434,1,'2019-02-04'),
(2,135,1,'2019-02-05'),	(3,235,1,'2019-02-05'),	(4,335,1,'2019-02-05'),	(5,435,1,'2019-02-05'),
(2,136,1,'2019-02-06'),	(3,236,1,'2019-02-06'),	(4,336,1,'2019-02-06'),	(5,436,1,'2019-02-06'),
(2,137,1,'2019-02-07'),	(3,237,1,'2019-02-07'),	(4,337,1,'2019-02-07'),	(5,437,1,'2019-02-07'),
(2,138,1,'2019-02-08'),	(3,238,1,'2019-02-08'),	(4,338,1,'2019-02-08'),	(5,438,1,'2019-02-08'),
(2,139,1,'2019-02-09'),	(3,239,1,'2019-02-09'),	(4,339,1,'2019-02-09'),	(5,439,1,'2019-02-09'),
(2,140,1,'2019-02-10'),	(3,240,1,'2019-02-10'),	(4,340,1,'2019-02-10'),	(5,440,1,'2019-02-10'),
(2,141,1,'2019-02-11'),	(3,241,1,'2019-02-11'),	(4,341,1,'2019-02-11'),	(5,441,1,'2019-02-11'),
(2,142,1,'2019-02-12'),	(3,242,1,'2019-02-12'),	(4,342,1,'2019-02-12'),	(5,442,1,'2019-02-12'),
(2,143,1,'2019-02-13'),	(3,243,1,'2019-02-13'),	(4,343,1,'2019-02-13'),	(5,443,1,'2019-02-13'),
(2,144,1,'2019-02-14'),	(3,244,1,'2019-02-14'),	(4,344,1,'2019-02-14'),	(5,444,1,'2019-02-14'),
(2,145,1,'2019-02-15'),	(3,245,1,'2019-02-15'),	(4,345,1,'2019-02-15'),	(5,445,1,'2019-02-15'),
(2,146,1,'2019-02-16'),	(3,246,1,'2019-02-16'),	(4,346,1,'2019-02-16'),	(5,446,1,'2019-02-16'),
(2,147,1,'2019-02-17'),	(3,247,1,'2019-02-17'),	(4,347,1,'2019-02-17'),	(5,447,1,'2019-02-17'),
(2,148,1,'2019-02-18'),	(3,248,1,'2019-02-18'),	(4,348,1,'2019-02-18'),	(5,448,1,'2019-02-18'),
(2,149,1,'2019-02-19'),	(3,249,1,'2019-02-19'),	(4,349,1,'2019-02-19'),	(5,449,1,'2019-02-19'),
(2,150,1,'2019-02-20'),	(3,250,1,'2019-02-20'),	(4,350,1,'2019-02-20'),	(5,450,1,'2019-02-20'),
(2,151,1,'2019-02-21'),	(3,251,1,'2019-02-21'),	(4,351,1,'2019-02-21'),	(5,451,1,'2019-02-21'),
(2,152,1,'2019-02-22'),	(3,252,1,'2019-02-22'),	(4,352,1,'2019-02-22'),	(5,452,1,'2019-02-22'),
(2,153,1,'2019-02-23'),	(3,253,1,'2019-02-23'),	(4,353,1,'2019-02-23'),	(5,453,1,'2019-02-23'),
(2,154,1,'2019-02-24'),	(3,254,1,'2019-02-24'),	(4,354,1,'2019-02-24'),	(5,454,1,'2019-02-24'),
(2,155,1,'2019-02-25'),	(3,255,1,'2019-02-25'),	(4,355,1,'2019-02-25'),	(5,455,1,'2019-02-25'),
(2,156,1,'2019-02-26'),	(3,256,1,'2019-02-26'),	(4,356,1,'2019-02-26'),	(5,456,1,'2019-02-26'),
(2,157,1,'2019-02-27'),	(3,257,1,'2019-02-27'),	(4,357,1,'2019-02-27'),	(5,457,1,'2019-02-27'),
(2,158,1,'2019-02-28'),	(3,258,1,'2019-02-28'),	(4,358,1,'2019-02-28'),	(5,458,1,'2019-02-28'),
(2,159,1,'2019-03-01'),	(3,259,1,'2019-03-01'),	(4,359,1,'2019-03-01'),	(5,459,1,'2019-03-01'),
(2,160,1,'2019-03-02'),	(3,260,1,'2019-03-02'),	(4,360,1,'2019-03-02'),	(5,460,1,'2019-03-02'),
(2,161,1,'2019-03-03'),	(3,261,1,'2019-03-03'),	(4,361,1,'2019-03-03'),	(5,461,1,'2019-03-03'),
(2,162,1,'2019-03-04'),	(3,262,1,'2019-03-04'),	(4,362,1,'2019-03-04'),	(5,462,1,'2019-03-04'),
(2,163,1,'2019-03-05'),	(3,263,1,'2019-03-05'),	(4,363,1,'2019-03-05'),	(5,463,1,'2019-03-05'),
(2,164,1,'2019-03-06'),	(3,264,1,'2019-03-06'),	(4,364,1,'2019-03-06'),	(5,464,1,'2019-03-06'),
(2,165,1,'2019-03-07'),	(3,265,1,'2019-03-07'),	(4,365,1,'2019-03-07'),	(5,465,1,'2019-03-07'),
(2,166,1,'2019-03-08'),	(3,266,1,'2019-03-08'),	(4,366,1,'2019-03-08'),	(5,466,1,'2019-03-08'),
(2,167,1,'2019-03-09'),	(3,267,1,'2019-03-09'),	(4,367,1,'2019-03-09'),	(5,467,1,'2019-03-09'),
(2,168,1,'2019-03-10'),	(3,268,1,'2019-03-10'),	(4,368,1,'2019-03-10'),	(5,468,1,'2019-03-10'),
(2,169,1,'2019-03-11'),	(3,269,1,'2019-03-11'),	(4,369,1,'2019-03-11'),	(5,469,1,'2019-03-11'),
(2,170,1,'2019-03-12'),	(3,270,1,'2019-03-12'),	(4,370,1,'2019-03-12'),	(5,470,1,'2019-03-12'),
(2,171,1,'2019-03-13'),	(3,271,1,'2019-03-13'),	(4,371,1,'2019-03-13'),	(5,471,1,'2019-03-13'),
(2,172,1,'2019-03-14'),	(3,272,1,'2019-03-14'),	(4,372,1,'2019-03-14'),	(5,472,1,'2019-03-14'),
(2,173,1,'2019-03-15'),	(3,273,1,'2019-03-15'),	(4,373,1,'2019-03-15'),	(5,473,1,'2019-03-15'),
(2,174,1,'2019-03-16'),	(3,274,1,'2019-03-16'),	(4,374,1,'2019-03-16'),	(5,474,1,'2019-03-16'),
(2,175,1,'2019-03-17'),	(3,275,1,'2019-03-17'),	(4,375,1,'2019-03-17'),	(5,475,1,'2019-03-17'),
(2,176,1,'2019-03-18'),	(3,276,1,'2019-03-18'),	(4,376,1,'2019-03-18'),	(5,476,1,'2019-03-18'),
(2,177,1,'2019-03-19'),	(3,277,1,'2019-03-19'),	(4,377,1,'2019-03-19'),	(5,477,1,'2019-03-19'),
(2,178,1,'2019-03-20'),	(3,278,1,'2019-03-20'),	(4,378,1,'2019-03-20'),	(5,478,1,'2019-03-20'),
(2,179,1,'2019-03-21'),	(3,279,1,'2019-03-21'),	(4,379,1,'2019-03-21'),	(5,479,1,'2019-03-21'),
(2,180,1,'2019-03-22'),	(3,280,1,'2019-03-22'),	(4,380,1,'2019-03-22'),	(5,480,1,'2019-03-22'),
(2,181,1,'2019-03-23'),	(3,281,1,'2019-03-23'),	(4,381,1,'2019-03-23'),	(5,481,1,'2019-03-23'),
(2,182,1,'2019-03-24'),	(3,282,1,'2019-03-24'),	(4,382,1,'2019-03-24'),	(5,482,1,'2019-03-24'),
(2,183,1,'2019-03-25'),	(3,283,1,'2019-03-25'),	(4,383,1,'2019-03-25'),	(5,483,1,'2019-03-25'),
(2,184,1,'2019-03-26'),	(3,284,1,'2019-03-26'),	(4,384,1,'2019-03-26'),	(5,484,1,'2019-03-26'),
(2,185,1,'2019-03-27'),	(3,285,1,'2019-03-27'),	(4,385,1,'2019-03-27'),	(5,485,1,'2019-03-27'),
(2,186,1,'2019-03-28'),	(3,286,1,'2019-03-28'),	(4,386,1,'2019-03-28'),	(5,486,1,'2019-03-28'),
(2,187,1,'2019-03-29'),	(3,287,1,'2019-03-29'),	(4,387,1,'2019-03-29'),	(5,487,1,'2019-03-29'),
(2,188,1,'2019-03-30'),	(3,288,1,'2019-03-30'),	(4,388,1,'2019-03-30'),	(5,488,1,'2019-03-30'),
(2,189,1,'2019-03-31'),	(3,289,1,'2019-03-31'),	(4,389,1,'2019-03-31'),	(5,489,1,'2019-03-31'),
(2,190,1,'2019-04-01'),	(3,290,1,'2019-04-01'),	(4,390,1,'2019-04-01'),	(5,490,1,'2019-04-01'),
(2,191,1,'2019-04-02'),	(3,291,1,'2019-04-02'),	(4,391,1,'2019-04-02'),	(5,491,1,'2019-04-02'),
(2,192,1,'2019-04-03'),	(3,292,1,'2019-04-03'),	(4,392,1,'2019-04-03'),	(5,492,1,'2019-04-03'),
(2,193,1,'2019-04-04'),	(3,293,1,'2019-04-04'),	(4,393,1,'2019-04-04'),	(5,493,1,'2019-04-04'),
(2,194,1,'2019-04-05'),	(3,294,1,'2019-04-05'),	(4,394,1,'2019-04-05'),	(5,494,1,'2019-04-05'),
(2,195,1,'2019-04-06'),	(3,295,1,'2019-04-06'),	(4,395,1,'2019-04-06'),	(5,495,1,'2019-04-06'),
(2,196,1,'2019-04-07'),	(3,296,1,'2019-04-07'),	(4,396,1,'2019-04-07'),	(5,496,1,'2019-04-07'),
(2,197,1,'2019-04-08'),	(3,297,1,'2019-04-08'),	(4,397,1,'2019-04-08'),	(5,497,1,'2019-04-08'),
(2,198,1,'2019-04-09'),	(3,298,1,'2019-04-09'),	(4,398,1,'2019-04-09'),	(5,498,1,'2019-04-09'),
(2,199,1,'2019-04-10'),	(3,299,1,'2019-04-10'),	(4,399,1,'2019-04-10'),	(5,499,1,'2019-04-10')




BACKUP DATABASE YieldDB TO DISK = 'C:\SuperYield\DBYield_20190227_145106x.bak'













insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-07-28',14968.9054273133)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-07-29',15105.979865937)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-07-30',15282.4897045221)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-07-31',15438.347372833)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-01',15504.2783366042)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-02',15625.4022327258)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-03',15828.7024684432)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-04',15867.8061940549)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-05',15918.3624695215)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-06',15983.9025702677)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-07',15988.5088795381)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-08',15989.8535772367)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-09',16078.1934842617)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-10',16196.5345834885)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-11',16218.2279857584)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-12',16359.2568186975)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-13',16536.6121645029)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-14',16748.0010993042)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-15',16895.6141849598)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-16',16926.8770319399)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-17',16995.2768456238)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-18',17049.8811537443)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-19',17254.4805604918)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-20',17486.1082072503)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-21',17513.3051327237)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-22',17524.7033036072)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-23',17566.9567776777)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-24',17688.1929008223)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-25',17758.0635406634)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-26',17795.0072953236)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-27',17890.8975204389)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-28',18024.99923131)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-29',18038.2873568125)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-30',18190.3199592731)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-08-31',18275.987486049)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-01',18305.7043182151)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-02',18428.4230814136)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-03',18492.3598309676)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-04',18501.2922125951)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-05',18535.9869004838)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-06',18691.6866680729)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-07',18834.7071921451)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-08',18835.885641214)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-09',18867.5386451155)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-10',19004.8649572594)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-11',19201.5354474208)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-12',19220.4388977878)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-13',19400.0409795479)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-14',19500.1340792956)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-15',19541.8667782444)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-16',19553.1418155408)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-17',19722.3884487292)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-18',19858.9541393814)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-19',20068.0071088442)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-20',20231.7923848569)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-21',20347.1936224661)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-22',20491.2005754431)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-23',20605.133049243)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-24',20769.5430555949)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-25',20932.3855941934)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-26',21158.5551491367)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-27',21217.2053819387)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-28',21362.8101903121)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-29',21461.4727854475)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-09-30',21516.2095184911)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-01',21711.0190176168)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-02',21812.0312788388)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-03',22076.989617393)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-04',22291.7685922336)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-05',22396.7004191813)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-06',22690.4213852064)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-07',22789.0110471617)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-08',22854.3836466936)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-09',22972.6365546272)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-10',23127.3876179988)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-11',23439.5966691259)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-12',23678.1655471138)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-13',23704.3842757956)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-14',24013.5305258382)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-15',24138.1185023538)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-16',24345.1124894011)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-17',24375.6618113301)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-18',24407.6398813797)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-19',24450.5323484778)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-20',24599.3050404234)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-21',24768.9377906269)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-22',24833.4338343585)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-23',24847.983984685)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-24',24956.8190578152)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-25',25030.5619540154)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-26',25169.2210910647)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-27',25365.785995315)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-28',25599.4647214196)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-29',25654.2428629242)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-30',25888.1985095723)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-10-31',25897.3897533337)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-01',26126.650628496)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-02',26348.2791680816)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-03',26621.0481538382)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-04',26972.1186734394)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-05',27110.7274062166)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-06',27387.3826523832)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-07',27735.6637965832)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-08',27846.2037871636)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-09',27951.8622065818)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-10',28115.8595845154)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-11',28486.9178442323)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-12',28661.584432578)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-13',28883.0611421604)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-14',28997.6618828906)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-15',29055.1728756455)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-16',29112.9152892525)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-17',29459.291804038)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-18',29572.5397616533)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-19',29946.3131967927)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-20',30085.8660756088)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-21',30290.6746375626)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-22',30552.4205852428)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-23',30804.5167440196)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-24',30954.5789869849)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-25',31118.2416673212)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-26',31454.6877720217)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-27',31813.1533041303)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-28',32196.6029309223)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-29',32405.6969324)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-11-30',32438.4723569064)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-01',32587.9050583334)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-02',32888.344076344)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-03',33024.4901683316)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-04',33150.7342759416)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-05',33561.8206752426)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-06',33764.1985043632)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-07',34146.4344318931)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-08',34167.187212461)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-09',34533.3477040394)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-10',34923.7426149925)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-11',35120.9230975728)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-12',35565.5334065225)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-13',35655.5340775385)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-14',35865.0739189663)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-15',36013.789213827)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-16',36208.4944553018)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-17',36679.0932492869)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-18',36788.6402034986)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-19',37010.0977854751)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-20',37503.533423893)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-21',37949.1321175284)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-22',38335.3783540294)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-23',38608.8280812967)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-24',38975.860394838)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-25',39340.2670490196)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-26',39414.3828870476)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-27',39514.2420514048)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-28',39905.959322031)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-29',39977.4784692333)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-30',40129.0625235859)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2018-12-31',40592.9607300504)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-01',40912.3582312971)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-02',41207.9066302658)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-03',41520.785088802)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-04',41997.6078110769)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-05',42548.870870558)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-06',42803.4461752568)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-07',43049.859902401)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-08',43119.8583759598)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-09',43331.3071563665)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-10',43368.776200164)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-11',43679.8952750395)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-12',44038.3343239556)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-13',44082.8991226683)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-14',44646.4325641844)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-15',44804.9383497358)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-16',45108.2315855961)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-17',45426.5129378051)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-18',45621.7625275931)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-19',45786.4606591204)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-20',45885.6823160904)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-21',46092.4511224973)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-22',46322.8766755104)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-23',46816.6226264244)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-24',46824.2609102646)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-25',47249.1596596302)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-26',47632.6123459686)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-27',47733.2974481538)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-28',47831.1128671381)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-29',48323.5047928264)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-30',48634.13808189)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-01-31',49273.6516414548)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-01',49699.1043342893)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-02',50200.9969134022)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-03',50623.0138573228)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-04',50979.5075317657)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-05',51389.1945416913)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-06',51846.8148167846)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-07',52003.4523519924)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-08',52057.8338501077)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-09',52739.0136317341)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-10',52813.4148499264)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-11',52895.8394921916)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-12',53257.3489441239)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-13',53366.0634963928)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-14',54003.3464080133)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-15',54716.5185267304)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-16',55146.6546782395)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-17',55876.3746618458)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-18',56612.4877069811)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-19',57009.1580285591)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-20',57526.580544208)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-21',57980.6975576523)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-22',58297.8560734931)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-23',59034.0697103787)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-24',59076.8116409874)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-25',59481.2297196698)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-26',60112.0664329431)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-27',60687.0940538381)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-02-28',61349.0502697989)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-01',61584.7677866928)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-02',62388.0318652809)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-03',62517.5991798427)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-04',62591.9681911387)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-05',62655.6978799316)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-06',62684.0770232107)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-07',63414.4181150981)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-08',63523.3226080448)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-09',64031.8483576313)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-10',64049.1743896242)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-11',64620.2775901657)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-12',65043.8669797414)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-13',65837.4790512008)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-14',66723.8624474495)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-15',67154.5864685964)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-16',67829.8167379015)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-17',68400.7635809156)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-18',68614.4511761997)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-19',69179.6426355107)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-20',69700.7783879901)
insert AccountValueHistory (AccountId, ValueDate, Value) values (1,'2019-03-21',70151.25)









insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-01',5774.98459771834)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-02',5850.52694122136)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-03',6087.68638554862)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-04',6223.35033693272)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-05',6326.61704612826)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-06',6417.45388129331)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-07',6646.39453976245)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-08',6871.70215705713)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-09',6950.84625376189)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-10',6998.47050019978)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-11',7109.61977562402)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-12',7136.5129131396)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-13',7152.73249649262)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-14',7155.29832293861)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-15',7393.14005439283)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-16',7460.2007243214)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-17',7529.07163324446)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-18',7695.19241752783)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-19',7830.31989786437)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-20',7874.81371154772)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-21',8149.60190979882)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-22',8414.09823484264)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-23',8532.90432403484)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-24',8610.40354213734)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-25',8679.19533420608)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-26',8982.21966779766)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-27',9253.11729119434)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-28',9387.71690287022)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-29',9503.55488693027)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-30',9727.0626043794)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-01-31',9805.54117057775)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-01',9871.04411108404)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-02',9926.24366260196)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-03',10314.8168248713)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-04',10438.1793273573)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-05',10455.0231679909)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-06',10727.4220875641)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-07',11167.983404562)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-08',11331.546444417)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-09',11348.7152826826)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-10',11455.6834106639)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-11',11549.935779768)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-12',11992.3969856243)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-13',12180.5785598297)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-14',12674.418762994)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-15',13058.1769716453)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-16',13237.8618483922)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-17',13628.3241413708)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-18',14130.7828486873)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-19',14318.6212418038)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-20',14877.6674252054)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-21',14878.3823265198)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-22',15428.5054801818)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-23',15672.013864033)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-24',15865.9975108309)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-25',16126.2628455824)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-26',16288.8236961239)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-27',16556.8666800314)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-02-28',16772.5106859596)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-01',17205.9247761923)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-02',17458.3771087446)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-03',17705.1478654838)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-04',18250.1123269831)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-05',18583.5440777367)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-06',18790.3946922878)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-07',19520.1910127421)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-08',19635.6655798935)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-09',19842.9842966951)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-10',20594.8397114452)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-11',21430.1404981224)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-12',21480.643545129)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-13',21653.1360222701)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-14',21889.5638670062)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-15',22428.3728133731)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-16',22855.9752310697)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-17',23197.4727394223)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-18',24077.2651304148)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-19',24259.5193122102)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-20',24310.1394083908)
insert AccountValueHistory (AccountId, ValueDate, Value) values (3,'2019-03-21',25319.06)



select * from AccountValueHistory


USE [master]
GO

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [SYConsole]    Script Date: 4/16/2019 7:09:36 AM ******/
CREATE LOGIN [SYConsole] WITH PASSWORD=N'SY0.!@#$%.Console', DEFAULT_DATABASE=[YieldDB], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [SYWeb]    Script Date: 4/16/2019 7:10:52 AM ******/
CREATE LOGIN [SYWeb] WITH PASSWORD=N'SY0.!@#$%.Web', DEFAULT_DATABASE=[YieldDB], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO


EAAAABxBUH+YQPtP+vl6hIeDt56Al3NiopEF4VZer9QLTif/oDjaphYHIsf300T4w9IdiiakNkc2VxJEz3MvS7cgzsyo0yxS/Duw2wh26ON7cWMDj7p+roAdN5znkcrbBTTrLUyl1RL59Cz4s6mtgC5RAws=
EAAAAOxOMYxi6Uj8H44dAINYwDps9a0b/UVxkw8Uc9ul1zocfu7iK1dUNZ4qZxwtXq9x+0rLJgaw5lGz7xL7lgwo+MHG/nihZRMov2ha7JACWm/Mmwf02Yx8WWwZMZXiaqwqKW7f5UyZWs9Zptj51M6YALc=

