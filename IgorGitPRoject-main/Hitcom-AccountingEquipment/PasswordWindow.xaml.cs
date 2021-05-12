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
using System.Windows.Shapes;

namespace Hitcom_AccountingEquipment
{
    /// <summary>
    /// Логика взаимодействия для PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : ChromelessWindow
    {
        /// <summary>
        /// Блок инициализации данных
        /// </summary>
        public PasswordWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Блок замены стандартного пароля компании для нового пользователя
        /// При нажатии будет сверен пароль в первом и втором Textbox 
        /// После чего будет добавлен как пароль учетной записи работника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var WorkerFPs = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == SenderMail.IntId).FirstOrDefault();
            if(PasswordTextBX.Password != PasswordTextBX2.Password)
            {
                MessageBox.Show("Введеные пароли не совпадают", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                WorkerFPs.Password = PasswordTextBX.Password;
                WorkerFPs.CheckFirstVisit = false;
                AccountingEquipmentEntities.GetContext().SaveChanges();
                this.Close();
            }
        }
    }
}
