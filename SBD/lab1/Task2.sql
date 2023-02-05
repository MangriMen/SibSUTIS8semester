DROP TABLE n_sal;

CREATE TABLE n_sal
  (
     text VARCHAR2 (20),
     cnt  VARCHAR2 (20)
  );

DECLARE
    town      VARCHAR2(20);
    count_sal VARCHAR2(20);
BEGIN
    town := 'Paris';

    SELECT Count(*)
    INTO   count_sal
    FROM   sal
    WHERE  city = town;

    IF count_sal > 0 THEN
      INSERT INTO n_sal
      VALUES ('In ' || town, count_sal);
    ELSE
      INSERT INTO n_sal
      VALUES ('In ' || town, 'No data');
    END IF;
    
    COMMIT;
END;

/
SELECT *
FROM   n_sal; 