DECLARE
    NO_DATES EXCEPTION;
    cursor ord_date(p_Date DATE) IS
        SELECT onum, odate FROM ord WHERE odate>p_Date;

BEGIN
    FOR v_ord IN ord_date('01/04/2010') LOOP
        DBMS_OUTPUT.PUT_LINE('Order number '||v_ord.onum||' was complete '||v_ord.odate);
    END LOOP;

EXCEPTION
 WHEN OTHERS THEN
    DBMS_OUTPUT.PUT_LINE('xD Exception');
END;
/