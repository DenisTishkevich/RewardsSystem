namespace RewardsSystem.Service.Models;

public class CustomerView
{
    public int Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<TransactionPerMonthView> PointsAmountPerMonth { get; set; }

    public decimal PointsAmount { get; set; }

    public IEnumerable<TransactionView> Transactions { get; set; }
}
