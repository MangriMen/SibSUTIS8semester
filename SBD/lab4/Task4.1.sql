DROP TABLE logs;
CREATE TABLE logs (command VARCHAR(20), table_name VARCHAR(20), username VARCHAR2(20), time DATE);

DROP TABLE emp;
CREATE TABLE emp (thing VARCHAR2(20));

CREATE TRIGGER logger
AFTER INSERT ON emp
FOR EACH ROW

BEGIN
  INSERT INTO logs VALUES ('insert', 'emp', user, sysdate);
END;