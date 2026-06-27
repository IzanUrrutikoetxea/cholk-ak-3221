using cholk_api.Data;
using cholk_api.Models;
using cholk_api.Services;
using Microsoft.EntityFrameworkCore;

namespace cholk_api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuth(this WebApplication app)
    {
        app.MapPost("/auth/register", async (RegisterRequest req, AppDbContext db, TokenService tokens) =>
        {
            if (await db.Users.AnyAsync(u => u.Email == req.Email.ToLower()))
                return Results.Conflict(new { error = "Email already registered" });

            var user = new AppUser
            {
                Email = req.Email.ToLower(),
                DisplayName = req.DisplayName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Results.Ok(new AuthResponse(tokens.Generate(user), user.Email, user.DisplayName));
        });

        app.MapPost("/auth/login", async (LoginRequest req, AppDbContext db, TokenService tokens) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == req.Email.ToLower());
            if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                return Results.Unauthorized();

            return Results.Ok(new AuthResponse(tokens.Generate(user), user.Email, user.DisplayName));
        });
    }
}
