using FluentAssertions;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace ClassLibrary1.Tests
{
    public class AccountEntityServiceTests : BaseTests
    {
        [Fact]
        public async Task When_AddAsync()
        {
            // Arrange

            var accountDataStore =
                new Mock<IAccountDataStore>();

            var accountEntityService =
                new AccountEntityService(
                    accountDataStore.Object);

            var accountEntityAddOptions =
                new AccountEntityAddOptions
                {
                    AccountNumber =
                        _faker.Random.Number(10000, 99999).ToString(),
                    SystemOfRecord =
                        _faker.PickRandom(
                            AccountData.SYSTEMOFRECORD_AUTOSUITE,
                            AccountData.SYSTEMOFRECORD_FISERVE,
                            AccountData.SYSTEMOFRECORD_ISERIES,
                            AccountData.SYSTEMOFRECORD_LEASEMASTER),
                    PhoneNumber =
                        _faker.Person.Phone,
                };

            // Act

            var accountEntity =
                await accountEntityService.AddAsync(
                    accountEntityAddOptions);

            // Assert

            accountDataStore.Verify(
                m => m.AddAsync(It.IsAny<AccountData>()));

            accountEntity.Should().NotBeNull();
            accountEntity.AccountNumber.Should().Be(accountEntityAddOptions.AccountNumber);
            accountEntity.PhoneNumber.Should().Be(accountEntityAddOptions.PhoneNumber);
            accountEntity.SystemOfRecord.Should().Be(accountEntityAddOptions.SystemOfRecord);
        }

        [Fact]
        public async Task When_DeleteByIdAsync()
        {
            // Arrange

            var accountDataStore =
                new Mock<IAccountDataStore>();

            var accountEntityService =
                new AccountEntityService(
                    accountDataStore.Object);

            var id =
                _faker.Random.Guid();
            var accountNumber =
                _faker.Random.Number(10000, 99999).ToString();

            // Act

            await accountEntityService.DeleteByIdAsync(
                id,
                accountNumber);

            // Assert

            accountDataStore.Verify(
                m => m.DeleteByIdAsync(id, accountNumber));
        }

        [Fact]
        [SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        public async Task When_FetchByAccountNumberAsync()
        {
            // Arrange

            var accountData =
                new AccountData(
                    _faker.Random.Number(10000, 99999).ToString());

            accountData.SystemOfRecord =
                _faker.PickRandom(
                    AccountData.SYSTEMOFRECORD_AUTOSUITE,
                    AccountData.SYSTEMOFRECORD_FISERVE,
                    AccountData.SYSTEMOFRECORD_ISERIES,
                    AccountData.SYSTEMOFRECORD_LEASEMASTER);
            accountData.PhoneNumber =
                _faker.Person.Phone;

            var accountDataStore =
                new Mock<IAccountDataStore>();

            accountDataStore
                .Setup(x => x.FetchByAccountNumberAsync(accountData.AccountNumber))
                .ReturnsAsync(accountData);

            var accountEntityService =
                new AccountEntityService(
                    accountDataStore.Object);

            // Act

            var accountEntity =
                await accountEntityService.FetchByAccountNumberAsync(
                    accountData.AccountNumber);

            // Assert

            accountDataStore.Verify(
                m => m.FetchByAccountNumberAsync(accountData.AccountNumber));

            accountEntity.Should().NotBeNull();
        }

        [Fact]
        [SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        public async Task When_FetchByIdAsync()
        {
            // Arrange

            var accountData =
                new AccountData(
                    _faker.Random.Number(10000, 99999).ToString());

            accountData.SystemOfRecord =
                _faker.PickRandom(
                    AccountData.SYSTEMOFRECORD_AUTOSUITE,
                    AccountData.SYSTEMOFRECORD_FISERVE,
                    AccountData.SYSTEMOFRECORD_ISERIES,
                    AccountData.SYSTEMOFRECORD_LEASEMASTER);
            accountData.PhoneNumber =
                _faker.Person.Phone;

            var accountDataStore =
                new Mock<IAccountDataStore>();

            accountDataStore
                .Setup(x => x.FetchByIdAsync(accountData.Id, accountData.AccountNumber))
                .ReturnsAsync(accountData);

            var accountEntityService =
                new AccountEntityService(
                    accountDataStore.Object);

            // Act

            var accountEntity =
                await accountEntityService.FetchByIdAsync(
                    accountData.Id,
                    accountData.AccountNumber);

            // Assert

            accountDataStore.Verify(
                m => m.FetchByIdAsync(accountData.Id, accountData.AccountNumber));

            accountEntity.Should().NotBeNull();
        }

        [Fact]
        [SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        public async Task When_UpdateByIdAsync()
        {
            // Arrange

            var accountData =
                new AccountData(
                    _faker.Random.Number(10000, 99999).ToString());

            accountData.SystemOfRecord =
                _faker.PickRandom(
                    AccountData.SYSTEMOFRECORD_AUTOSUITE,
                    AccountData.SYSTEMOFRECORD_FISERVE,
                    AccountData.SYSTEMOFRECORD_ISERIES,
                    AccountData.SYSTEMOFRECORD_LEASEMASTER);
            accountData.PhoneNumber =
                _faker.Person.Phone;

            var accountDataStore =
                new Mock<IAccountDataStore>();

            accountDataStore
                .Setup(x => x.FetchByIdAsync(accountData.Id, accountData.AccountNumber))
                .ReturnsAsync(accountData);
            accountDataStore
                .Setup(x => x.UpdateByIdAsync(accountData.Id, accountData.AccountNumber, accountData));

            var accountEntityService =
                new AccountEntityService(
                    accountDataStore.Object);

            var accountEntityUpdateOptions =
                new AccountEntityUpdateOptions
                {
                    SystemOfRecord =
                        _faker.PickRandom(
                            AccountData.SYSTEMOFRECORD_AUTOSUITE,
                            AccountData.SYSTEMOFRECORD_FISERVE,
                            AccountData.SYSTEMOFRECORD_ISERIES,
                            AccountData.SYSTEMOFRECORD_LEASEMASTER),
                    PhoneNumber =
                        _faker.Person.Phone,
                };

            // Act

            await accountEntityService.UpdateByIdAsync(
                accountData.Id,
                accountData.AccountNumber,
                accountEntityUpdateOptions);

            // Assert

            accountDataStore.Verify(
                m => m.UpdateByIdAsync(accountData.Id, accountData.AccountNumber, It.IsAny<AccountData>()));
        }
    }
}
