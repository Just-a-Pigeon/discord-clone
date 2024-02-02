using signalRchat.Core.Entities;
using signalRchat.Core.Repositories.Interfaces;
using signalRchat.Core.Services.Interfaces;

namespace signalRchat.Core.Services;

public class AccountService : IAccountService
{
    protected readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }


    public Task<ICollection<ApplicationUser>> GetAllUsers()
    {
        var users = _accountRepository.GetAll();
        return users;
    }

    public Task<ApplicationUser> GetUserById(Guid id)
    {
        var user = _accountRepository.GetByIdAsync(id);
        return user;
    }

    public Task<ApplicationUser> AddUser(ApplicationUser user)
    {
        var addedUser = _accountRepository.AddAsync(user);
        return addedUser;
    }

    public Task<ApplicationUser> UpdateUser(ApplicationUser user)
    {
        var updatedUser = _accountRepository.UpdateAsync(user);
        return updatedUser;
    }

    public Task<ApplicationUser> DeleteUser(ApplicationUser user)
    {
        var deletedUser = _accountRepository.DeleteAysnc(user);
        return deletedUser;
    }
}