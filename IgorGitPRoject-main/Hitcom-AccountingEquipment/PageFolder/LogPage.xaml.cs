using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Hitcom_AccountingEquipment.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для LogPage.xaml
    /// </summary>
    public partial class LogPage : Page
    {
        SenderMail Class = new SenderMail();
        AuthorizationData AUData = new AuthorizationData();
        /// <summary>
        ///Блок инициализации данных
        /// </summary>
        public LogPage()
        {
            InitializeComponent();
        }
        /// <summary>
        ///Блок проверки введеных данных и сопостовление его с бд
        /// </summary>

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var idCheck = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.Login.Equals(LoginTextBX.Text) && w.Password.Equals(PasswordTextBX.Password)).Select(s => s.id).FirstOrDefault();
            var idChecklogin = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.Login.Equals(LoginTextBX.Text)).Select(s => s.id).FirstOrDefault();
            if (AUData.LoginEnteringTextCheck(LoginTextBX.Text) == false || AUData.PasswordEnteringTextCheck(PasswordTextBX.Password) == false)
            {

                Fail1.Visibility = Visibility.Visible;
                Fail1.Content = "Введите логин";
                Fail2.Visibility = Visibility.Visible;
                Fail2.Content = "Введите пароль";
            }
            else if (AUData.LoginEnteringTextCheck(LoginTextBX.Text) == false)
            {

                Fail1.Visibility = Visibility.Visible;
                Fail1.Content = "Введите логин";
            }
            else if (AUData.PasswordEnteringTextCheck(PasswordTextBX.Password) == false)
            {

                Fail2.Content = "Введите пароль";
                Fail2.Visibility = Visibility.Visible;
            }
            else
            {
                if (idChecklogin == 0)
                {
                    GlobarFail.Visibility = Visibility.Visible;
                    GlobarFail.HorizontalContentAlignment = HorizontalAlignment.Center;
                    GlobarFail.Content = "Пользователя с такими данными не существует!";
                }
                else
                {
                    if (idCheck == 0)
                    {
                        GlobarFail.Content = "Пользователя с такими данными не существует!";
                        GlobarFail.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        string Code = Class.SenderCode();
                        Class.senderMAil(AccountingEquipmentEntities.GetContext().Worker.Where(w=>w.id == idCheck).Select(s=>s.EmailOfWorker).FirstOrDefault(), Code);
                        SenderMail.IntId = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == idCheck).Select(s => s.id).FirstOrDefault();
                        SenderMail.PositionName = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == idCheck).Select(s => s.Position.PostionName).FirstOrDefault();
                        FrameManager.LogFrame.Navigate(new AutherizationPage(Code));
                    }
                }

            }
        }

        
    }
}
