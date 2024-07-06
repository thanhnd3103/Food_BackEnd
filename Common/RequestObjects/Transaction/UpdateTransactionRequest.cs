using System.ComponentModel.DataAnnotations;

namespace Common.RequestObjects.Transaction;

public class UpdateTransactionRequest
{
    [Required]
    public string bankCode { get; set; }
}