using InfrastructureLayer.Interfaces;
using ModelsLayer.DataContext;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Repositories;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Proposal.Core
{
    public static class IocConfig
    {
        private readonly static Lazy<Container> _containerBulder =
            new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);
        public static IContainer Container
        {
            get { return _containerBulder.Value; }
        }

        public static void Execute()
        {
            //defaultContainer();
        }

        private static Container defaultContainer()
        {
            return new Container(ioc =>
            {
                ioc.For<IUnitOfWork>().Use<DataContext>();//.Singleton();
             
                ioc.Scan(x =>
                {

                    x.AssemblyContainingType(typeof(GenericRepository<,>));
                    x.AddAllTypesOf(typeof(GenericRepository<,>));
                    x.ConnectImplementationsToTypesClosing(typeof(IGenericRepository<,>));
                    x.WithDefaultConventions();
                });
               
            });
        }
    }

}