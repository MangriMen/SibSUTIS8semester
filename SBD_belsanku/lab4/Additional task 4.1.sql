DROP TABLE logs;
CREATE TABLE logs (command VARCHAR(20), table_name VARCHAR(20), username VARCHAR2(20), time DATE);

CREATE TRIGGER additional_logger
AFTER INSERT OR DELETE ON cust
FOR EACH ROW

DECLARE
  operation VARCHAR2(20);
BEGIN
  IF INSERTING THEN
    operation := 'insert';
  ELSIF DELETING THEN
    operation := 'delete';
  END IF;

  INSERT INTO logs VALUES (operation, 'emp', user, sysdate);
END;