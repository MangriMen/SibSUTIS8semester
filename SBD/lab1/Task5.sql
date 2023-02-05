DECLARE
    invalid_rating exception;
    cust_rating NUMBER(3, 0);

BEGIN
    SELECT Min(rating) INTO cust_rating FROM CUST;

    IF cust_rating < 200 THEN
        RAISE invalid_rating;
    END IF;
    COMMIT;

    EXCEPTION
        WHEN invalid_rating THEN
            DBMS_OUTPUT.PUT_LINE('Error invalid_rating');
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('Unhandled error (other)');

END;