DROP TABLE IF EXISTS CaseGoals;
DROP TABLE IF EXISTS Goals;
DROP TABLE IF EXISTS Cases;
DROP TABLE IF EXISTS Allergies;
DROP TABLE IF EXISTS PatientMedications;
DROP TABLE IF EXISTS Diagnoses;
DROP TABLE IF EXISTS MedicalHistory;
DROP TABLE IF EXISTS Patients;
DROP TABLE IF EXISTS IllnessTreatments;
DROP TABLE IF EXISTS Illnesses;
DROP TABLE IF EXISTS Medications;
DROP TABLE IF EXISTS users;

CREATE TABLE users
(
    UserId Integer primary key,
    Role   INTEGER NOT NULL,
    Name   TEXT
)

CREATE TABLE Medications
(
    MedicationId        Integer Primary key,
    MedicationName      VARCHAR(255),
    Dosage              Integer,
    AdministrationRoute TINYINT
)

CREATE TABLE Illnesses
(
    IllnessId   Integer Primary Key,
    IllnessName VARCHAR(255),
    Antidote    Integer references Medications (MedicationId)
)

CREATE TABLE Patients
(
    PatientId        Integer primary key,
    Status           Integer,
    PatientName      TEXT,
    Weight           Integer,
    Age              Integer,
    Sex              TINYINT,
    Heartrate        Integer,
    Bloodpressure    Integer,
    RespiratoryRate  Integer,
    OxygenSaturation Decimal(5, 2),
    Temperature      Decimal(4, 1),
)

CREATE TABLE MedicalHistory
(
    PatientId Integer not null references Patients (PatientId),
    IllnessId Integer not null references Illnesses (IllnessId),
    PRIMARY KEY (PatientId, IllnessId)
)

CREATE TABLE Diagnoses
(
    PatientId Integer not null references Patients (PatientId),
    IllnessId Integer not null references Illnesses (IllnessId),
    PRIMARY KEY (PatientId, IllnessId)
)

CREATE TABLE PatientMedications
(
    PatientId    Integer not null references Patients (PatientId),
    MedicationId Integer not null references Medications (MedicationId),
    PRIMARY KEY (PatientId, MedicationId)
)

CREATE TABLE Allergies
(
    PatientId    Integer not null references Patients (PatientId),
    MedicationId Integer not null references Medications (MedicationId),
    PRIMARY KEY (PatientId, MedicationId)
)

CREATE TABLE Cases
(
    CaseId    Integer primary key,
    PatientId Integer not null references Patients (PatientId),
    UserId    Integer not null references Users (UserId)
)

CREATE TABLE Goals
(
    GoalId      Integer primary key,
    Name        VARCHAR(255),
    TimeLimit   Integer,
    Description TEXT,
)

CREATE TABLE CaseGoals
(
    CaseId Integer references Cases (CaseId),
    GoalId Integer references Goals (GoalId),
    PRIMARY KEY (CaseId, GoalId)
)