CREATE OR REPLACE FUNCTION TO_STRING_RUS(amt IN NUMBER DEFAULT NULL)
RETURN VARCHAR2
IS
  rubles NUMBER;
  penny NUMBER;
  rublesStr VARCHAR(20);
  pennyStr VARCHAR(20);
  str VARCHAR2(64);
BEGIN
    rubles := trunc(amt);
    penny := to_number(regexp_replace(to_char(mod(amt, 1)), '\.', ''));

    IF trunc(mod(rubles, 100) / 10) = 1 THEN
      rublesStr := 'рублей';
    ELSIF mod(rubles, 10) = 1 THEN
      rublesStr := 'рубль';
    ELSIF mod(rubles, 10) >= 2 AND mod(rubles, 10) <= 4 THEN
      rublesStr := 'рубля'; 
    ELSE
      rublesStr := 'рублей';
    END IF;

    IF trunc(mod(penny, 100) / 10) = 1 THEN
      pennyStr := 'копеек';
    ELSIF mod(penny, 10) = 1 THEN
      pennyStr := 'копейка';
    ELSIF mod(penny, 10) >= 2 AND mod(penny, 10) <= 4 THEN
      pennyStr := 'копейки'; 
    ELSE
      pennyStr := 'копеек';
    END IF;

    str := rubles || ' ' || rublesStr || ' ' || penny || ' ' || pennyStr;
    
    RETURN(str);
END;
/

SELECT onum, TO_STRING_RUS(amt) FROM ord;