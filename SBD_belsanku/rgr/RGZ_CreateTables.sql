CREATE TABLE construction_companies
(
    company_id NUMBER(4),
    company_name VARCHAR2(100) NOT NULL,
    CONSTRAINT construction_companies_pk PRIMARY KEY (company_id)
);

CREATE TABLE federal_highway
(
    highway_id NUMBER(4),
    company_id NUMBER(4),
    highway_cost NUMBER(7),
    highway_name VARCHAR2(100) NOT NULL,
    CONSTRAINT federal_highway_pk PRIMARY KEY (highway_id),
    CONSTRAINT federal_highway_fk FOREIGN KEY (company_id) REFERENCES construction_companies(company_id) ON delete CASCADE
);