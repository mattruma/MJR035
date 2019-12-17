using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class AccountEntityService : IAccountEntityService
    {
        private IAccountDataStore _accountDataStore;

        public AccountEntityService(
            IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        public async Task<AccountEntity> AddAsync(
            AccountEntityAddOptions accountAddOptions)
        {
            if (string.IsNullOrWhiteSpace(accountAddOptions.AccountNumber))
            {
                throw new ValidationException($"'account_number' is required.");
            }

            if (string.IsNullOrWhiteSpace(accountAddOptions.SystemOfRecord))
            {
                throw new ValidationException($"'system_of_record' is required.");
            }

            var accountData =
                new AccountData
                {
                    AccountNumber = accountAddOptions.AccountNumber,
                    SystemOfRecord = accountAddOptions.SystemOfRecord,
                    PhoneNumber = accountAddOptions.PhoneNumber
                };

            await _accountDataStore.AddAsync(
                accountData);

            var accountEntity =
                new AccountEntity
                {
                    Id = accountData.Id,
                    CreatedOn = accountData.CreatedOn,
                    AccountNumber = accountData.AccountNumber,
                    SystemOfRecord = accountData.SystemOfRecord,
                    PhoneNumber = accountData.PhoneNumber
                };

            return accountEntity;
        }

        public async Task DeleteByIdAsync(
            Guid id,
            string accountNumber)
        {
            await _accountDataStore.DeleteByIdAsync(
                id,
                accountNumber);
        }

        public async Task<AccountEntity> FetchByAccountNumberAsync(
            string accountNumber)
        {
            var accountData =
                await _accountDataStore.FetchByAccountNumberAsync(
                    accountNumber);

            var accountEntity =
                new AccountEntity
                {
                    Id = accountData.Id,
                    CreatedOn = accountData.CreatedOn,
                    AccountNumber = accountData.AccountNumber,
                    SystemOfRecord = accountData.SystemOfRecord,
                    PhoneNumber = accountData.PhoneNumber
                };

            return accountEntity;
        }

        public async Task<AccountEntity> FetchByIdAsync(
            Guid id,
            string accountNumber)
        {
            var accountData =
                await _accountDataStore.FetchByIdAsync(
                    id,
                    accountNumber);

            var accountEntity =
                new AccountEntity
                {
                    Id = accountData.Id,
                    CreatedOn = accountData.CreatedOn,
                    AccountNumber = accountData.AccountNumber,
                    SystemOfRecord = accountData.SystemOfRecord,
                    PhoneNumber = accountData.PhoneNumber
                };

            return accountEntity;
        }

        public async Task UpdateByIdAsync(
            Guid id,
            string accountNumber,
            AccountEntityUpdateOptions accountEntityUpdateOptions)
        {
            var accountData =
                await _accountDataStore.FetchByIdAsync(
                    id,
                    accountNumber);

            accountData.SystemOfRecord =
                accountEntityUpdateOptions.SystemOfRecord;
            accountData.PhoneNumber =
                accountEntityUpdateOptions.PhoneNumber;

            await _accountDataStore.UpdateByIdAsync(
                accountData.Id,
                accountData.AccountNumber,
                accountData);
        }
    }
}
