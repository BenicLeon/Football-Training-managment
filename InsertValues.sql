
INSERT INTO "Team" (team_id, team_name, city, stadium, founded_year) VALUES
(gen_random_uuid(), 'Chelsea', 'London', 'Stamford Bridge', 1905);

WITH team_info AS (
    SELECT team_id FROM "Team" WHERE team_name = 'Chelsea'
)

INSERT INTO "Players" (player_id, team_id, player_name, position, number, age, nationality) VALUES
(gen_random_uuid(), (SELECT team_id FROM team_info), 'Đorđe Petrović', 'Goalkeeper', 1, 30, 'Serbian'),
(gen_random_uuid(), (SELECT team_id FROM team_info), 'Levi Colwil', 'Defender', 5, 21, 'English'),
(gen_random_uuid(), (SELECT team_id FROM team_info), 'Enzo Fernandez', 'Midfielder', 8, 22, 'Argentinian');