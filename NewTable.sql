CREATE TABLE myvocabdb.users (
    UserId INT PRIMARY KEY AUTO_INCREMENT,
    UserName VARCHAR(50) NOT NULL,
    UserEmail VARCHAR(255) NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL   
    );
    
CREATE TABLE myvocabdb.learnedwords (
    UserId INT,
    WordId INT,
    PRIMARY KEY (UserId, WordId),
    FOREIGN KEY (UserId) REFERENCES myvocabdb.users(UserId),
    FOREIGN KEY (WordID) REFERENCES myvocabdb.words(WordID)
);

ALTER TABLE myvocabdb.learnedwords
ADD COLUMN Status BOOLEAN DEFAULT FALSE;