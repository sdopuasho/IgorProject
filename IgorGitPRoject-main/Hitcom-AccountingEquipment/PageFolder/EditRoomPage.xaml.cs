using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// <summary>
    /// Логика взаимодействия для EditRoomPage.xaml
    /// </summary>
    public partial class EditRoomPage : Page
    {
        Room _CurrentRoom = new Room();
        /// <summary>
        /// Передача и создание элемента EquipmentCard 
        /// Для DataContext, а так же задание ItemSource для Combobox
        /// </summary>
        public EditRoomPage(Room SelectedRoom)
        {
            InitializeComponent(); 
            if (SelectedRoom != null)
                _CurrentRoom = SelectedRoom;
            DataContext = _CurrentRoom;

            ComboTypeRoom.ItemsSource = AccountingEquipmentEntities.GetContext().TypeOfRoom.ToList();
        }
        /// <summary>
        /// Блок проверки введенных данных
        /// В случае если что-то будет неуказанно
        /// Будет добавленна строка с этим элементом, после чего под нужным элементом появится сообщение
        /// </summary>
        public bool CheckDataContext()
        {
            StringBuilder errors = new StringBuilder();
            if (ComboTypeRoom.SelectedItem == null)
                errors.AppendLine("Тип");
            if (string.IsNullOrEmpty(_CurrentRoom.NameOfRoom))
                errors.AppendLine("Номер");
            if (errors.Length > 0)
            {
                if (errors.ToString().Contains("Тип") == true)
                {
                    TypeRoomFail.Visibility = Visibility.Visible;
                    TypeRoomFail.Content = "Укажите тип помещения";
                }
                else
                {
                    TypeRoomFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Номер") == true)
                {
                    RoomFail.Visibility = Visibility.Visible;
                    RoomFail.Content = "Введите номер помещения";
                }
                else
                {
                    RoomFail.Visibility = Visibility.Collapsed;
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// Обработка собыитя нажатия кнопки сохранить
        /// После нажатия проверяется все ли данные введены
        /// В случае если нет, то редактирование или сохранение отменится
        /// В противном случае данные будут внесены в бд
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckDataContext() == true)
            {
                if (_CurrentRoom.id == 0)
                    AccountingEquipmentEntities.GetContext().Room.Add(_CurrentRoom);

                try
                {
                    AccountingEquipmentEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    OperationHystory OHistory = new OperationHystory() { FK_Worker_id = SenderMail.IntId, Operation = "Добавление в таблицу помещения", DateTimeOfOperation = DateTime.Now };
                    AccountingEquipmentEntities.GetContext().OperationHystory.Add(OHistory);
                    AccountingEquipmentEntities.GetContext().SaveChanges();
                    FrameManager.MainFrame.GoBack();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// Проверка на вводимые символы в Textbox
        /// В случае если символ введен неверный, то запись в TextBox не произойдет
        /// </summary>

        private void SpaceTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
