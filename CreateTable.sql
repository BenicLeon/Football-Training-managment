CREATE TABLE "Team" (
    team_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    team_name VARCHAR(100) NOT NULL,
    city VARCHAR(100) NOT NULL,
    stadium VARCHAR(100) NOT NULL,
    founded_year INT
);


CREATE TABLE "Players" (
    player_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    team_id UUID NOT NULL,
    player_name VARCHAR(100) NOT NULL,
    position VARCHAR(50),
    number INT,
    age INT,
    nationality VARCHAR(50),
    FOREIGN KEY (team_id) REFERENCES "Team"(team_id)
);