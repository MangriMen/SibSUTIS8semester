DECLARE
    BAD_RATING EXCEPTION;
    cust_rating NUMBER(3,0);
    cust_name VARCHAR(20);
BEGIN
    SELECT cname, rating INTO cust_name, cust_rating FROM cust WHERE cnum = 2001;

    IF cust_rating < 200 THEN
        RAISE BAD_RATING;
    ELSE 
        DBMS_OUTPUT.PUT_LINE('Custumer ' || cust_name || 'with rating' || cust_rating);
    END IF;
 
EXCEPTION
WHEN BAD_RATING THEN
    DBMS_OUTPUT.PUT_LINE('Please, change number of the seller. This one sucks');
WHEN OTHERS THEN
    DBMS_OUTPUT.PUT_LINE('Ohhh my god, you have BIG problems!');

END;