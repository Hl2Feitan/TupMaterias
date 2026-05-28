

--Crear la base de datos
CREATE DATABASE TupMaterias;
GO

USE TupMaterias;
GO

--Tabla de Profesores
CREATE TABLE Profesores (
    Id       INT PRIMARY KEY IDENTITY(1,1),
    Nombre   NVARCHAR(100) NOT NULL,
    Email    NVARCHAR(150)
);
GO

--Tabla de Materias
CREATE TABLE Materias (
    Id          INT PRIMARY KEY IDENTITY(1,1),
    Nombre      NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(500),
    ProfesorId  INT NOT NULL,
    FOREIGN KEY (ProfesorId) REFERENCES Profesores(Id)
);
GO

--Tabla de Alumnos
CREATE TABLE Alumnos (
    Id      INT PRIMARY KEY IDENTITY(1,1),
    Nombre  NVARCHAR(100) NOT NULL,
    Legajo  NVARCHAR(20)  NOT NULL
);
GO

--Tabla de Inscripciones [relacion N:M entre Alumnos y Materias]
CREATE TABLE Inscripciones (
    AlumnoId   INT NOT NULL,
    MateriaId  INT NOT NULL,
    PRIMARY KEY (AlumnoId, MateriaId),
    FOREIGN KEY (AlumnoId)  REFERENCES Alumnos(Id),
    FOREIGN KEY (MateriaId) REFERENCES Materias(Id)
);
GO

--DATOS DE PRUEBA

INSERT INTO Profesores (Nombre, Email) VALUES
    ('Carlos Gutierrez',  'cgutierrez@frre.utn.edu.ar'),
    ('Maria Lopez',       'mlopez@frre.utn.edu.ar'),
    ('Juan Perez',        'jperez@frre.utn.edu.ar'),
    ('Ana Martinez',      'amartinez@frre.utn.edu.ar');
GO

INSERT INTO Materias (Nombre, Descripcion, ProfesorId) VALUES
    ('Programacion I',      'Introduccion a la programacion con C#',             1),
    ('Programacion II',     'Programacion orientada a objetos',                  1),
    ('Programacion III',    'Desarrollo web con ASP.NET MVC',                    2),
    ('Base de Datos',       'Diseño y consultas en SQL Server',                  3),
    ('Matematica',          'Algebra y analisis matematico',                     4),
    ('Laboratorio',         'Practicas de hardware y redes',                     3);
GO

INSERT INTO Alumnos (Nombre, Legajo) VALUES
    ('Lucas Fernandez',    'TUP-001'),
    ('Sofia Rodriguez',    'TUP-002'),
    ('Diego Gomez',        'TUP-003'),
    ('Valentina Torres',   'TUP-004'),
    ('Matias Diaz',        'TUP-005'),
    ('Camila Sanchez',     'TUP-006'),
    ('Nicolas Alvarez',    'TUP-007'),
    ('Florencia Ruiz',     'TUP-008');
GO

--Inscripciones de varios alumnos en varias materias
INSERT INTO Inscripciones (AlumnoId, MateriaId) VALUES
    (1, 1), (1, 2), (1, 4),
    (2, 1), (2, 3), (2, 4),
    (3, 2), (3, 3), (3, 5),
    (4, 1), (4, 4), (4, 6),
    (5, 2), (5, 3),
    (6, 1), (6, 2), (6, 3),
    (7, 4), (7, 5), (7, 6),
    (8, 1), (8, 3), (8, 6);
GO

--Verificacion
SELECT m.Nombre AS Materia, p.Nombre AS Profesor,
       COUNT(i.AlumnoId) AS CantidadAlumnos
FROM Materias m
JOIN Profesores p ON m.ProfesorId = p.Id
LEFT JOIN Inscripciones i ON m.Id = i.MateriaId
GROUP BY m.Nombre, p.Nombre;
GO