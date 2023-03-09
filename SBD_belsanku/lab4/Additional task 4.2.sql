CREATE OR REPLACE VIEW additional_view AS
    SELECT sname AS Имя_продавца, SUM(amt) AS Сумма_заказов FROM sal, ord
    WHERE sal.snum = ord.snum GROUP BY sname;

GRANT select ON additional_view TO PUBLIC;