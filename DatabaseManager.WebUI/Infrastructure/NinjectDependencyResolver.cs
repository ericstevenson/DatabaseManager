using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using DatabaseManager.Domain.Abstract;
using DatabaseManager.Domain.Entities;
using Moq;
using DatabaseManager.Domain.Concrete;
using DatabaseManager.WebUI.Infrastructure.Abstract;
using DatabaseManager.WebUI.Infrastructure.Concrete;

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
            kernel.Bind <ILawsonDatabaseRepository>().To<EFLawsonDatabaseRepository>();
            kernel.Bind<IUserRepository>().To<EFUserRepository>();
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
            kernel.Bind<IEmailProvider>().To<SimpleEmailSender>();
            kernel.Bind<IEmailRepository>().To<EFEmailRepository>();
            kernel.Bind<IFinanceRepository>().To<EFFinanceRepository>();
            kernel.Bind<IPaymentProvider>().To<PaymentCalculator>();
        }
    }
}