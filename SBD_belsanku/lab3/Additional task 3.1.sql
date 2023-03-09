DROP TABLE voltage;
DROP SEQUENCE uniq_id;
CREATE SEQUENCE uniq_id START WITH 1;

CREATE TABLE voltage
(
    voltage_id NUMBER(4),
    course_number NUMBER(4),
    view_control VARCHAR2(20),
    pass_number NUMBER(4)
);


INSERT INTO voltage VALUES (uniq_id.nextval, 1, 'зачёт', 3);
INSERT INTO voltage VALUES (uniq_id.nextval, 2, 'экзамен', 2);
INSERT INTO voltage VALUES (uniq_id.nextval, 3, 'экзамен', 1);
INSERT INTO voltage VALUES (uniq_id.nextval, 4, 'зачёт', 1);
INSERT INTO voltage VALUES (uniq_id.nextval, 2, 'экзамен', 4);
INSERT INTO voltage VALUES (uniq_id.nextval, 3, 'зачёт', 3);
INSERT INTO voltage VALUES (uniq_id.nextval, 4, 'зачёт', 2);