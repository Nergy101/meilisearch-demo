/* 
    Initial setup
    Adds Movies -table
*/
CREATE TABLE IF NOT EXISTS Movies (
    Id INT PRIMARY KEY,
    Title VARCHAR(255),
    Year INT
);