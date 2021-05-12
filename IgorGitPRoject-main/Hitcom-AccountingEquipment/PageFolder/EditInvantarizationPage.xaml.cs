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
    /// Логика взаимодействия для EditInvantarizationPage.xaml
    /// </summary>
    public partial class EditInvantarizationPage : Page
    {
        Inventory _CurrentInventory = new Inventory() {InventoryDate = DateTime.Now };
        /// <summary>
        /// Передача и создание элемента EquipmentCard 
        /// Для DataContext, а так же задание ItemSource для Combobox
        /// </summary>
        public EditInvantarizationPage(Inventory SelectedInventory)
        {
            InitializeComponent();
            if (SelectedInventory != null)
                _CurrentInventory = SelectedInventory;
            DataContext = _CurrentInventory;
            ComboInvent.ItemsSource = AccountingEquipmentEntities.GetContext().EquipmentCard.ToList();
            ComboManufacturerName.ItemsSource = AccountingEquipmentEntities.GetContext().Manufacturer.ToList();
            ComboModel.ItemsSource = AccountingEquipmentEntities.GetContext().Equipment.ToList();
            ComboNameOfNomenclature.ItemsSource = AccountingEquipmentEntities.GetContext().Nomenclature.ToList();
            ComboRoom.ItemsSource = AccountingEquipmentEntities.GetContext().Room.ToList();
            ComboStatus.ItemsSource = AccountingEquipmentEntities.GetContext().StatusOfInventory.ToList();
            ComboFname.ItemsSource = AccountingEquipmentEntities.GetContext().Worker.ToList();
        }
        /// <summary>
        /// Блок проверки введенных данных
        /// В случае если что-то будет неуказанно
        /// Будет добавленна строка с этим элементом, после чего под нужным элементом появится сообщение
        /// </summary>
        public bool CheckDataContext()
        {
            StringBuilder errors = new StringBuilder();
            if (ComboNameOfNomenclature.SelectedItem == null)
                errors.AppendLine("Номенклатура");
            if (ComboInvent.SelectedItem == null)
                errors.AppendLine("Инвентаризация");
            if (ComboModel.SelectedItem == null)
                errors.AppendLine("Модель");
            if (ComboRoom.SelectedItem == null)
                errors.AppendLine("Помещение");
            if (ComboStatus.SelectedItem == null)
                errors.AppendLine("Статус");
            if (ComboFname.SelectedItem == null)
                errors.AppendLine("Работник");
            if (ComboManufacturerName.SelectedItem == null)
                errors.AppendLine("Производитель");
            if (errors.Length > 0)
            {
                if (errors.ToString().Contains("Номенклатура") == true)
                {
                    NomenclatureFail.Visibility = Visibility.Visible;
                    NomenclatureFail.Content = "Укажите номенклатуру";
                }
                else
                {
                    NomenclatureFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Инвентаризация") == true)
                {
                    InventNumberFail.Visibility = Visibility.Visible;
                    InventNumberFail.Content = "Выберите инвентаризацию";
                }
                else
                {
                    InventNumberFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Модель") == true)
                {
                    ModelFail.Visibility = Visibility.Visible;
                    ModelFail.Content = "Выберите модель";
                }
                else
                {
                    ModelFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Помещение") == true)
                {
                    RoomFail.Visibility = Visibility.Visible;
                    RoomFail.Content = "Укажите помещение";
                }
                else
                {
                    RoomFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Работник") == true)
                {
                    WorkerFail.Visibility = Visibility.Visible;
                    WorkerFail.Content = "Выберите работника";
                }
                else
                {
                    WorkerFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Производитель") == true)
                {
                    ManufacturerFail.Visibility = Visibility.Visible;
                    ManufacturerFail.Content = "Укажите производителя";
                }
                else
                {
                    ManufacturerFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Статус") == true)
                {
                    StatusFail.Visibility = Visibility.Visible;
                    StatusFail.Content = "Укажите cтатус";
                }
                else
                {
                    StatusFail.Visibility = Visibility.Collapsed;
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
                if (_CurrentInventory.id == 0)
                    AccountingEquipmentEntities.GetContext().Inventory.Add(_CurrentInventory);

                try
                {
                    AccountingEquipmentEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    OperationHystory OHistory = new OperationHystory() { FK_Worker_id = SenderMail.IntId, Operation = "Добавление в таблицу инвентаризация", DateTimeOfOperation = DateTime.Now };
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
    }
}
