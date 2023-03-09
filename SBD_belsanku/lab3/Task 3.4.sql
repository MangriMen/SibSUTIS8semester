CREATE OR REPLACE PACKAGE lib IS
  FUNCTION shorter_surname (first_surname VARCHAR2, second_surname VARCHAR2) RETURN VARCHAR2;
  FUNCTION longer_surname (first_surname VARCHAR2, second_surname VARCHAR2) RETURN VARCHAR2;
END lib;
/

CREATE OR REPLACE PACKAGE BODY lib IS
  FUNCTION shorter_surname (first_surname VARCHAR2, second_surname VARCHAR2)
  RETURN VARCHAR2 IS returnSurname VARCHAR(20);
  BEGIN
    IF length(first_surname) > length(second_surname) THEN
      returnSurname := second_surname;
    ELSE
      returnSurname := first_surname;
    END IF;
    
    return returnSurname;
  END shorter_surname;

  FUNCTION longer_surname (first_surname VARCHAR2, second_surname VARCHAR2)
  RETURN VARCHAR2 IS returnSurname VARCHAR(20);
  BEGIN
    IF length(first_surname) > length(second_surname) THEN
      returnSurname := first_surname;
    ELSE
      returnSurname := second_surname;
    END IF;

    return returnSurname;
  END longer_surname;

END lib;
/

CREATE OR REPLACE PACKAGE caller IS
  PROCEDURE print;
END caller;
/

CREATE OR REPLACE PACKAGE BODY caller IS
  shorterSurname VARCHAR(20);
  longerSurname VARCHAR(20);
  
  PROCEDURE print IS
  BEGIN
    shorterSurname := lib.shorter_surname('Belonogov', 'Stepanov');
    longerSurname := lib.longer_surname('Belonogov', 'Stepanov');

    dbms_output.put_line('Short dick man: ' || shorterSurname);
    dbms_output.put_line('Big dick boy: ' || longerSurname);
  END print;
END caller;
/

BEGIN
  caller.print;
END;
/