DECLARE
    max_date DATE;
BEGIN
    SELECT MAX(odate) INTO max_date FROM ord;

    DBMS_OUTPUT.PUT_LINE(max_date);

END;