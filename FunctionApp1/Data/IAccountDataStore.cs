using System;
using System.Threading.Tasks;

namespace FunctionApp1.Data
{
    public interface IAccountDataStore
    {
        Task AddAsync(
            AccountData accountData);

        Task DeleteByIdAsync(
            Guid id,
            string accountNumber);

        Task<AccountData> FetchByAccountNumberAsync(
            string accountNumber);

        Task<AccountData> FetchByIdAsync(
            Guid id);

        Task UpdateByIdAsync(
            Guid id,
            string accountNumber,
            AccountData accountData);
    }
}
