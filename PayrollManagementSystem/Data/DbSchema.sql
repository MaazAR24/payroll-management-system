IF NOT EXISTS(SELECT 1 FROM master.sys.databases where name = 'PayrollManagementSystem')
CREATE DATABASE PayrollManagementSystem;

GO

use PayrollManagementSystem;

GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
CREATE TABLE Users(
	Id int IDENTITY(1,1) PRIMARY KEY,
	UserName varchar(500),
	[Password] varchar(500),
	[Role] varchar(500)
)

GO

IF NOT EXISTS(SELECT 1 FROM Users)
INSERT INTO Users(UserName, [Password], [Role])
VALUES
('user1', 'abc123', 'Accountant'),
('user2', 'abc123', 'Manager'),
('user3', 'abc123', 'CEO')

GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Employees')
CREATE TABLE Employees(
	Id int IDENTITY(1,1) PRIMARY KEY,
	[Name] varchar(500),
	Position varchar(500),
	Department varchar(500)
)

GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Salaries')
CREATE TABLE Salaries(
	Id int IDENTITY(1,1) PRIMARY KEY,
	EmployeeId int not null,
	[Month] int,
	[Year] int,
	Amount int,
	Transfered bit
)

GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Requests')
CREATE TABLE Requests(
	Id int IDENTITY(1,1) PRIMARY KEY,
	EmployeeId int not null,
	IsApprovedByAccountant bit,
	IsApprovedByManager bit,
	[Status] int
)

GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RequestedSalaries')
CREATE TABLE RequestedSalaries(
	Id int IDENTITY(1,1) PRIMARY KEY,
	RequestId int not null,
	SalaryId int not null
)

GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Files')
CREATE TABLE Files(
	Id int IDENTITY(1,1) PRIMARY KEY,
	ModuleId int not null,
	RecordId int not null,
	[FileName] nvarchar(500),
	[StoredFileName] nvarchar(500)
)
