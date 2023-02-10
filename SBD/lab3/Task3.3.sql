CREATE OR replace PACKAGE pack_2
IS
  PROCEDURE sal_cnt (city VARCHAR2);
  PROCEDURE cust_cnt (city VARCHAR2);
END pack_2;
/

CREATE OR replace PACKAGE BODY pack_2
IS
  cnt NUMBER(2);

  CURSOR sal_cur IS
  SELECT city FROM sal;

  CURSOR cust_cur IS
  SELECT city FROM cust;

  PROCEDURE Sal_cnt (city VARCHAR2)
  IS
  BEGIN
    cnt := 0;

    FOR rec in sal_cur
    LOOP
      IF rec.city = city THEN
        cnt := cnt + 1;
      END IF;
    END LOOP;

    dbms_output.Put_line('Sal count = ' || cnt);
  END sal_cnt;
  PROCEDURE Cust_cnt (city VARCHAR2)
  IS
  BEGIN
    cnt := 0;

    FOR rec in cust_cur
    LOOP
      IF rec.city = city THEN
        cnt := cnt + 1;
      END IF;
    END LOOP;

    dbms_output.Put_line('Cust count = ' || cnt);
  END cust_cnt;
END pack_2;
/

BEGIN
  pack_2.sal_cnt('London');
  pack_2.cust_cnt('San Jose');
END;
/