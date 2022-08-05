using AutoMapper;
using RewardsSystem.Domain.Entities;
using RewardsSystem.Service.Models;

namespace RewardsSystem.Common.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Customer, CustomerView>();
        CreateMap<Transaction, TransactionView>();
    }
}
