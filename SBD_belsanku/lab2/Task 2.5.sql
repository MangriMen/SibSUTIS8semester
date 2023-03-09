DROP SEQUENCE orderSeq;

CREATE SEQUENCE orderSeq
  INCREMENT BY 10
  START WITH 5000;

DROP TABLE carib_TC;

CREATE TABLE carib_TC (id INT, sname VARCHAR2(10), comm NUMBER(7,2));

DECLARE
  CURSOR curs IS
  SELECT sname, comm FROM sal;
BEGIN
  FOR rec IN curs
  LOOP
    INSERT INTO carib_TC VALUES(orderSeq.NEXTVAL, rec.sname, rec.comm);
  END LOOP;
END;
/