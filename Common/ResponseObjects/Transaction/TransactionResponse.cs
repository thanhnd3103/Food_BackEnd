using Common.Status;

namespace Common.ResponseObjects.Transaction;

public class TransactionResponse
{
    public int TransactionID { get; set; }
    public string? BankCode { get; set; }
    public decimal TotalPrice { get; set; }
    public TransactionHistoryStatus Status { get; set; }
    public DateTime TransactionDate { get; set; }
}