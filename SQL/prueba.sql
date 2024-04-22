-- Active: 1713312421907@@b99bayllqraqwwcc0gja-mysql.services.clever-cloud.com@3306@b99bayllqraqwwcc0gja
CREATE TABLE Empleados(
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Names VARCHAR(45),
    LastNames VARCHAR(45),
    Email VARCHAR(45),
    Password VARCHAR(45)
);

SELECT * FROM `Empleados`;


CREATE TABLE Historiales(
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Entrada DATETIME,
    Salida DATETIME NULL,
    Empleado_Id INT
);

ALTER TABLE `Historiales` ADD FOREIGN KEY (Empleado_Id) REFERENCES Empleados(Id);

DROP TABLE `Empleados`;


INSERT INTO Empleados (Names, LastNames, Email, Password) 
VALUES 
('Juan', 'Perez', 'juan@example.com', 'password1'),
('Maria', 'Gonzalez', 'maria@example.com', 'password2'),
('Carlos', 'Lopez', 'carlos@example.com', 'password3'),
('Ana', 'Martinez', 'ana@example.com', 'password4'),
('Pedro', 'Rodriguez', 'pedro@example.com', 'password5');
