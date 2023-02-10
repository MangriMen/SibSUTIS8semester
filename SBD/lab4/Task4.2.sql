DROP TABLE logs;
CREATE TABLE logs (command VARCHAR(20), table_name VARCHAR(20), username VARCHAR2(20), time DATE);

DROP TABLE emp;
CREATE TABLE emp (thing VARCHAR2(20));

CREATE TRIGGER logger
AFTER INSERT OR DELETE OR UPDATE ON emp
FOR EACH ROW

DECLARE
  operation VARCHAR2(20);
BEGIN
  IF INSERTING THEN
    operation := 'insert';
  ELSIF UPDATING THEN
    operation := 'update';
  ELSIF DELETING THEN
    operation := 'delete';
  END IF;

  INSERT INTO logs VALUES (operation, 'emp', user, sysdate);
END;