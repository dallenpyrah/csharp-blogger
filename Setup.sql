USE bloggerdallen;

-- CREATE TABLE profiles
-- (
--     id VARCHAR(255) NOT NULL,
--     name VARCHAR(255) NOT NULL,
--     email VARCHAR(255) NOT NULL,
--     picture VARCHAR(255) NOT NULL,

--     PRIMARY KEY (id)
-- )


CREATE TABLE theblogs
(
    id INT NOT NULL AUTO_INCREMENT,
    title VARCHAR(255) NOT NULL,
    body VARCHAR(255),
    imgUrl VARCHAR(255),
    published BOOLEAN,
    creatorId VARCHAR(255) NOT NULL,

    PRIMARY KEY (id),

    FOREIGN KEY (creatorId)
    REFERENCES profiles (id)
    ON DELETE CASCADE
)

-- CREATE TABLE comments
-- (
--     id INT NOT NULL AUTO_INCREMENT,
--     creatorId VARCHAR(255) NOT NULL,
--     body VARCHAR(255) NOT NULL,
--     blogId INT NOT NULL,

--     PRIMARY KEY (id),

--     FOREIGN KEY (blogId)
--         REFERENCES blogs (id)
--         ON DELETE CASCADE
-- );