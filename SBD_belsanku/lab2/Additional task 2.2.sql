DECLARE
    OVERFLOW_EXCEPTION EXCEPTION;
    vname VARCHAR(20);
    vcomm NUMBER(7,2);
    maxcomm NUMBER(7,2);
    cnt NUMBER(4,0);
    CURSOR cur_sal IS
        SELECT sname, comm FROM sal;

    CURSOR cur_sal2 IS
        SELECT sname, comm FROM sal;

BEGIN  
    IF NOT cur_sal%ISOPEN THEN
        OPEN cur_sal;
    END IF;

    cnt := 0;
    maxcomm := 0;

    FOR it IN cur_sal2 LOOP
        IF maxcomm < it.comm THEN maxcomm := it.comm;
        END IF;
    END LOOP;

    LOOP
        FETCH cur_sal INTO vname, vcomm;
        IF vcomm < maxcomm THEN
            DBMS_OUTPUT.PUT_LINE(vname || ' with com ' || vcomm );
            cnt := cnt+1;
        END IF;
        IF cnt = 4 THEN
            EXIT;
        ELSIF cnt > 4 THEN
            RAISE OVERFLOW_EXCEPTION;
        END IF;
    END LOOP;


EXCEPTION
    WHEN OVERFLOW_EXCEPTION THEN
        DBMS_OUTPUT.PUT_LINE('Unluck-unluck');
END;
/

