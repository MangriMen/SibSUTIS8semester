DECLARE
 last_date DATE;

BEGIN
    SELECT Max(Odate) INTO last_date FROM ord;
    DBMS_OUTPUT.PUT_LINE(last_date);
END;
