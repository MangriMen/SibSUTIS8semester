DECLARE
    NO_CITY EXCEPTION;
    cursor ord_city(cur_city VARCHAR2) IS
        SELECT * FROM cust WHERE city = cur_city;

BEGIN
    FOR v_ord IN ord_city('London') LOOP
        DBMS_OUTPUT.PUT_LINE(v_ord.cnum || ' ' || v_ord.city || ' ' || v_ord.cname);
    END LOOP;

EXCEPTION
 WHEN OTHERS THEN
    DBMS_OUTPUT.PUT_LINE('Have trouble?');
END;
/