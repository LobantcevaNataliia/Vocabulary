CREATE database myVocabDB ;

Create table myVocabDB.words(
    WordID int NOT NULL PRIMARY KEY,
    EnglishWord varchar(255) NOT NULL UNIQUE,
    UkrainianWord varchar(255) NOT NULL); 
    
ALTER TABLE myVocabDB.words
MODIFY WordID int AUTO_INCREMENT;

ALTER TABLE myVocabDB.words
drop column  UkrainianWord;

ALTER TABLE myVocabDB.words
add column  UkrainianWord varchar(255) NOT NULL;

INSERT INTO myVocabDB.words (EnglishWord, Transcription, UkrainianWord)
VALUES ('Аlmost','ˈɔːlməust' , 'Майже '), 
('Alias','ˈeɪliəs' , 'Псевдонім'), 
('Near','nɪə(r)' , 'Близько'),
('Quotes', 'kwəut' , 'Лапки');

Select * from myVocabDB.words;