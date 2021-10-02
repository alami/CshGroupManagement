using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;

namespace CodingMilitia.PlayBall.GroupManagement.Business.Impl.Services
{
    public class InMemoryGroupsService : IGroupsService
    {
        private readonly List<Group> _groups = new List<Group>();
        private long _currentId = 0;
        public Task<IReadOnlyCollection<Group>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyCollection<Group>>(_groups.AsReadOnly());
        }

        public Task<Group> GetByIdAsync(long id)
        {
            return Task.FromResult(_groups.SingleOrDefault(g => g.Id == id));
        }

        public Task<Group> UpdateAsync(Group group)
        {
            var toUpdate = _groups.SingleOrDefault(g => g.Id == group.Id);
            if (toUpdate == null)
            {
                return null;
            }

            toUpdate.Name = group.Name;
            return Task.FromResult(toUpdate);
        }

        public Task<Group> AddAsync(Group group)
        {
            group.Id = ++_currentId;
            _groups.Add(group);
            return Task.FromResult(group);
        }
    }
}