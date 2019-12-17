using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class AccountService : IAccountService
    {
        private IAccountDataStore _accountDataStore;

        public AccountService(
            IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        public async Task AddAsync(
            AccountAddOptions accountAddOptions)
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
        }
    }
}
