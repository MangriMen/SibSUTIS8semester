DROP TABLE dep;

CREATE TABLE dep(id NUMBER(5), name VARCHAR2(20));

INSERT INTO dep VALUES (0, 'IVT');
INSERT INTO dep VALUES (1, 'MRM');
INSERT INTO dep VALUES (2, 'Anemoi PC');
INSERT INTO dep VALUES (3, 'Hynix hot memory');
INSERT INTO dep VALUES (4, 'Pust budet');
INSERT INTO dep VALUES (5, 'Danil Memshikov');

CREATE OR REPLACE PROCEDURE change_dep_id(current_dep_id IN NUMBER, new_dep_id IN NUMBER) AS
BEGIN
    UPDATE dep SET id = new_dep_id WHERE id = current_dep_id;
END;
/

DECLARE
    CURSOR cur IS
    SELECT * FROM dep;
BEGIN
    FOR rec IN cur
    LOOP
      DBMS_OUTPUT.PUT_LINE(rec.id || '  ' || rec.name);
    END LOOP;

    change_dep_id(5, 10);
    change_dep_id(2, 11);

    DBMS_OUTPUT.PUT_LINE('===');
        FOR rec IN cur
    LOOP
      DBMS_OUTPUT.PUT_LINE(rec.id || '  ' || rec.name);
    END LOOP;
END;
/