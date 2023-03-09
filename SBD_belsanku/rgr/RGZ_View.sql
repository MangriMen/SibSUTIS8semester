CREATE OR REPLACE VIEW table_view AS
    SELECT company_name AS ДТО, highway_name AS Трасса FROM construction_companies, federal_highway
    WHERE construction_companies.company_id = federal_highway.company_id AND company_name <> 'Дора Дорога';