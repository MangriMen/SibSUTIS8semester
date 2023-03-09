DROP SEQUENCE highway_sequence;
DROP SEQUENCE companies_sequence;

CREATE SEQUENCE highway_sequence START WITH 1;
CREATE SEQUENCE companies_sequence START WITH 1;

CREATE OR REPLACE PACKAGE Pack_1 IS
    PROCEDURE tables_init;
    PROCEDURE tables_delete;
    END Pack_1;
/


CREATE OR REPLACE PACKAGE BODY Pack_1 IS
PROCEDURE tables_init IS
BEGIN
    INSERT INTO construction_companies VALUES (companies_sequence.nextval, 'Дора Дорога');
    INSERT INTO construction_companies VALUES (companies_sequence.nextval, 'Яма На Яме');
    INSERT INTO construction_companies VALUES (companies_sequence.nextval, 'Цемент И Кирпич');
    INSERT INTO construction_companies VALUES (companies_sequence.nextval, 'Песок на рану');
    INSERT INTO construction_companies VALUES (companies_sequence.nextval, 'Разлом Сан-Андреас');
    INSERT INTO construction_companies VALUES (companies_sequence.nextval, 'Гудрон Из Гондураса');
    INSERT INTO construction_companies VALUES (companies_sequence.nextval, 'Биврёст Одина');
    INSERT INTO construction_companies VALUES (companies_sequence.nextval, 'И так сойдёт');

    INSERT INTO federal_highway VALUES (highway_sequence.nextval, 1, 4678, 'Первая Параллельная');
    INSERT INTO federal_highway VALUES (highway_sequence.nextval, 2, 8888, 'Вторая Прямая');
    INSERT INTO federal_highway VALUES (highway_sequence.nextval, 3, 1585, 'Третья Кривая');
    INSERT INTO federal_highway VALUES (highway_sequence.nextval, 4, 4567, 'В Вальхаллу');
    INSERT INTO federal_highway VALUES (highway_sequence.nextval, 5, 1345, 'КудаТутПоворачивать-17');
    INSERT INTO federal_highway VALUES (highway_sequence.nextval, 6, 200, 'Тупиковая');
    INSERT INTO federal_highway VALUES (highway_sequence.nextval, 7, 400, 'Ветвь');
    INSERT INTO federal_highway VALUES (highway_sequence.nextval, 8, 5900, 'Развитие');
    INSERT INTO federal_highway VALUES (highway_sequence.nextval, 8, 5900, 'Стагнация');
    

    EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.put_line(SQLERRM);
        ROLLBACK;
END;

PROCEDURE tables_delete IS
BEGIN
    DELETE FROM construction_companies;
    DELETE FROM federal_highway;
    COMMIT;

    EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.put_line(SQLERRM);
        ROLLBACK;
END;
END Pack_1;
/