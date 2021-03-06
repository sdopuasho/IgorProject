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
    /// <summary>
    /// Логика взаимодействия для RoomPage.xaml
    /// </summary>
    public partial class RoomPage : Page
    {
        Room _CurrentRoom = new Room();
        /// <summary>
        ///Блок инициализации данных
        /// </summary>
        public RoomPage()
        {
            InitializeComponent();
            DataContext = _CurrentRoom;
            DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Room.ToList();
        }
        /// <summary>
        /// Блоки перехода для добавление или редактирования данных
        /// в случае редактирования будут переданны данные выбраного элемента
        /// </summary>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EditRoomPage((sender as Button).DataContext as Room));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EditRoomPage(null));
        }
        /// <summary>
        /// Блок перезагрузки сущености в случае изменения данных
        /// Срабатывает после появления страницы пользователю
        /// </summary>

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                AccountingEquipmentEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Room.ToList();
            }
        }

        /// <summary>
        /// Блок проверки разрешения на удаление
        /// </summary>
        public bool AccessToDelBtn(int RowToDel)
        {
            if(RowToDel > 0 && SenderMail.PositionName.Contains("Администратор"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки удаление данных
        /// в случае если проверка на удаление пройдет
        /// то программа уточнит хочет ли пользователь удалить эти данные
        /// </summary>
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var EquipmentForRemoving = DgridMyPage.SelectedItems.Cast<Room>().ToList();
            if (AccessToDelBtn(EquipmentForRemoving.Count) == true)
            {
            if (MessageBox.Show($"Вы точно хотите удалить следующие {EquipmentForRemoving.Count} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AccountingEquipmentEntities.GetContext().Room.RemoveRange(EquipmentForRemoving);
                    AccountingEquipmentEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены");
                    OperationHystory OHistory = new OperationHystory() { FK_Worker_id = SenderMail.IntId, Operation = "Удаление из таблицы помещения", DateTimeOfOperation = DateTime.Now };
                    AccountingEquipmentEntities.GetContext().OperationHystory.Add(OHistory);
                    AccountingEquipmentEntities.GetContext().SaveChanges();
                    DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Room.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());

                    }

                }
            }
            else
            {
                return;
            }
        }
    }
}
