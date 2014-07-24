using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using DatabaseManager.Domain.Abstract;
using DatabaseManager.Domain.Entities;
using Moq;

namespace DatabaseManager.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            Mock<IDatabaseRepository> mock = new Mock<IDatabaseRepository>();
            mock.Setup(m => m.Databases).Returns(new List<Database> {
                new Database { Name = "LawsonAdmin", PIName = "David Hill", Developer = "John Soer"},
                new Database { Name = "PREP", PIName = "Rupinder Mann", Developer = "John Soer"},
                new Database { Name = "MHEN", PIName = "Cheryl Forchuk", Developer = "John Soer"}
            });

            kernel.Bind<IDatabaseRepository>().ToConstant(mock.Object);
        }
    }
}