using System;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public interface IAccountEntityService
    {
        Task<AccountEntity> AddAsync(
            AccountEntityAddOptions accountEntityAddOptions);

        Task DeleteByIdAsync(
            Guid id,
            string accountNumber);

        Task<AccountEntity> FetchByAccountNumberAsync(
            string accountNumber);

        Task<AccountEntity> FetchByIdAsync(
            Guid id,
            string accountNumber);

        Task UpdateByIdAsync(
            Guid id,
            string accountNumber,
            AccountEntityUpdateOptions accountEntityUpdateOptions);
    }
}
