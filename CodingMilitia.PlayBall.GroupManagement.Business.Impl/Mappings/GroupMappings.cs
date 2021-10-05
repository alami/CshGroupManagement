using System.Collections.Generic;
using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Data.Configurations;
using CodingMilitia.PlayBall.GroupManagement.Data.Entities;

namespace CodingMilitia.PlayBall.GroupManagement.Business.Impl.Mappings
{
    public static class GroupMappings
    {
        public static Group ToService(this GroupEntity entity)
        {
            return entity != null ? new Group{ Id = entity.Id, Name = entity.Name } : null;
        }
        public static GroupEntity ToEntity(this Group model)
        {
            return model != null ? new GroupEntity{ Id = model.Id, Name = model.Name } : null;
        }

        public static IReadOnlyCollection<Group> ToService(this IReadOnlyCollection<GroupEntity> entities) =>
            entities.MapCollection(ToService);
    }
}