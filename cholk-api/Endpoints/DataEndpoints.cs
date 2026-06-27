using System.Security.Claims;
using System.Text.Json;
using cholk_api.Data;
using cholk_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cholk_api.Endpoints;

public static class DataEndpoints
{
    private static Guid UserId(HttpContext ctx) =>
        Guid.Parse(ctx.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? ctx.User.FindFirstValue("sub")!);

    // ── Accounts ─────────────────────────────────────────────────────────────

    public static void MapData(this WebApplication app)
    {
        var accounts = app.MapGroup("/api/accounts").RequireAuthorization();

        accounts.MapGet("/", async (HttpContext ctx, AppDbContext db) =>
        {
            var uid = UserId(ctx);
            var list = await db.Accounts.Where(a => a.UserId == uid).ToListAsync();
            return Results.Ok(list.Select(ToDto));
        });

        accounts.MapPost("/", async (HttpContext ctx, AccountDto dto, AppDbContext db) =>
        {
            var uid = UserId(ctx);
            var entity = FromDto(dto);
            entity.UserId = uid;
            db.Accounts.Add(entity);
            await db.SaveChangesAsync();
            return Results.Ok(ToDto(entity));
        });

        accounts.MapPut("/{id:guid}", async (Guid id, HttpContext ctx, AccountDto dto, AppDbContext db) =>
        {
            var uid = UserId(ctx);
            var entity = await db.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.UserId == uid);
            if (entity is null) return Results.NotFound();
            ApplyDto(entity, dto);
            await db.SaveChangesAsync();
            return Results.Ok(ToDto(entity));
        });

