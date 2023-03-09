DECLARE
    OVERFLOW_EXCEPTION EXCEPTION;
    vname VARCHAR(20);
    vcomm NUMBER(7,2);
    CURSOR cur_sal IS
        SELECT sname, comm FROM sal WHERE city <> 'London';

BEGIN  
    IF NOT cur_sal%ISOPEN THEN
        OPEN cur_sal;
    END IF;
    LOOP
        FETCH cur_sal INTO vname, vcomm;
        DBMS_OUTPUT.PUT_LINE('This debiloid not from London '|| vname || ' with com = ' || vcomm );
        IF cur_sal%ROWCOUNT = 2 THEN
            EXIT;
        ELSIF cur_sal%ROWCOUNT > 2 THEN
            RAISE OVERFLOW_EXCEPTION;
        END IF;
    END LOOP;


EXCEPTION
    WHEN OVERFLOW_EXCEPTION THEN
        DBMS_OUTPUT.PUT_LINE('You escaped from your luck, didnt you?');

    WHEN OTHERS THEN 
        DBMS_OUTPUT.PUT_LINE('Smeeeeeeeeert');
END;
/

