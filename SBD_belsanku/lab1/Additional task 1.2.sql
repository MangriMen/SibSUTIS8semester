DECLARE
    CUSTOM_EXCEPTION EXCEPTION;
    avg_price NUMBER(7,2);
    high_price NUMBER(7,2);
    cust_name VARCHAR(20);
    cust_number NUMBER;
    CURSOR current_cur IS
        SELECT amt, cnum FROM ord;

BEGIN
    SELECT AVG(amt) INTO avg_price FROM ord;
    DBMS_OUTPUT.PUT_LINE('Average amount: ' || avg_price);

    FOR x IN current_cur LOOP
        IF x.amt > avg_price THEN
            cust_number := x.cnum;
            SELECT cname INTO cust_name FROM cust WHERE cnum = cust_number;
            RAISE CUSTOM_EXCEPTION;
        END IF;
    END LOOP;

EXCEPTION
    WHEN CUSTOM_EXCEPTION THEN
        DBMS_OUTPUT.PUT_LINE('Catch the seller: ' || cust_name);

END;
/