CREATE TABLE FlowerShopCart
 (Id int NOT NULL　 IDENTITY(1,1) ,
 [UsersId] int NULL ,
 [FlowerId] [varchar](300) NULL,
 Num int NULL ,
 [Status] int NULL ,
 CreateTime datetime NULL ,
 UpdateTime datetime NULL );