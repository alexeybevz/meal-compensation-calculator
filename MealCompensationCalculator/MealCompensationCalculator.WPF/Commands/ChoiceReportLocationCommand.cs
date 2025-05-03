using System.Windows.Forms;
using MealCompensationCalculator.WPF.ViewModels;

namespace MealCompensationCalculator.WPF.Commands
{
    public class ChoiceReportLocationCommand : CommandBase
    {
        private readonly ReportLocationViewModel _reportLocationViewModel;

        public ChoiceReportLocationCommand(ReportLocationViewModel reportLocationViewModel)
        {
            _reportLocationViewModel = reportLocationViewModel;
        }

        public override void Execute(object parameter)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = "C:\\";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _reportLocationViewModel.PathToReport = dialog.SelectedPath;
                }
            }
        }
    }
}