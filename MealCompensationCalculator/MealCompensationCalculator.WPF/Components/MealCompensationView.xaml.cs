using System.Windows;
using System.Windows.Controls;

namespace MealCompensationCalculator.WPF.Components
{
    /// <summary>
    /// Логика взаимодействия для MealCompensationView.xaml
    /// </summary>
    public partial class MealCompensationView : UserControl
    {
        public MealCompensationView()
        {
            InitializeComponent();
        }

        private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox) sender;
            tb.SelectAll();
        }
    }
}
