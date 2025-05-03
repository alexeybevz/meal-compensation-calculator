using System;
using System.Windows;
using System.Windows.Input;

namespace MealCompensationCalculator.WPF.Behaviors
{
    public static class WindowClosingBehavior
    {
        public static readonly DependencyProperty ClosedProperty =
            DependencyProperty.RegisterAttached(
                "Closed", typeof(ICommand), typeof(WindowClosingBehavior),
                new PropertyMetadata(null, OnClosedChanged));

        public static void SetClosed(Window window, ICommand value) => window.SetValue(ClosedProperty, value);
        public static ICommand GetClosed(Window window) => (ICommand)window.GetValue(ClosedProperty);

        private static void OnClosedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                window.Closed -= Window_Closed;
                if (e.NewValue != null)
                    window.Closed += Window_Closed;
            }
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            var window = sender as Window;
            var command = GetClosed(window);
            command?.Execute(null);
        }
    }
}