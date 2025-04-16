using RentNest.Core.Domains;

namespace RentNest.Service.Interfaces
{
    public interface IAccountService
    {
        Task<Account?> GetAccountByEmailAsync(string email);
    }

}
