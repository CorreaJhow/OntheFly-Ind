create database OnTheFly;
use OnTheFly;

create table CompanhiaAerea(
	CNPJ varchar(50) not null,
	RazaoSocial varchar(50) not null,
	DataAbertura date not null,
	UltimoVoo date not null,
	DataCadastro date not null,
	Situacao char(1)

	constraint PK_CNPJ_CIAAEREA primary key (CNPJ)
);

create table Passageiro(
	Nome varchar(50) not null,
	Cpf varchar(25) not null,
	DataNascimento date not null,
	Sexo char(1) not null,
	UltimaCompra date,
	DataCadastro date not null,
	Situacao char(1) not null,

	constraint PK_CNPJ_PASSAGEIRO primary key (Cpf)
);

create table Aeronave(
	InscricaoANAC varchar(6)not null,
	Capacidade int not null,
	UltimaVenda date not null,
	DataCadastro date not null, 
	Situacao char(1) not null,
	CNPJ varchar(50) not null,

	constraint PK_ID_AERONAVE primary key (InscricaoANAC),
	foreign key(CNPJ) references CompanhiaAerea(CNPJ),
);

create table Voo( 
	Id varchar(10) ,
	InscricaoAeronave varchar(6),
	AssentosOcupados int,
	DataCadastro Date,
	Situacao char(1),
	DataVoo datetime,
	Destino varchar(10),

	constraint PK_ID_VOO primary key (Id),
	foreign key(InscricaoAeronave) references Aeronave(InscricaoANAC),
);

Create table Passagem( 
	Id varchar(50) not null,
	IdVoo varchar(10) not null,
	DataUltimaOperacao date not null,
	Valor float, 
	Situacao char not null,

	constraint PK_ID_PASSAGEM primary key (Id),
	foreign key (IdVoo) references Voo(Id),
);

create table PassagemVenda( 
	Id int not null,  
	DataVenda date not null, 
	Passageiro varchar(25) not null, 
	ValorTotal float not null,  
	Voo varchar(10) not null, 
	IDItemVenda varchar, 
	ValorUnitario float,

	constraint PK_ID_PASSAGEMVENDA primary key (Id),
	foreign key (Voo) references Voo(Id),
	foreign key (Passageiro) references Passageiro(Cpf) 
);

create table VendaPassageiro( 
	Id int identity not null,
	DataVenda date, 
	CPF varchar(25), 
	ValorTotal float,

	constraint PK_ID_VENDAPASSAGEIRO primary key (Id),
	foreign key (CPF) references Passageiro(CPF),
);

CREATE TABLE Cnpj_Restrito(
	Cnpj varchar(14) PRIMARY KEY NOT NULL,
);
CREATE TABLE Cpf_Restrito(
	Cpf varchar(11) PRIMARY KEY NOT NULL,
);




