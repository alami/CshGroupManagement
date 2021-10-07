using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Business.Impl.Mappings;
using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using CodingMilitia.PlayBall.GroupManagement.Data;
using CodingMilitia.PlayBall.GroupManagement.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CodingMilitia.PlayBall.GroupManagement.Business.Impl.Services
{
    public class GroupService: IGroupsService
    {
        private readonly GroupManagementDbContext _context;

        public GroupService(GroupManagementDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyCollection<Group>> GetAllAsync(CancellationToken ct)
        {
            var groups = await _context.Groups.OrderBy(g=>g.Id).ToListAsync(ct);
            return groups.ToService();
        }

        public async Task<Group> GetByIdAsync(long id, CancellationToken ct)
        {
            //var group = await _context.Groups.FindAsync(new object[] {id},ct);  //какой лучше?
            var group = await _context.Groups.SingleOrDefaultAsync(g=> g.Id==id,ct);
            return group.ToService();
        }

        public async Task<Group> UpdateAsync(Group group, CancellationToken ct)
        {
            // throw new DbUpdateException();
            var updateGroupEntry = _context.Groups.Update(group.ToEntity());
            await _context.SaveChangesAsync(ct);
            return updateGroupEntry.Entity.ToService();
        }
        
        public async Task<Group> AddAsync(Group group, CancellationToken ct)
        {
            var addedGroupEntry = _context.Groups.Add(group.ToEntity());
            await _context.SaveChangesAsync(ct);
            return addedGroupEntry.Entity.ToService();
        }

        public async Task RemoveAsync(long id, CancellationToken ct)
        {
            var entityToRemove = await _context.Groups.SingleOrDefaultAsync(g=>g.Id==id, ct);
            _context.Groups.Remove(entityToRemove);
            await _context.SaveChangesAsync(ct);
         }
    }
}