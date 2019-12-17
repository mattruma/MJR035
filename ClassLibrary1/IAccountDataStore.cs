﻿using System;
using System.Threading.Tasks;

namespace ClassLibrary1
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
            Guid id,
            string accountNumber);

        Task UpdateByIdAsync(
            Guid id,
            string accountNumber,
            AccountData accountData);
    }
}
