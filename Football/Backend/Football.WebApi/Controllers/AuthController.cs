// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Football.WebApi;
using Football.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    private const string TrainerEmail = "trenermaric@gmail.com"; // Hardkodirani email za trenera
    private const string SecretaryEmail = "tajniklukic@gmail.com"; // Hardkodirani email za tajnika

    public AuthController(ILogger<AuthController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var userExistsQuery = "SELECT COUNT(1) FROM users WHERE email = @Email";
            using var userExistsCmd = new NpgsqlCommand(userExistsQuery, conn);
            userExistsCmd.Parameters.AddWithValue("@Email", model.Email);

            var userExists = (long)await userExistsCmd.ExecuteScalarAsync() > 0;
            if (userExists)
            {
                return BadRequest("Email already exists.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // Assign roles based on hardcoded email
            int roleId;
            if (model.Email == SecretaryEmail)
            {
                roleId = await GetRoleIdAsync(conn, "Secretary");
            }
            else if (model.Email == TrainerEmail)
            {
                roleId = await GetRoleIdAsync(conn, "Coach");
            }
            else
            {
                roleId = await GetRoleIdAsync(conn, "Player");
            }

            // Insert user into Users table and return the ID
            var insertUserQuery = @"
                INSERT INTO users (id, username, email, passwordhash) 
                VALUES (gen_random_uuid(), @UserName, @Email, @PasswordHash) RETURNING id";
            using var insertUserCmd = new NpgsqlCommand(insertUserQuery, conn);
            insertUserCmd.Parameters.AddWithValue("@UserName", model.UserName);
            insertUserCmd.Parameters.AddWithValue("@Email", model.Email);
            insertUserCmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

            var userId = (Guid)await insertUserCmd.ExecuteScalarAsync();

            // Insert user role into UserRoles table
            var insertUserRoleQuery = "INSERT INTO userroles (userid, roleid) VALUES (@UserId, @RoleId)";
            using var insertUserRoleCmd = new NpgsqlCommand(insertUserRoleQuery, conn);
            insertUserRoleCmd.Parameters.AddWithValue("@UserId", userId);
            insertUserRoleCmd.Parameters.AddWithValue("@RoleId", roleId);

            await insertUserRoleCmd.ExecuteNonQueryAsync();

            // If the role is Player, insert into Players table with an empty string contract
            var playerRoleId = await GetRoleIdAsync(conn, "Player");
            if (roleId == playerRoleId)
            {
                var insertPlayerQuery = @"
                    INSERT INTO players (id, name, contract, userid) 
                    VALUES (gen_random_uuid(), @UserName, ' ', @UserId)";
                using var insertPlayerCmd = new NpgsqlCommand(insertPlayerQuery, conn);
                insertPlayerCmd.Parameters.AddWithValue("@UserName", model.UserName);
                insertPlayerCmd.Parameters.AddWithValue("@UserId", userId);

                await insertPlayerCmd.ExecuteNonQueryAsync();
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return BadRequest();
        }
    }


    private async Task<int> GetRoleIdAsync(NpgsqlConnection conn, string roleName)
    {
        var getRoleIdQuery = "SELECT id FROM roles WHERE name = @RoleName";
        using var getRoleIdCmd = new NpgsqlCommand(getRoleIdQuery, conn);
        getRoleIdCmd.Parameters.AddWithValue("@RoleName", roleName);
        return (int)await getRoleIdCmd.ExecuteScalarAsync();
    }



    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var getUserQuery = "SELECT Id, PasswordHash FROM Users WHERE Email = @Email";
        using var getUserCmd = new NpgsqlCommand(getUserQuery, conn);
        getUserCmd.Parameters.AddWithValue("@Email", model.Email);

        using var reader = await getUserCmd.ExecuteReaderAsync();
        if (!reader.Read())
        {
            return NotFound(new { message = "User not found" });
        }

        var userId = reader.GetGuid(0);
        var passwordHash = reader.GetString(1);
        reader.Close();

        if (!BCrypt.Net.BCrypt.Verify(model.Password, passwordHash))
        {
            return Unauthorized(new { message = "Invalid password" });
        }

        var getUserRoleQuery = @"
        SELECT r.Name 
        FROM UserRoles ur
        JOIN Roles r ON ur.RoleId = r.Id
        WHERE ur.UserId = @UserId";
        using var getUserRoleCmd = new NpgsqlCommand(getUserRoleQuery, conn);
        getUserRoleCmd.Parameters.AddWithValue("@UserId", userId);

        var roleName = (string)await getUserRoleCmd.ExecuteScalarAsync();

        // Fetch the playerId if the user is a Player
        Guid? playerId = null;
        if (roleName == "Player")
        {
            var getPlayerIdQuery = "SELECT Id FROM Players WHERE UserId = @UserId";
            using var getPlayerIdCmd = new NpgsqlCommand(getPlayerIdQuery, conn);
            getPlayerIdCmd.Parameters.AddWithValue("@UserId", userId);

            playerId = (Guid)await getPlayerIdCmd.ExecuteScalarAsync();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(ClaimTypes.Email, model.Email),
        new Claim(ClaimTypes.Role, roleName)
    };

        if (playerId.HasValue)
        {
            claims.Add(new Claim("playerId", playerId.Value.ToString()));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Issuer"]
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString });
    }
    [HttpGet("users")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = new List<object>();
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = "SELECT id, username, email FROM users";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new
                {
                    Id = reader.GetGuid(reader.GetOrdinal("id")),
                    UserName = reader.GetString(reader.GetOrdinal("username")),
                    Email = reader.GetString(reader.GetOrdinal("email"))
                });
            }

            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching users");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("users/{id}")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var transaction = await conn.BeginTransactionAsync();
        try
        {
            
            var checkUserQuery = "SELECT COUNT(1) FROM users WHERE id = @Id";
            using var checkUserCmd = new NpgsqlCommand(checkUserQuery, conn);
            checkUserCmd.Parameters.AddWithValue("@Id", id);
            var userExists = (long)await checkUserCmd.ExecuteScalarAsync() > 0;

            if (!userExists)
            {
                return NotFound("User not found");
            }

            
            var deleteTrainingAttendeesQuery = "DELETE FROM trainingattendees WHERE playerid = (SELECT id FROM players WHERE userid = @Id)";
            using var deleteTrainingAttendeesCmd = new NpgsqlCommand(deleteTrainingAttendeesQuery, conn, transaction);
            deleteTrainingAttendeesCmd.Parameters.AddWithValue("@Id", id);
            await deleteTrainingAttendeesCmd.ExecuteNonQueryAsync();

            
            var deletePlayerQuery = "DELETE FROM players WHERE userid = @Id";
            using var deletePlayerCmd = new NpgsqlCommand(deletePlayerQuery, conn, transaction);
            deletePlayerCmd.Parameters.AddWithValue("@Id", id);
            await deletePlayerCmd.ExecuteNonQueryAsync();

            
            var deleteUserRolesQuery = "DELETE FROM userroles WHERE userid = @Id";
            using var deleteUserRolesCmd = new NpgsqlCommand(deleteUserRolesQuery, conn, transaction);
            deleteUserRolesCmd.Parameters.AddWithValue("@Id", id);
            await deleteUserRolesCmd.ExecuteNonQueryAsync();

            var deleteUserQuery = "DELETE FROM users WHERE id = @Id";
            using var deleteUserCmd = new NpgsqlCommand(deleteUserQuery, conn, transaction);
            deleteUserCmd.Parameters.AddWithValue("@Id", id);
            await deleteUserCmd.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            return Ok("User deleted successfully");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error deleting user: {Message}", ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }



    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        if (request.NewPassword != request.ConfirmPassword)
        {
            return BadRequest("Passwords do not match.");
        }

        var userId = GetUserIdFromToken();

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        var cmd = new NpgsqlCommand("UPDATE users SET passwordhash = @NewPasswordHash WHERE id = @UserId", conn);
        cmd.Parameters.AddWithValue("@NewPasswordHash", passwordHash);
        cmd.Parameters.AddWithValue("@UserId", userId);

        var rowsAffected = await cmd.ExecuteNonQueryAsync();
        if (rowsAffected > 0)
        {
            return Ok("Password changed successfully.");
        }
        else
        {
            return StatusCode(500, "An error occurred while changing the password.");
        }
    }

    private Guid GetUserIdFromToken()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }
}
