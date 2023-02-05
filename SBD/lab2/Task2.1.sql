DECLARE
  vname VARCHAR(20);
  vcomm NUMBER(7, 2);

  CURSOR cur1 IS
  SELECT sname, comm FROM sal WHERE city <> 'London';

BEGIN
  IF NOT cur1%ISOPEN THEN
    OPEN cur1;
  END IF;

  LOOP
    FETCH cur1 INTO vname, vcomm;

    IF cur1%NOTFOUND OR cur1%ROWCOUNT = 3 THEN
      EXIT;
    END IF;
    
    DBMS_OUTPUT.PUT_LINE('Продавец: ' || vname || ' Комиссионные: ' || vcomm);
  END LOOP;
END;
/