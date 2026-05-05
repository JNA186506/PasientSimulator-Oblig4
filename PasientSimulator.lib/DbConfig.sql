DROP TABLE IF EXISTS casegoals;
DROP TABLE IF EXISTS goals;
DROP TABLE IF EXISTS event;
DROP TABLE IF EXISTS cases;
DROP TABLE IF EXISTS allergies;
DROP TABLE IF EXISTS patientmedications;
DROP TABLE IF EXISTS diagnoses;
DROP TABLE IF EXISTS medicalhistory;
DROP TABLE IF EXISTS patients;
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
    antidoteid  INTEGER REFERENCES medications (medicationid) ON DELETE SET NULL
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
    patientid INTEGER NOT NULL REFERENCES patients (patientid) ON DELETE CASCADE,
    illnessid INTEGER NOT NULL REFERENCES illnesses (illnessid) ON DELETE CASCADE,
    PRIMARY KEY (patientid, illnessid)
);

CREATE TABLE diagnoses
(
    patientid INTEGER NOT NULL REFERENCES patients (patientid) ON DELETE CASCADE,
    illnessid INTEGER NOT NULL REFERENCES illnesses (illnessid) ON DELETE CASCADE,
    PRIMARY KEY (patientid, illnessid)
);

CREATE TABLE patientmedications
(
    patientid    INTEGER NOT NULL REFERENCES patients (patientid) ON DELETE CASCADE,
    medicationid INTEGER NOT NULL REFERENCES medications (medicationid) ON DELETE CASCADE,
    PRIMARY KEY (patientid, medicationid)
);

CREATE TABLE allergies
(
    patientid    INTEGER NOT NULL REFERENCES patients (patientid) ON DELETE CASCADE,
    medicationid INTEGER NOT NULL REFERENCES medications (medicationid) ON DELETE CASCADE,
    PRIMARY KEY (patientid, medicationid)
);

CREATE TABLE cases
(
    caseid    SERIAL PRIMARY KEY,
    patientid INTEGER NOT NULL REFERENCES patients (patientid) ON DELETE CASCADE,
    userid    INTEGER NOT NULL REFERENCES users (userid) ON DELETE RESTRICT
);

CREATE TABLE goals
(
    goalid      SERIAL PRIMARY KEY,
    goalname    VARCHAR(255),
    timelimit   INTEGER,
    description TEXT,
    caseid      INTEGER REFERENCES cases (caseid) ON DELETE CASCADE
);

CREATE TABLE casegoals
(
    caseid INTEGER REFERENCES cases (caseid) ON DELETE CASCADE,
    goalid INTEGER REFERENCES goals (goalid) ON DELETE CASCADE,
    PRIMARY KEY (caseid, goalid)
);

CREATE TABLE event
(
    eventid     SERIAL PRIMARY KEY,
    eventtype   INTEGER,
    description TEXT,
    timeadded   TIMESTAMP,
    caseid      INTEGER REFERENCES cases (caseid) ON DELETE CASCADE,
    userid      INTEGER REFERENCES users (userid) ON DELETE SET NULL
);