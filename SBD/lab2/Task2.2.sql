DECLARE
    CURSOR cur(p_Date DATE) IS
    SELECT onum, amt, odate FROM ord
    WHERE odate > p_Date;

BEGIN
    FOR v_ord IN cur(TO_DATE('04.01.2010', 'dd.mm.yyyy')) LOOP
        DBMS_OUTPUT.PUT_LINE('Заказ '||v_ord.onum||' сумма '||v_ord.amt||' (дата '||v_ord.odate||' )');
    END LOOP;

END;
/