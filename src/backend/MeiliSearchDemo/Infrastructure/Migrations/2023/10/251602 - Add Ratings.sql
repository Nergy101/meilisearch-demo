/* 
    Add Ratings -table
*/

CREATE TABLE IF NOT EXISTS Ratings (
    MovieId INT,
    Rating INT,
    Votes INT,
    FOREIGN KEY (MovieId) REFERENCES Movie(Id)
    );