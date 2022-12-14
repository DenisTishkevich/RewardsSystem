namespace RewardsSystem.Domain.Entities;

public class Customer
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<Transaction> Transactions { get; set; }
}
