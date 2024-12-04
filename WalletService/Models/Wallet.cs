namespace WalletService.Models;

public class Wallet
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
    public decimal Savings { get; set; }
    public List<Transaction> Transactions { get; set; } = new();
}
