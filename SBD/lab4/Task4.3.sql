DROP TABLE mysal;

CREATE TABLE mysal AS (SELECT * FROM sal);

CREATE OR REPLACE TRIGGER SAL_NOT_FROM_ROME
  BEFORE INSERT ON mysal
  FOR EACH ROW
BEGIN
    IF :new.city = 'Rome' THEN
      Raise_application_error(-20993, 'Sellers from ' || :new.city || ' are not allowed');
    END IF;
END; 