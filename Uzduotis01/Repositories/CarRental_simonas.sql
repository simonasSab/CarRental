    --Naudokite Microsoft SQL Server ir Dapper ORM biblioteką duomenų saugojimui ir manipuliavimui.
    --Duomenų bazėje turi būti šios lentelės:

    --Automobiliai: bendra lentelė visiems automobiliams.
    --NaftosKuroAutomobiliai: specifinė informacija naftos kuro automobiliams.
    --Elektromobiliai: specifinė informacija elektromobiliams.
    --Klientai: specifinė informacija apie klientus.
    --Nuoma: lentelėje turi būti informacija apie automobilių nuomą, tai yra turi būti nuorodos į automobilį, į klientą, bei datos NUO/IKI

----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
CREATE TABLE Vehicles (
	ID int IDENTITY(1,1) PRIMARY KEY,
	Make nvarchar(100),
	Model nvarchar(100),
	ProductionYear int,
	VIN nvarchar(17) DEFAULT dbo.GenerateVIN(NEWID())
);

INSERT INTO Vehicles (Make, Model, ProductionYear)
VALUES ('Toyota', 'Corolla', 2015);

INSERT INTO Vehicles (Make, Model, ProductionYear)
VALUES ('Nissan', 'Leaf', 2015);

----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------

SELECT * FROM Vehicles;

----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
CREATE TABLE FossilFuelVehicles (
	ID int UNIQUE FOREIGN KEY REFERENCES Vehicles(ID),
	TankCapacity long(4,2),
);

INSERT INTO FossilFuelVehicles (ID, TankCapacity)
VALUES (1, 60.05),
(2, 60.01),
(3, 60.01),
(4, 60.01),
(5, 60.01),
(6, 60.01),
(7, 60.01),
(8, 60.01),
(9, 60.01),
(10, 60.01);

SELECT * FROM FossilFuelVehicles;
DROP TABLE FossilFuelVehicles;
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
CREATE TABLE ElectricVehicles (
	ID int UNIQUE FOREIGN KEY REFERENCES Vehicles(ID),
	BatteryCapacity long(4,2),
);

INSERT INTO ElectricVehicles (ID, BatteryCapacity)
VALUES(11, 27.7),
(12, 27.7),
(13, 27.7),
(14, 27.7),
(15, 27.7),
(16, 27.7),
(17, 27.7),
(18, 27.7),
(19, 27.7),
(20, 27.7);

SELECT * FROM ElectricVehicles;
DROP TABLE ElectricVehicles;
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
 -- Papildinys: Klientų lentelėje turi būti stulpelis, reprezentuojantis kliento registracijos sistemoje datą ir laiką.
 -- (Padaryti su DEFAULT SQL arba registruojant klientą naudoti DateTime.Now)
CREATE TABLE Clients (
	ID int IDENTITY(1,1) PRIMARY KEY,
	FullName nvarchar(100),
	PersonalID long(11,0) DEFAULT dbo.GeneratePersonalID(RAND()),
	RegistrationDateTime datetime DEFAULT SYSDATETIME()
);
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------

INSERT INTO Clients (FullName)
VALUES ('Bobby Fischer');

INSERT INTO Clients (FullName)
VALUES ('John Frusciante');

INSERT INTO Clients (FullName)
VALUES ('Aholf Ditler');

INSERT INTO Clients (FullName)
VALUES ('Donald Duck');

INSERT INTO Clients (FullName)
VALUES ('Dalia Grybauskaitė');

INSERT INTO Clients (FullName)
VALUES ('Sadhguru');

SELECT * FROM CLients;
DROP TABLE Clients;

----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
CREATE TABLE Rents (
	ID int IDENTITY(1,1) PRIMARY KEY,
	VehicleID int FOREIGN KEY REFERENCES Vehicles(ID),
	ClientID int FOREIGN KEY REFERENCES Clients(ID),
	DateFrom date DEFAULT CONVERT(date, SYSDATETIME()),
	DateTo date DEFAULT null
);

INSERT INTO Rents (VehicleID, ClientID)
VALUES (1, 1), (2, 2), (3, 1), (4, 3);

INSERT INTO Rents (VehicleID, ClientID, DateFrom, DateTo)
VALUES (5, 4, CONVERT(date, SYSDATETIME()), '2024-06-10');

INSERT INTO Rents (VehicleID, ClientID, DateFrom, DateTo)
VALUES (5, 4, '2024-05-10', CONVERT(date, SYSDATETIME()));

----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
SELECT * FROM Rents;
SELECT * FROM Rents WHERE (VehicleID = 1);
DELETE FROM Rents WHERE (DateTo <= CONVERT(date, SYSDATETIME()));
DROP TABLE Rents;




--- Juodrastis
----------------------------------------------------------------------------------------

SELECT v.ID, v.Make, v.Model, v.ProductionYear, v.VIN, e.BatteryCapacity FROM Vehicles v INNER JOIN ElectricVehicles e
ON v.ID = e.ID;

SELECT v.ID, v.Make, v.Model, v.ProductionYear, v.VIN, f.TankCapacity FROM Vehicles v INNER JOIN FossilFuelVehicles f
ON v.ID = f.ID;

SELECT v.ID, v.Make, v.Model, v.ProductionYear, v.VIN FROM Vehicles v
INNER JOIN ElectricVehicles e ON v.ID = e.ID
INNER JOIN FossilFuelVehicles f ON v.ID = f.ID;

SELECT * FROM Vehicles v
INNER JOIN ElectricVehicles e ON v.ID = e.ID
INNER JOIN FossilFuelVehicles f ON v.ID = f.ID;

SELECT TOP(1) ID FROM Vehicles ORDER BY ID DESC;