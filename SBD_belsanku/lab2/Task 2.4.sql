DROP SEQUENCE orderSeq;

CREATE SEQUENCE orderSeq
  INCREMENT BY 10
  START WITH 5
  MAXVALUE 75;

DROP TABLE orderSeq_table;

CREATE TABLE orderSeq_table(num NUMBER);

INSERT INTO orderSeq_table(num) VALUES(orderSeq.NEXTVAL);
INSERT INTO orderSeq_table(num) VALUES(orderSeq.NEXTVAL);
INSERT INTO orderSeq_table(num) VALUES(orderSeq.NEXTVAL);

SELECT * FROM orderSeq_table;