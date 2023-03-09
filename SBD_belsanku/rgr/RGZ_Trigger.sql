ALTER SESSION SET TIME_ZONE = '+7:0';

CREATE OR REPLACE TRIGGER table_trigger
BEFORE INSERT ON federal_highway FOR EACH ROW
DECLARE
    now_date NUMERIC(4);
BEGIN
    SELECT EXTRACT (DAY FROM CURRENT_DATE) INTO now_date FROM dual;
    IF now_date > 15 THEN
        raise_application_error(-20001, 'WRONG DATE');
    END IF;
END;
/