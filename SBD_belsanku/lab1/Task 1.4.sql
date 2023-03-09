DECLARE
    important_sname VARCHAR(20);
BEGIN
    SELECT sname INTO important_sname FROM sal WHERE snum = 10000;

EXCEPTION
    WHEN OTHERS
    THEN
        DBMS_OUTPUT.put_line('Seller does not exist. Up your hands to the sun!');

END;
/
    