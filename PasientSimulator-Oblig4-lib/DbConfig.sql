-- Database initialization for PasientSimulator-Oblig4
PRAGMA foreign_keys = ON;

-- Sexes (mapped from C# enum Sex)
CREATE TABLE IF NOT EXISTS sexes (
  SexId INTEGER PRIMARY KEY,
  SexLabel TEXT NOT NULL UNIQUE
);

-- Users
CREATE TABLE IF NOT EXISTS users (
  Id INTEGER PRIMARY KEY,
  Role INTEGER NOT NULL,
  Name TEXT
);

-- Patients (Sex referenced by SexId for referential integrity)
CREATE TABLE IF NOT EXISTS patients (
  PatientId INTEGER PRIMARY KEY,
  Status INTEGER,
  PatientName TEXT,
  Weight INTEGER,
  Age INTEGER,
  SexId INTEGER,
  Heartrate INTEGER,
  Bloodpressure INTEGER,
  RespiratoryRate INTEGER,
  OxygenSaturation REAL,
  Temperature REAL,
  FOREIGN KEY (SexId) REFERENCES sexes(SexId) ON DELETE SET NULL
);

-- Medications
CREATE TABLE IF NOT EXISTS medications (
  MedicationId INTEGER PRIMARY KEY,
  MedicationName TEXT,
  Dosage INTEGER,
  AdministrationRoots TEXT
);

-- Medication effects (one row per effect)
CREATE TABLE IF NOT EXISTS medication_effects (
  MedicationId INTEGER NOT NULL,
  Effect TEXT NOT NULL,
  PRIMARY KEY (MedicationId, Effect),
  FOREIGN KEY (MedicationId) REFERENCES medications(MedicationId) ON DELETE CASCADE
);

-- Illnesses (Antidote -> Medication)
CREATE TABLE IF NOT EXISTS illnesses (
  Id INTEGER PRIMARY KEY,
  Name TEXT,
  AntidoteId INTEGER,
  FOREIGN KEY (AntidoteId) REFERENCES medications(MedicationId) ON DELETE SET NULL
);

-- Cases
CREATE TABLE IF NOT EXISTS cases (
  CaseId INTEGER PRIMARY KEY,
  CasePatientId INTEGER,
  StudentId INTEGER,
  FOREIGN KEY (CasePatientId) REFERENCES patients(PatientId) ON DELETE SET NULL,
  FOREIGN KEY (StudentId) REFERENCES users(Id) ON DELETE SET NULL
);

-- Goals (each goal belongs to a case)
CREATE TABLE IF NOT EXISTS goals (
  GoalId INTEGER PRIMARY KEY AUTOINCREMENT,
  CaseId INTEGER,
  GoalName TEXT,
  TimeLimit INTEGER,
  Description TEXT,
  FOREIGN KEY (CaseId) REFERENCES cases(CaseId) ON DELETE CASCADE
);

-- Patient <> Illness relationships
CREATE TABLE IF NOT EXISTS patient_medical_history (
  PatientId INTEGER NOT NULL,
  IllnessId INTEGER NOT NULL,
  DateAdded TEXT,
  PRIMARY KEY (PatientId, IllnessId),
  FOREIGN KEY (PatientId) REFERENCES patients(PatientId) ON DELETE CASCADE,
  FOREIGN KEY (IllnessId) REFERENCES illnesses(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS patient_diagnoses (
  PatientId INTEGER NOT NULL,
  IllnessId INTEGER NOT NULL,
  DateDiagnosed TEXT,
  PRIMARY KEY (PatientId, IllnessId),
  FOREIGN KEY (PatientId) REFERENCES patients(PatientId) ON DELETE CASCADE,
  FOREIGN KEY (IllnessId) REFERENCES illnesses(Id) ON DELETE CASCADE
);

-- Patient <> Medication relationships
CREATE TABLE IF NOT EXISTS patient_medications (
  PatientId INTEGER NOT NULL,
  MedicationId INTEGER NOT NULL,
  StartDate TEXT,
  EndDate TEXT,
  PRIMARY KEY (PatientId, MedicationId, StartDate),
  FOREIGN KEY (PatientId) REFERENCES patients(PatientId) ON DELETE CASCADE,
  FOREIGN KEY (MedicationId) REFERENCES medications(MedicationId) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS patient_allergies (
  PatientId INTEGER NOT NULL,
  MedicationId INTEGER NOT NULL,
  PRIMARY KEY (PatientId, MedicationId),
  FOREIGN KEY (PatientId) REFERENCES patients(PatientId) ON DELETE CASCADE,
  FOREIGN KEY (MedicationId) REFERENCES medications(MedicationId) ON DELETE CASCADE
);

-- Seed common sexes
INSERT OR IGNORE INTO sexes (SexId, SexLabel) VALUES (1, 'Male'), (2, 'Female'), (3, 'Other');
