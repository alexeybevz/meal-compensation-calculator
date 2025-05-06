using System.Diagnostics;
using System.Threading.Tasks;
using MealCompensationCalculator.WPF.ViewModels;

namespace MealCompensationCalculator.WPF.Commands
{
    public class OpenReportLocationCommand : AsyncCommandBase
    {
        private readonly ReportLocationViewModel _reportLocationViewModel;

        public OpenReportLocationCommand(ReportLocationViewModel reportLocationViewModel)
        {
            _reportLocationViewModel = reportLocationViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(_reportLocationViewModel.PathToReport))
                    Process.Start("explorer.exe", _reportLocationViewModel.PathToReport);
            });
        }
    }
}