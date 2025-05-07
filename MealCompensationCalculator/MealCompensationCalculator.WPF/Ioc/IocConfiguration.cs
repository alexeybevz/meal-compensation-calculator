using MealCompensationCalculator.BusinessLogic.Commands;
using MealCompensationCalculator.BusinessLogic.Queries;
using MealCompensationCalculator.Domain.Commands;
using MealCompensationCalculator.Domain.Queries;
using MealCompensationCalculator.WPF.Stores;
using MealCompensationCalculator.WPF.ViewModels;
using Ninject.Modules;

namespace MealCompensationCalculator.WPF.Ioc
{
    public class IocConfiguration : NinjectModule
    {
        public override void Load()
        {
            Bind<IGetConfigQuery>().To<GetConfigQuery>().InTransientScope();
            Bind<ISaveConfigCommand>().To<SaveConfigCommand>().InTransientScope();
            Bind<ConfigStore>().ToSelf().InTransientScope();

            Bind<MealCompensationCalculatorViewModel>().ToMethod(ctx => MealCompensationCalculatorViewModel.LoadViewModel(IocKernel.Get<ConfigStore>())).InTransientScope();
            Bind<MainViewModel>().ToSelf().InTransientScope();

            Bind<MainWindow>().ToMethod(ctx => new MainWindow() { DataContext = IocKernel.Get<MainViewModel>() }).InSingletonScope();
        }
    }
}