        accounts.MapDelete("/{id:guid}", async (Guid id, HttpContext ctx, AppDbContext db) =>
        {
            var uid = UserId(ctx);
            var entity = await db.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.UserId == uid);
            if (entity is null) return Results.NotFound();
            db.Accounts.Remove(entity);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // ── Trades ───────────────────────────────────────────────────────────

        var trades = app.MapGroup("/api/trades").RequireAuthorization();

        trades.MapGet("/", async (HttpContext ctx, AppDbContext db) =>
        {
            var uid = UserId(ctx);
            var list = await db.Trades.Where(t => t.UserId == uid).ToListAsync();
            return Results.Ok(list);
        });

        trades.MapPost("/", async (HttpContext ctx, Trade trade, AppDbContext db) =>
        {
            var uid = UserId(ctx);
            trade.Id = Guid.NewGuid();
            trade.UserId = uid;
            db.Trades.Add(trade);
            await db.SaveChangesAsync();
            return Results.Ok(trade);
        });

        trades.MapPut("/{id:guid}", async (Guid id, HttpContext ctx, Trade trade, AppDbContext db) =>
        {
            var uid = UserId(ctx);
            var entity = await db.Trades.FirstOrDefaultAsync(t => t.Id == id && t.UserId == uid);
            if (entity is null) return Results.NotFound();
            trade.Id = id;
            trade.UserId = uid;
            db.Entry(entity).CurrentValues.SetValues(trade);
            await db.SaveChangesAsync();
            return Results.Ok(entity);
        });

        trades.MapDelete("/{id:guid}", async (Guid id, HttpContext ctx, AppDbContext db) =>
        {
            var uid = UserId(ctx);
            var entity = await db.Trades.FirstOrDefaultAsync(t => t.Id == id && t.UserId == uid);
            if (entity is null) return Results.NotFound();
            db.Trades.Remove(entity);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // ── Strategies ───────────────────────────────────────────────────────

        var strategies = app.MapGroup("/api/strategies").RequireAuthorization();

        strategies.MapGet("/", async (HttpContext ctx, AppDbContext db) =>
        {
            var uid = UserId(ctx);
            var list = await db.Strategies.Where(s => s.UserId == uid).ToListAsync();
            return Results.Ok(list.Select(ToStrategyDto));
        });

        strategies.MapPost("/", async (HttpContext ctx, StrategyDto dto, AppDbContext db) =>
        {
            var uid = UserId(ctx);
            var entity = FromStrategyDto(dto);
            entity.UserId = uid;
            db.Strategies.Add(entity);
            await db.SaveChangesAsync();
            return Results.Ok(ToStrategyDto(entity));
        });

        strategies.MapPut("/{id:guid}", async (Guid id, HttpContext ctx, StrategyDto dto, AppDbContext db) =>
        {
            var uid = UserId(ctx);
            var entity = await db.Strategies.FirstOrDefaultAsync(s => s.Id == id && s.UserId == uid);
            if (entity is null) return Results.NotFound();
            ApplyStrategyDto(entity, dto);
            await db.SaveChangesAsync();
            return Results.Ok(ToStrategyDto(entity));
        });

        strategies.MapDelete("/{id:guid}", async (Guid id, HttpContext ctx, AppDbContext db) =>
        {
            var uid = UserId(ctx);
            var entity = await db.Strategies.FirstOrDefaultAsync(s => s.Id == id && s.UserId == uid);
            if (entity is null) return Results.NotFound();
            db.Strategies.Remove(entity);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }

    // ── Account DTO mapping ───────────────────────────────────────────────────

    private static AccountDto ToDto(Account a) => new(
        a.Id, a.Name, a.Abbreviation, a.Subtitle, a.Type, a.Status,
        a.Balance, a.MonthPnl,
        JsonSerializer.Deserialize<List<decimal>>(a.EquityHistoryJson) ?? [],
        JsonSerializer.Deserialize<List<AccountOperationDto>>(a.OperationsJson) ?? []);

    private static Account FromDto(AccountDto dto)
    {
        var e = new Account { Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id };
        ApplyDto(e, dto);
        return e;
    }

    private static void ApplyDto(Account e, AccountDto dto)
    {
        e.Name = dto.Name;
        e.Abbreviation = dto.Abbreviation;
        e.Subtitle = dto.Subtitle;
        e.Type = dto.Type;
        e.Status = dto.Status;
        e.Balance = dto.Balance;
        e.MonthPnl = dto.MonthPnl;
        e.EquityHistoryJson = JsonSerializer.Serialize(dto.EquityHistory);
        e.OperationsJson = JsonSerializer.Serialize(dto.Operations);
    }

    // ── Strategy DTO mapping ─────────────────────────────────────────────────

    private static StrategyDto ToStrategyDto(Strategy s) => new(
        s.Id, s.Name, s.Description, s.Status, s.TotalPnl, s.WinRate,
        s.ProfitFactor, s.TradeCount,
        JsonSerializer.Deserialize<List<decimal>>(s.EquityHistoryJson) ?? []);

    private static Strategy FromStrategyDto(StrategyDto dto)
    {
        var e = new Strategy { Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id };
        ApplyStrategyDto(e, dto);
        return e;
    }

    private static void ApplyStrategyDto(Strategy e, StrategyDto dto)
    {
        e.Name = dto.Name;
        e.Description = dto.Description;
        e.Status = dto.Status;
        e.TotalPnl = dto.TotalPnl;
        e.WinRate = dto.WinRate;
        e.ProfitFactor = dto.ProfitFactor;
        e.TradeCount = dto.TradeCount;
        e.EquityHistoryJson = JsonSerializer.Serialize(dto.EquityHistory);
    }
}

// ── API DTOs (JSON-friendly, with deserialized lists) ─────────────────────────

public record AccountDto(
    Guid Id, string Name, string Abbreviation, string Subtitle,
    AccountType Type, AccountStatus Status,
    decimal Balance, decimal MonthPnl,
    List<decimal> EquityHistory,
    List<AccountOperationDto> Operations);

public record AccountOperationDto(Guid Id, decimal Amount, string Description, DateTime Date);

public record StrategyDto(
    Guid Id, string Name, string Description,
    StrategyStatus Status,
    decimal TotalPnl, decimal WinRate, decimal ProfitFactor, int TradeCount,
    List<decimal> EquityHistory);
