using System.Collections.Generic;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Business.Models;

namespace CodingMilitia.PlayBall.GroupManagement.Business.Services
{
    public interface IGroupsService
    {
        Task<IReadOnlyCollection<Group>> GetAllAsync();
        Task<Group> GetByIdAsync(long id);
        Task<Group> UpdateAsync(Group group);
        Task<Group> AddAsync (Group group);
    }
}