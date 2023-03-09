DECLARE
    CURSOR cur IS
        SELECT cnum FROM cust ORDER BY cnum DESC;
    pre NUMBER;

BEGIN
    FOR x IN cur LOOP
        IF cur%rowcount = 1 THEN
            pre := x.cnum;
            CONTINUE;
        END IF;
        IF pre-1 <> x.cnum THEN
           DBMS_OUTPUT.PUT_LINE(pre-1);
           EXIT;
        ELSE
            pre := pre-1;
        END IF;
    END LOOP;      
END;
/