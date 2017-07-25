Create DataBase dbEncurtadorUrl;

Go

Use dbEncurtadorUrl;

Create Table tblUsuario
(
	UserId Varchar(100)
)

Create Table tblUrl
(
	Id int IDENTITY(1,1) PRIMARY KEY,
	ShortUrl Varchar(500),
	Url Varchar(500),
	UserId Varchar(100)
)

Create Table tblUrlLog
(
	UrlLogId int IDENTITY(1,1) PRIMARY KEY,
	Id int,
	FOREIGN KEY (Id) REFERENCES tblUrl(Id)
)

/* EXECUTAR OS GRANTS CASO SEJA NECESSÁRIO */

GRANT SELECT, INSERT, DELETE, UPDATE ON OBJECT::tblUsuario TO *** USUARIO CRIADO NO BANCO SQL *** ;  
GO 
GRANT SELECT,INSERT, DELETE, UPDATE ON OBJECT::tblUrl TO *** USUARIO CRIADO NO BANCO SQL *** ;  
GO 
GRANT SELECT,INSERT, DELETE, UPDATE ON OBJECT::tblUrlLog TO *** USUARIO CRIADO NO BANCO SQL *** ;  
GO 