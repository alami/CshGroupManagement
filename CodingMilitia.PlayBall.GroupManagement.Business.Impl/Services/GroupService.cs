using System.Collections.Generic;
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
            return await SimplestUpdateAsync(group, ct); 
        }

        private async Task<Group> SimplestUpdateAsync(Group group, CancellationToken ct)
        {
            var updateGroupEntry = _context.Groups.Update(group.ToEntity());
            await _context.SaveChangesAsync(ct);
            return updateGroupEntry.Entity.ToService();
        } 
        private async Task<Group> UpdateWithFetchAsync(Group group, CancellationToken ct)
        {
            var existingGroup = await _context.Groups.SingleOrDefaultAsync(g=>g.Id==group.Id,ct);
            existingGroup.Name = group.Name;
            await _context.SaveChangesAsync(ct);
            return existingGroup. ToService();
        } 
        private async Task<Group> UpdateWithFetch2Async(Group group, CancellationToken ct)
        {
            var existingGroup = await _context.Groups.SingleOrDefaultAsync(g=>g.Id==group.Id,ct);
            _context.Entry(existingGroup).CurrentValues.SetValues(group.ToEntity());
            await _context.SaveChangesAsync(ct);
            return existingGroup. ToService();
        } 

        public async Task<Group> AddAsync(Group group, CancellationToken ct)
        {
            var addedGroupEntry = _context.Groups.Add(group.ToEntity());
            await _context.SaveChangesAsync(ct);
            return addedGroupEntry.Entity.ToService();
        }
    }
}