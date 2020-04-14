use boeki;

create table provincie(

ProvincieID int NOT NULL  PRIMARY KEY,
ProvincieNaam varchar(255) NOT NULL,

);

create table gemeente(

GemeenteID int NOT NULL  PRIMARY KEY,
GemeenteNaam varchar(255) NOT NULL,
ProvincieID int NOT NULL FOREIGN KEY REFERENCES provincie(ProvincieID)

);

create table straat(

StraatID int NOT NULL  PRIMARY KEY,
StraatNaam varchar(255) NOT NULL,
GemeenteID int NOT NULL FOREIGN KEY REFERENCES gemeente(GemeenteID)

);

create table graaf(

GraafID int NOT NULL  PRIMARY KEY,
StraatID int NOT NULL FOREIGN KEY REFERENCES straat(StraatID)

);

create table segment(
SegmentID int NOT NULL  PRIMARY KEY,
GraafID int NOT NULL FOREIGN KEY REFERENCES graaf(GraafID)

);

create table knoop(
KnoopID int NOT NULL  PRIMARY KEY,
SegmentID int NOT NULL FOREIGN KEY REFERENCES segment(SegmentID)
);

create table punt(

X decimal NOT NULL,
Y decimal NOT NULL,
SegmentID int NOT NULL FOREIGN KEY REFERENCES segment(SegmentID),
KnoopID int FOREIGN KEY REFERENCES knoop(KnoopID),
PuntID int NOT NULL IDENTITY(1,1) PRIMARY KEY
);