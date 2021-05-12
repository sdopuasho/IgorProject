using Syncfusion.Windows.Shared;
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
    /// Логика взаимодействия для LogWindow.xaml
    /// </summary>
    public partial class LogWindow : ChromelessWindow
    {
        SenderMail Class = new SenderMail();
        /// <summary>
        /// Блок инициализации данных
        /// </summary>
        public LogWindow()
        {
            Syncfusion.SfSkinManager.SfSkinManager.ApplyStylesOnApplication = true;
            InitializeComponent();
            FrameManager.LogFrame = FrameLogin;
            FrameManager.LogFrame.Navigate(new PageFolder.LogPage());

        }
        /// <summary>
        /// Блок перехода по гиперссылке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
           
            System.Diagnostics.Process.Start("https://hitcom.pro/");
            
        }
        /// <summary>
        /// Блок отработки если сайт оффлайн
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private void Application_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (e.Exception is System.Net.WebException)
            {
                MessageBox.Show("Сайт " + e.Uri.ToString() + " не доступен :(");
                e.Handled = true;
                return;
            }
        }
    }
}
