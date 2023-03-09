CREATE OR REPLACE PACKAGE Pack_2 IS
    PROCEDURE tables_fill;
    PROCEDURE tables_clear;
    PROCEDURE change_highway_cost(find_id IN NUMBER, new_cost IN NUMBER);
    PROCEDURE show_biggest_companies(minimal_cost IN NUMBER, maximal_cost IN NUMBER);
    END Pack_2;
/

CREATE OR REPLACE PACKAGE BODY Pack_2 IS

CURSOR current_cur (minimal in NUMBER, maximal IN NUMBER) IS
    SELECT highway_cost, highway_name
    FROM federal_highway
    WHERE highway_cost > minimal AND highway_cost < maximal;

PROCEDURE tables_fill AS
BEGIN
    Pack_1.tables_init;
END tables_fill;

PROCEDURE tables_clear AS
BEGIN
    Pack_1.tables_delete;
END tables_clear;


PROCEDURE change_highway_cost(find_id IN NUMBER, new_cost IN NUMBER) AS
BEGIN
    UPDATE federal_highway 
    SET highway_cost = new_cost
    WHERE highway_id = find_id;
END change_highway_cost;



PROCEDURE show_biggest_companies(minimal_cost IN NUMBER, maximal_cost IN NUMBER) AS
BEGIN
    FOR it IN current_cur(minimal_cost, maximal_cost) LOOP
        DBMS_OUTPUT.PUT_LINE('Трасса ' || it.highway_name || ' стоит ' || it.highway_cost);
    END LOOP;
END show_biggest_companies;

END Pack_2;
/
