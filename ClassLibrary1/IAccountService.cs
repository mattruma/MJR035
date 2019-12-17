using System.Threading.Tasks;

namespace ClassLibrary1
{
    public interface IAccountService
    {
        Task AddAsync(
            AccountAddOptions accountAddOptions);
    }
}
