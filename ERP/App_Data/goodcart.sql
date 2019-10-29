CREATE TABLE FlowerShopCart
 (Id int NOT NULL　 IDENTITY(1,1) ,
 [UsersId] int NULL ,
 [FlowerId] [varchar](300) NULL,
 Num int NULL ,
 [Status] int NULL ,
 CreateTime datetime NULL ,
 UpdateTime datetime NULL );
 
 --D:\Program Files\Microsoft SQL Server\MSSQL13.SQLEXPRESS\MSSQL\DATA
 ----
 CREATE TABLE FlowerActive
 (Id int NOT NULL　 IDENTITY(1,1) ,
 [UsersId] int NULL ,
 [FlowerId]　varchar(300),
 [Content] nvarchar(500) NULL , 
 CreateTime datetime NULL ,
 UpdateTime datetime NULL );

CREATE TABLE UsersLoginLog
 (Id int NOT NULL　 IDENTITY(1,1) ,
 [UsersId] int NULL ,
 [Year]　varchar(30),
 [Month]　varchar(30),
 [Day]　varchar(30),
 [PhoneNum]　varchar(300),
 [Content] nvarchar(500) NULL , 
 LoginTime datetime NULL );

CREATE TABLE  Adviertisement
 (Id int NOT NULL　 IDENTITY(1,1) ,
 [UsersId] int NULL , 
 [Title]　varchar(50),
 [Picture]　varchar(300),
 [LinkUrl]　varchar(300),
 [AdvPageLocation]　varchar(300),
 [Content] nvarchar(500) NULL , 
 CreateTime datetime NULL ,
 UpdateTime datetime NULL );

 --2019-10-29
alter table [FlowerTreatment] add endtime Datetime 