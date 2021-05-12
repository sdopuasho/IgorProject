using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hitcom_AccountingEquipment
{
    /// <summary>
    /// Логика взаимодействия для WaterMarkPswrdBx.xaml
    /// </summary>
    public partial class WaterMarkPswrdBx : UserControl
    {
            public WaterMarkPswrdBx()
            {
                this.InitializeComponent();
            CheckWatermark();
            }
            private void PasswordLostFocus(object sender, RoutedEventArgs e)
            {
                CheckWatermark();
            }

            public void CheckWatermark()
            {
                var passwordEmpty = string.IsNullOrEmpty(pb.Password);
                tbWatermark.Opacity = passwordEmpty ? 100 : 0;
                pb.Opacity = passwordEmpty ? 0 : 100;
            }

            private void PasswordGotFocus(object sender, RoutedEventArgs e)
            {
                tbWatermark.Opacity = 0;
                pb.Opacity = 100;
            }

            private void Pb_OnPasswordChanged(object sender, RoutedEventArgs e)
            {
                Password = pb.Password;
            }

            public string Watermark
            {
                get { return (string)GetValue(WatermarkProperty); }
                set { SetValue(WatermarkProperty, value); }
            }

            public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(WaterMarkPswrdBx), new PropertyMetadata("", WatermarkChanged));

            private static void WatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var controll = (WaterMarkPswrdBx)d;
                var val = (string)e.NewValue;
                controll.tbWatermark.Text = val;
            }


            /// <summary>
            /// Password
            /// </summary>

            public string Password
            {
                get { return (string)GetValue(PasswordProperty); }
                set { SetValue(PasswordProperty, value); }
            }

            public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(WaterMarkPswrdBx), new PropertyMetadata("", PasswordChanged));

            private static void PasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var controll = (WaterMarkPswrdBx)d;
                var val = (string)e.NewValue;
                if (val == null)
                {
                    controll.pb.Password = "";
                    return;
                }
                controll.pb.Password = val;
            }


            /// <summary>
            /// TextSize
            /// </summary>

            public int TextSize
            {
                get { return (int)GetValue(TextSizeProperty); }
                set { SetValue(TextSizeProperty, value); }
            }

            public static readonly DependencyProperty TextSizeProperty = DependencyProperty.Register("TextSize", typeof(int), typeof(WaterMarkPswrdBx), new PropertyMetadata(0, TextSizeChanged));

            private static void TextSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var controll = (WaterMarkPswrdBx)d;
                var val = (int)e.NewValue;
                if (val < 10) { val = 10; }
                controll.pb.FontSize = val;
                controll.tbWatermark.FontSize = val;
            }
        }
    }