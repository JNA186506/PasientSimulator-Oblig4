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
    UserId Integer IDENTITY(1,1) PRIMARY KEY,
    Role   INTEGER NOT NULL,
    Name   TEXT
)

CREATE TABLE Medications
(
    MedicationId        Integer IDENTITY(1,1) PRIMARY KEY,
    MedicationName      VARCHAR(255),
    Dosage              Integer,
    AdministrationRoute INT
)

CREATE TABLE Illnesses
(
    IllnessId   Integer IDENTITY(1,1) PRIMARY KEY,
    IllnessName VARCHAR(255),
    AntidoteId  Integer REFERENCES Medications(MedicationId)
)

CREATE TABLE Patients
(
    PatientId               Integer IDENTITY(1,1) PRIMARY KEY,
    Status                  Integer,
    PatientName             TEXT,
    Weight                  Integer,
    Age                     Integer,
    Sex                     INT,
    Heartrate               Integer,
    BloodPressure_Systolic  Integer,
    BloodPressure_Diastolic Integer,
    RespiratoryRate         Integer,
    OxygenSaturation        FLOAT,
    Temperature             FLOAT
)

CREATE TABLE MedicalHistory
(
    PatientId Integer NOT NULL REFERENCES Patients(PatientId),
    IllnessId Integer NOT NULL REFERENCES Illnesses(IllnessId),
    PRIMARY KEY (PatientId, IllnessId)
)

CREATE TABLE Diagnoses
(
    PatientId Integer NOT NULL REFERENCES Patients(PatientId),
    IllnessId Integer NOT NULL REFERENCES Illnesses(IllnessId),
    PRIMARY KEY (PatientId, IllnessId)
)

CREATE TABLE PatientMedications
(
    PatientId    Integer NOT NULL REFERENCES Patients(PatientId),
    MedicationId Integer NOT NULL REFERENCES Medications(MedicationId),
    PRIMARY KEY (PatientId, MedicationId)
)

CREATE TABLE Allergies
(
    PatientId    Integer NOT NULL REFERENCES Patients(PatientId),
    MedicationId Integer NOT NULL REFERENCES Medications(MedicationId),
    PRIMARY KEY (PatientId, MedicationId)
)

CREATE TABLE Cases
(
    CaseId    Integer IDENTITY(1,1) PRIMARY KEY,
    PatientId Integer NOT NULL REFERENCES Patients(PatientId),
    UserId    Integer NOT NULL REFERENCES Users(UserId)
)

CREATE TABLE Goals
(
    GoalId      Integer IDENTITY(1,1) PRIMARY KEY,
    GoalName        VARCHAR(255),
    TimeLimit   Integer,
    Description TEXT,
    CaseId Integer REFERENCES Cases(CaseId)
)

CREATE TABLE CaseGoals
(
    CaseId Integer REFERENCES Cases(CaseId),
    GoalId Integer REFERENCES Goals(GoalId),
    PRIMARY KEY (CaseId, GoalId)
)