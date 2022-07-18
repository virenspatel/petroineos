using Autofac;
using Petroineos.Helpers;
using Petroineos.Interfaces;
using Petroineos.Services;
using Services;

namespace Petroineos
{
    public static class Bootstrapper
    {
        public static Autofac.IContainer AppContainer { get; set; }
        public static Autofac.IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<IOService>().As<IIOService>().InstancePerLifetimeScope();
            builder.RegisterType<CSVFileService>().As<ICSVFileService>().SingleInstance();
            builder.RegisterType<ReportCreatorService>().As<IReportCreatorService>().InstancePerLifetimeScope();
            builder.RegisterType<PowerService>().As<IPowerService>().InstancePerLifetimeScope();
            builder.RegisterType<ConfigReaderService>().As<IConfigReaderService>().SingleInstance();
            builder.RegisterType<TradePositionFinderService>().As<ITradePositionFinderService>().InstancePerLifetimeScope();

            builder.RegisterType<SchedulerService>().As<SchedulerService>().SingleInstance();
            builder.RegisterModule(new LogInjectionModule());

            var container = builder.Build();

            AppContainer = container;
            return container;
        }
    }
}
