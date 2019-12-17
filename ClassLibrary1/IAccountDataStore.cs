using System;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public interface IAccountDataStore
    {
        Task AddAsync(
            AccountData accountData);
        Task DeleteByIdAsync(
             Guid id);
        Task<AccountData> FetchByIdAsync(
            Guid id);
        Task UpdateByIdAsync(
            Guid id,
            AccountData accountData);
    }
}
