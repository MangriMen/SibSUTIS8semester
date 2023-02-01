DECLARE
    error_value VARCHAR2(20);

BEGIN
    SELECT Snum INTO error_value FROM sal WHERE city = sname;

    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('Error!!');

END;

/