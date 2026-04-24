-- Database template generated from PasientSimulator-Oblig4 models
PRAGMA foreign_keys = ON;

-- Users
CREATE TABLE users (
  id INTEGER PRIMARY KEY,
  role INTEGER NOT NULL,
  name TEXT
);

-- Patients
CREATE TABLE patients (
  patient_id INTEGER PRIMARY KEY,
  status INTEGER,
  patient_name TEXT,
  weight INTEGER,
  age INTEGER,
  sex TEXT,
  heartrate INTEGER,
  bloodpressure INTEGER,
  respiratory_rate INTEGER,
  oxygen_saturation REAL,
  temperature REAL
);

-- Medications
CREATE TABLE medications (
  medication_id INTEGER PRIMARY KEY,
  medication_name TEXT,
  dosage INTEGER,
  administration_roots TEXT,
  effects_json TEXT -- optional JSON array of effects
);

-- Medication effects (one row per effect) - alternate to effects_json
CREATE TABLE medication_effects (
  id INTEGER PRIMARY KEY,
  medication_id INTEGER NOT NULL,
  effect TEXT,
  FOREIGN KEY(medication_id) REFERENCES medications(medication_id) ON DELETE CASCADE
);

-- Illnesses
CREATE TABLE illnesses (
  id INTEGER PRIMARY KEY,
  name TEXT,
  antidote_medication_id INTEGER,
  FOREIGN KEY(antidote_medication_id) REFERENCES medications(medication_id)
);

-- Cases
CREATE TABLE cases (
  case_id INTEGER PRIMARY KEY,
  patient_id INTEGER,
  student_id INTEGER,
  FOREIGN KEY(patient_id) REFERENCES patients(patient_id) ON DELETE SET NULL,
  FOREIGN KEY(student_id) REFERENCES users(id) ON DELETE SET NULL
);

-- Goals (each goal belongs to a case)
CREATE TABLE goals (
  goal_id INTEGER PRIMARY KEY,
  case_id INTEGER,
  goal_name TEXT,
  time_limit INTEGER,
  description TEXT,
  FOREIGN KEY(case_id) REFERENCES cases(case_id) ON DELETE CASCADE
);

-- Patient <> Illness relationships
CREATE TABLE patient_medical_history (
  patient_id INTEGER NOT NULL,
  illness_id INTEGER NOT NULL,
  PRIMARY KEY(patient_id, illness_id),
  FOREIGN KEY(patient_id) REFERENCES patients(patient_id) ON DELETE CASCADE,
  FOREIGN KEY(illness_id) REFERENCES illnesses(id) ON DELETE CASCADE
);

CREATE TABLE patient_diagnoses (
  patient_id INTEGER NOT NULL,
  illness_id INTEGER NOT NULL,
  PRIMARY KEY(patient_id, illness_id),
  FOREIGN KEY(patient_id) REFERENCES patients(patient_id) ON DELETE CASCADE,
  FOREIGN KEY(illness_id) REFERENCES illnesses(id) ON DELETE CASCADE
);

-- Patient <> Medication relationships
CREATE TABLE patient_medications (
  patient_id INTEGER NOT NULL,
  medication_id INTEGER NOT NULL,
  PRIMARY KEY(patient_id, medication_id),
  FOREIGN KEY(patient_id) REFERENCES patients(patient_id) ON DELETE CASCADE,
  FOREIGN KEY(medication_id) REFERENCES medications(medication_id) ON DELETE CASCADE
);

CREATE TABLE patient_allergies (
  patient_id INTEGER NOT NULL,
  medication_id INTEGER NOT NULL,
  PRIMARY KEY(patient_id, medication_id),
  FOREIGN KEY(patient_id) REFERENCES patients(patient_id) ON DELETE CASCADE,
  FOREIGN KEY(medication_id) REFERENCES medications(medication_id) ON DELETE CASCADE
);

-- Notes:
-- - Medication.effects is modeled as both medication_effects (relational) and effects_json (simple store). Choose one.
-- - Context model is empty and therefore not represented.
-- - Adjust types and constraints to match the intended DB (SQLite, Postgres, etc.).
