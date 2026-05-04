DROP TABLE IF EXISTS casegoals;
DROP TABLE IF EXISTS goals;
DROP TABLE IF EXISTS cases;
DROP TABLE IF EXISTS allergies;
DROP TABLE IF EXISTS patientmedications;
DROP TABLE IF EXISTS diagnoses;
DROP TABLE IF EXISTS medicalhistory;
DROP TABLE IF EXISTS patients;
DROP TABLE IF EXISTS illnesstreatments;
DROP TABLE IF EXISTS illnesses;
DROP TABLE IF EXISTS medications;
DROP TABLE IF EXISTS users;

CREATE TABLE users
(
    userid SERIAL PRIMARY KEY,
    role   INTEGER NOT NULL,
    name   TEXT
);

CREATE TABLE medications
(
    medicationid        SERIAL PRIMARY KEY,
    medicationname      VARCHAR(255),
    dosage              INTEGER,
    administrationroute INT
);

CREATE TABLE illnesses
(
    illnessid   SERIAL PRIMARY KEY,
    illnessname VARCHAR(255),
    antidoteid  INTEGER REFERENCES medications (medicationid)
);

CREATE TABLE patients
(
    patientid               SERIAL PRIMARY KEY,
    status                  INTEGER,
    patientname             TEXT,
    weight                  INTEGER,
    age                     INTEGER,
    sex                     INT,
    heartrate               INTEGER,
    bloodpressure_systolic  INTEGER,
    bloodpressure_diastolic INTEGER,
    respiratoryrate         INTEGER,
    oxygensaturation        FLOAT,
    temperature             FLOAT
);

CREATE TABLE medicalhistory
(
    patientid INTEGER NOT NULL REFERENCES patients (patientid),
    illnessid INTEGER NOT NULL REFERENCES illnesses (illnessid),
    PRIMARY KEY (patientid, illnessid)
);

CREATE TABLE diagnoses
(
    patientid INTEGER NOT NULL REFERENCES patients (patientid),
    illnessid INTEGER NOT NULL REFERENCES illnesses (illnessid),
    PRIMARY KEY (patientid, illnessid)
);

CREATE TABLE patientmedications
(
    patientid    INTEGER NOT NULL REFERENCES patients (patientid),
    medicationid INTEGER NOT NULL REFERENCES medications (medicationid),
    PRIMARY KEY (patientid, medicationid)
);

CREATE TABLE allergies
(
    patientid    INTEGER NOT NULL REFERENCES patients (patientid),
    medicationid INTEGER NOT NULL REFERENCES medications (medicationid),
    PRIMARY KEY (patientid, medicationid)
);

CREATE TABLE cases
(
    caseid    SERIAL PRIMARY KEY,
    patientid INTEGER NOT NULL REFERENCES patients (patientid),
    userid    INTEGER NOT NULL REFERENCES users (userid)
);

CREATE TABLE goals
(
    goalid      SERIAL PRIMARY KEY,
    goalname    VARCHAR(255),
    timelimit   INTEGER,
    description TEXT,
    caseid      INTEGER REFERENCES cases
);

CREATE TABLE casegoals
(
    caseid INTEGER REFERENCES cases,
    goalid INTEGER REFERENCES goals,
    PRIMARY KEY (caseid, goalid)
);
