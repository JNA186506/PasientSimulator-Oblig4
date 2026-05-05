INSERT INTO users (role, name)
VALUES (1, 'DudeBro McMan');

INSERT INTO medications (medicationname, dosage, administrationroute)
VALUES ('Cheese', 3, 1);

INSERT INTO illnesses (illnessname, antidoteid)
VALUES ('Lack of cheese', 1);

INSERT INTO patients (status, patientname, weight, age, sex, heartrate, bloodpressure_systolic, bloodpressure_diastolic, respiratoryrate, oxygensaturation, temperature)
VALUES (
           1,
           'DudeBro McMan',
           5,
           42,
           1,
           42,
           12,
           13,
           10,
           120.15,
           37.2
       );

INSERT INTO medicalhistory (patientid, illnessid)
VALUES (1, 1);

INSERT INTO diagnoses (patientid, illnessid)
VALUES (1, 1);

INSERT INTO patientmedications (patientid, medicationid)
VALUES (1, 1);

INSERT INTO allergies (patientid, medicationid)
VALUES (1, 1);

INSERT INTO cases (patientid, userid)
VALUES (1, 1);

INSERT INTO goals (goalname, timelimit, description)
VALUES ('Cheeeeeeesey!', 15, 'Get this man some cheese!');

INSERT INTO casegoals (caseid, goalid)
VALUES (1, 1);

INSERT INTO event (caseid, eventtype, description, timeadded, userid) 
VALUES ( 1, 1, 'This was a good intervention', now(), 1);