using signalRchat.Core.Entities;

namespace signalRchat.Core.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<ICollection<ApplicationUser>> GetAll();
    Task<ApplicationUser> GetByIdAsync(Guid id);
    
    Task<ApplicationUser> AddAsync(ApplicationUser user);
    Task<ApplicationUser> UpdateAsync(ApplicationUser user);
    Task<ApplicationUser> DeleteAysnc(ApplicationUser user);
}