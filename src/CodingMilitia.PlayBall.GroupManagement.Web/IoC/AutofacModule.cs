using System;
using System.Collections.Generic;
using Autofac;
using CodingMilitia.PlayBall.GroupManagement.Business.Impl.Services;
using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryGroupsService>().Named<IGroupsService>("groupService").SingleInstance();
            builder.RegisterDecorator<IGroupsService>((context, service)
                =>new GroupsServiceDecorator(service),"groupService");
        }
        private class GroupsServiceDecorator : IGroupsService
        {
            private readonly IGroupsService _inner;

            public GroupsServiceDecorator(IGroupsService inner)
            {
                _inner = inner;
            }
            public IReadOnlyCollection<Group> GetAll()
            {
                Console.WriteLine($"######## Helooo from {nameof(GetAll)} #########");
                return _inner.GetAll() ;
            }

            public Group GetById(long id)
            {
                Console.WriteLine($"######## Helooo from {nameof(GetById)} #########");
                return _inner.GetById(id) ;
            }

            public Group Update(Group group)
            {
                Console.WriteLine($"######## Helooo from {nameof(Update)} #########");
                return _inner.Update(group) ;
            }

            public Group Add(Group group)
            {
                Console.WriteLine($"######## Helooo from {nameof(Add)} #########");
                return _inner.Add(group) ;
             }
        }
    }
}