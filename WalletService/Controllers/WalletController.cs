using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalletService.Models;
namespace WalletService.Controllers;

[ApiController]
[Route("api/wallet")]
public class WalletController : ControllerBase
{
    private readonly WalletDbContext _context;

    public WalletController(WalletDbContext context)
    {
        _context = context;
    }

    // Get Wallet Balance
    // [Authorize]
    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        // var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        // if (userId == null) return Unauthorized();
        var userId = "12345678-1234-1234-1234-123456789abc";

        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == Guid.Parse(userId));
        if (wallet == null) return NotFound("Wallet not found.");

        return Ok(new { Balance = wallet.Balance });
    }

    // [Authorize]
    [HttpPost("savings/add")]
    public async Task<IActionResult> AddSavings([FromBody] SavingsRequest request)
    {
        // var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        // if (userId == null) return Unauthorized();
        var userId = "12345678-1234-1234-1234-123456789abc";

        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == Guid.Parse(userId));
        if (wallet == null) return NotFound("Wallet not found.");

        wallet.Savings += request.Amount;
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Savings updated successfully", Savings = wallet.Savings });
    }

    // Get Transaction History
    [Authorize]
    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactionHistory()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == Guid.Parse(userId));
        if (wallet == null) return NotFound("Wallet not found.");

        var transactions = _context.Transactions.Where(t => t.WalletId == wallet.Id).ToList();

        return Ok(transactions);
    }

    // Add Transaction
    [Authorize]
    [HttpPost("transactions/add")]
    public async Task<IActionResult> AddTransaction([FromBody] TransactionRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == Guid.Parse(userId));
        if (wallet == null) return NotFound("Wallet not found.");

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            WalletId = wallet.Id,
            Amount = request.Amount,
            Description = request.Description,
            TransactionDate = DateTime.UtcNow
        };

        _context.Transactions.Add(transaction);
        wallet.Balance += request.Amount; // Update balance
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Transaction added successfully", Balance = wallet.Balance });
    }
}
