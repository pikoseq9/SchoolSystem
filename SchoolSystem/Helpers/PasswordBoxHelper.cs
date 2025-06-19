using System.Windows;
using System.Windows.Controls;

namespace SchoolSystem.Helpers
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPassword =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxHelper),
                new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject d) =>
            (string)d.GetValue(BoundPassword);

        public static void SetBoundPassword(DependencyObject d, string value) =>
            d.SetValue(BoundPassword, value);

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox box)
            {
                box.PasswordChanged -= PasswordBox_PasswordChanged;
                if (!(bool)box.GetValue(UpdatingPassword))
                    box.Password = (string)e.NewValue;
                box.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static readonly DependencyProperty UpdatingPassword =
            DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false));

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox box)
            {
                box.SetValue(UpdatingPassword, true);
                SetBoundPassword(box, box.Password);
                box.SetValue(UpdatingPassword, false);
            }
        }
    }
}
