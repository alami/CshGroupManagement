using System;
using System.Collections.Generic;
using Autofac;
using CodingMilitia.PlayBall.GroupManagement.Business.Impl.Services;
using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using Microsoft.Extensions.Logging;

namespace CodingMilitia.PlayBall.GroupManagement.Web.IoC
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryGroupsService>().Named<IGroupsService>("groupService").SingleInstance();
            builder.RegisterDecorator<IGroupsService>((context, service)
                =>new GroupsServiceDecorator(service,context.Resolve< ILogger<GroupsServiceDecorator>>()),"groupService");
        }
        private class GroupsServiceDecorator : IGroupsService
        {
            private readonly IGroupsService _inner; 
            private readonly ILogger<GroupsServiceDecorator> _logger;
            public GroupsServiceDecorator(IGroupsService inner, ILogger<GroupsServiceDecorator> logger)
            {
                _inner = inner;
                _logger = logger;
            }
            public IReadOnlyCollection<Group> GetAll()
            {
                _logger.LogTrace("######## Helooo from {decoratedMethod} #########", nameof(GetAll));
                return _inner.GetAll() ;
            }

            public Group GetById(long id)
            {
                _logger.LogWarning($"######## Helooo from {nameof(GetById)} #########");
                return _inner.GetById(id) ;
            }

            public Group Update(Group group)
            {
                _logger.LogWarning($"######## Helooo from {nameof(Update)} #########");
                return _inner.Update(group) ;
            }

            public Group Add(Group group)
            {
                _logger.LogWarning($"######## Helooo from {nameof(Add)} #########");
                return _inner.Add(group) ;
             }
        }
    }
}