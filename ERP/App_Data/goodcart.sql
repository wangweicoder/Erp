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
---2019-11-01
--dianz
 CREATE TABLE  FlowerAppraise
 (Id int NOT NULL　 IDENTITY(1,1) ,
 [UsersId] int NULL , 
 [ArrangementId]　int NULL,
 [IsGood]　varchar(10) null, 
 [Pictures]　varchar(300),
 [Content] nvarchar(500) NULL , 
 CreateTime datetime NULL ,
 UpdateTime datetime NULL );
 ---2019-1105为记录开始养护时间，去掉默认约束
ALTER TABLE [dbo].[FlowerTreatment] DROP CONSTRAINT [DF_FlowerTreatment_time]