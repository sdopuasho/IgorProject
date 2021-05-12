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

namespace Hitcom_AccountingEquipment.PageFolder
{
    public partial class AutherizationPage : Page
    {
        public string CodeResult; 
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        SenderMail page = new SenderMail();
        /// <summary>
        /// Страница получает созданный код и начинает отсчет таймера для создания нового кода
        /// </summary>
        public AutherizationPage(string code)
        {
            InitializeComponent();
            CodeResult = code;
            timer.Tick += dispatcherTimer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timer.Start();
            lbltxt.MouseEnter -= lbltxt_MouseEnter;
        }
        /// <summary>
        /// работа таймера
        /// в случае если таймер дойдет до 1 секунды, он предложит выслать новый пароль на почту
        /// </summary>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            string a = (string)lbl.Content;
            if (a == "1")
            {
                lbl.Content = "";
                lbltxt.Content = "Выслать повторно";
                lbltxt.MouseEnter += lbltxt_MouseEnter;
                timer.Tick -= dispatcherTimer_Tick;

            }
            else
            {
                int time = Convert.ToInt32(lbl.Content);
                lbl.Content = Convert.ToString(time - 1);
            }
        }
        /// <summary>
        /// Обработка нажатие на кнопку
        /// Если полвведеный код окажется верным, то откроется MainWindow, а в противном выведет сообщение о неверном коде
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Auten.Text != CodeResult)
            {
                MessageBox.Show("Введен неверный код","Внимание",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }
            else
            {
                LogWindow win = new LogWindow();
                OperationHystory OHistory = new OperationHystory() { FK_Worker_id = SenderMail.IntId, Operation = "Авторизация", DateTimeOfOperation = DateTime.Now };
                AccountingEquipmentEntities.GetContext().OperationHystory.Add(OHistory);
                AccountingEquipmentEntities.GetContext().SaveChanges();
                MainWindow win2 = new MainWindow();
                win2.ShowDialog() ;
            }
        }

        private void lbltxt_MouseEnter(object sender, MouseEventArgs e)
        {
            CodeResult = page.SenderCode();
            page.senderMAil("", CodeResult);
        }
    }
}
