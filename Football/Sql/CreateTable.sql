-- Kreiranje users tablice
CREATE TABLE IF NOT EXISTS Users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    username VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL,
    passwordhash VARCHAR(256) NOT NULL,
    CONSTRAINT uq_email UNIQUE (email)
);

-- Kreiranje roles tablice
CREATE TABLE IF NOT EXISTS Roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL
);

-- Kreiranje user roles tablice
CREATE TABLE IF NOT EXISTS UserRoles (
    userid UUID,
    roleid INT,
    PRIMARY KEY (userid, roleid),
    FOREIGN KEY (userid) REFERENCES Users(id) ON DELETE CASCADE,
    FOREIGN KEY (roleid) REFERENCES Roles(id) ON DELETE CASCADE
);

-- Kreiranje players tablice
CREATE TABLE IF NOT EXISTS Players (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(100) NOT NULL,
    contract TEXT NOT NULL,
    userid UUID,
    FOREIGN KEY (userid) REFERENCES Users(id) ON DELETE CASCADE
);
