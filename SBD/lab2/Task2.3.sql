SELECT sname,
    SUM(DECODE(ord.odate, TO_DATE('03.01.2010', 'dd.mm.yyyy'), ord.amt, null)) "03.01.2010",
    SUM(DECODE(ord.odate, TO_DATE('04.01.2010', 'dd.mm.yyyy'), ord.amt, null)) "04.01.2010",
    SUM(DECODE(ord.odate, TO_DATE('05.01.2010', 'dd.mm.yyyy'), ord.amt, null)) "05.01.2010",
    SUM(DECODE(ord.odate, TO_DATE('06.01.2010', 'dd.mm.yyyy'), ord.amt, null)) "06.01.2010"
FROM ord LEFT JOIN sal USING(snum) GROUP BY sname ORDER BY sname ASC;