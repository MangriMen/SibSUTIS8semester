DROP TABLE additional_table;

CREATE OR REPLACE PACKAGE new_package IS
    PROCEDURE table_fill;
    END new_package;
/

CREATE TABLE additional_table
(
    control_type VARCHAR2(20),
    first_c NUMBER(2),
    second_c NUMBER(2),
    third_c NUMBER(2),
    fourth_c NUMBER(2)
);

CREATE OR REPLACE PACKAGE BODY new_package IS
    CURSOR new_cur IS
        SELECT *
        FROM voltage;

    i number(2);
    test_n NUMBER(2,0);
    exam_n NUMBER(2,0);

    PROCEDURE table_fill IS
    BEGIN
        INSERT INTO additional_table (control_type) VALUES ('экзамен');
        INSERT INTO additional_table (control_type) VALUES ('зачёт');

        i := 1;

FOR it IN new_cur LOOP
    IF it.course_number = i THEN
        SELECT SUM(pass_number) INTO test_n FROM voltage WHERE course_number = i AND view_control = 'зачёт';
        SELECT SUM(pass_number) INTO exam_n FROM voltage WHERE course_number = i AND view_control = 'экзамен';       
    END IF;

    IF i = 1 THEN
        UPDATE additional_table SET first_c = test_n WHERE control_type = 'зачёт';
        UPDATE additional_table SET first_c = exam_n WHERE control_type = 'экзамен';
    END IF;

    IF i = 2 THEN
        UPDATE additional_table SET second_c = test_n WHERE control_type = 'зачёт';
        UPDATE additional_table SET second_c = exam_n WHERE control_type = 'экзамен';
    END IF;

    IF i = 3 THEN
        UPDATE additional_table SET third_c = test_n WHERE control_type = 'зачёт';
        UPDATE additional_table SET third_c = exam_n WHERE control_type = 'экзамен';
    END IF;

    IF i = 4 THEN
        UPDATE additional_table SET fourth_c = test_n WHERE control_type = 'зачёт';
        UPDATE additional_table SET fourth_c = exam_n WHERE control_type = 'экзамен';
    END IF;

    i := i + 1;
    END LOOP;
    END table_fill;
END new_package;
/

BEGIN
    new_package.table_fill;
END;
/