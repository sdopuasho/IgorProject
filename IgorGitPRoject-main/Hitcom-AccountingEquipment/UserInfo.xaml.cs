using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для UserInfo.xaml
    /// </summary>
    public partial class UserInfo : UserControl
    {
        public string FIOW { get; set; }
        public string PositionW { get; set; }
        public BitmapImage ImageByte { get; set; }
        public UserInfo()
        {
            InitializeComponent();
            DataContext = this;
        }
        

        private void Label_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Close();
            LogWindow pg = new LogWindow();
            pg.ShowDialog();
        }
    }
}
