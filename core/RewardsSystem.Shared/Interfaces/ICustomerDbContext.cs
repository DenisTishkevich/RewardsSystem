using Microsoft.EntityFrameworkCore;
using RewardsSystem.Domain.Entities;

namespace RewardsSystem.Shared.Interfaces;

public interface ICustomerDbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}
