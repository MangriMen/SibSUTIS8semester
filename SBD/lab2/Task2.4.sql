DROP SEQUENCE seq;

CREATE SEQUENCE seq
  INCREMENT BY 20
  START WITH 20
  MAXVALUE 100;

DROP TABLE test_seq;

CREATE TABLE test_seq(num NUMBER);

INSERT INTO test_seq(num) VALUES(seq.NEXTVAL);
INSERT INTO test_seq(num) VALUES(seq.NEXTVAL);
INSERT INTO test_seq(num) VALUES(seq.NEXTVAL);

SELECT * FROM test_seq;