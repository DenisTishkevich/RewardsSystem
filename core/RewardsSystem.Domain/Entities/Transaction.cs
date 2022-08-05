using System.ComponentModel.DataAnnotations.Schema;

namespace RewardsSystem.Domain.Entities;

public class Transaction
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    public decimal Points { get; set; }

    public int CustomerId { get; set; }

    public Customer Customer { get; set; }
}