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
    /// Логика взаимодействия для EditRepairPage.xaml
    /// </summary>
    public partial class EditRepairPage : Page
    {
        Repair _CurrentBreakdown = new Repair() {DateOfDeliveryForRepair = DateTime.Now, DateOfReceiving = DateTime.Now.AddDays(7) };
        /// <summary>
        /// Передача и создание элемента EquipmentCard 
        /// Для DataContext, а так же задание ItemSource для Combobox
        /// </summary>
        public EditRepairPage(Repair SelectedBRKDWN)
        {
            InitializeComponent(); 
            if (SelectedBRKDWN != null)
                _CurrentBreakdown = SelectedBRKDWN;
            DataContext = _CurrentBreakdown;
            ComboBreakdown.ItemsSource = AccountingEquipmentEntities.GetContext().Breakdown.ToList();
            InventNumber.ItemsSource = AccountingEquipmentEntities.GetContext().EquipmentCard.ToList();
            ComboModel.ItemsSource = AccountingEquipmentEntities.GetContext().Equipment.ToList();
            ComboServiceName.ItemsSource = AccountingEquipmentEntities.GetContext().ServiceOrganization.ToList();
            ComboStatusRepair.ItemsSource = AccountingEquipmentEntities.GetContext().StatusEquipInRepair.ToList();
            ComboFIO.ItemsSource = AccountingEquipmentEntities.GetContext().Worker.ToList();
        }
        /// <summary>
        /// Блок проверки введенных данных
        /// В случае если что-то будет неуказанно
        /// Будет добавленна строка с этим элементом, после чего под нужным элементом появится сообщение
        /// </summary>
        public bool checkDataContext()
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(_CurrentBreakdown.NumberOfRepair))
                errors.AppendLine("Номер");
            if (DPDeliver == null)
                errors.AppendLine("DateSend");
            if (DPBack == null)
                errors.AppendLine("DateBack");
            if (ComboBreakdown.SelectedItem == null)
                errors.AppendLine("Поломка");
            if (InventNumber.SelectedItem == null)
                errors.AppendLine("Инвентарь");
            if (ComboModel.SelectedItem == null)
                errors.AppendLine("Модель");
            if (ComboServiceName.SelectedItem == null)
                errors.AppendLine("Сервис");
            if (ComboFIO.SelectedItem == null)
                errors.AppendLine("Сотрудник");
            if (ComboStatusRepair.SelectedItem == null)
                errors.AppendLine("Статус");
            if (errors.Length > 0)
            {
                if (errors.ToString().Contains("Номер") == true)
                {
                    NumberRepairFail.Visibility = Visibility.Visible;
                    NumberRepairFail.Content = "Введите номер ремонта";
                }
                else
                {
                    NumberRepairFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("DateSend") == true)
                {
                    SendDateFail.Visibility = Visibility.Visible;
                    SendDateFail.Content = "Укажите дату выдачи в ремонт оборудования ";
                }
                else
                {
                    SendDateFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("DateBack") == true)
                {
                    BackDateFail.Visibility = Visibility.Visible;
                    BackDateFail.Content = "Укажите дату получения оборудования ";
                }
                else
                {
                    BackDateFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Поломка") == true)
                {
                    BreakdownFail.Visibility = Visibility.Visible;
                    BreakdownFail.Content = "Укажите поломку";
                }
                else
                {
                    BreakdownFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Инвентарь") == true)
                {
                    InventNumberFail.Visibility = Visibility.Visible;
                    InventNumberFail.Content = "Выберите инвентарный номер";
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
                if (errors.ToString().Contains("Сервис") == true)
                {
                    ServiceFail.Visibility = Visibility.Visible;
                    ServiceFail.Content = "Выберите сервис организацию";
                }
                else
                {
                    ServiceFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Сотрудник") == true)
                {
                    WorkerFail.Visibility = Visibility.Visible;
                    WorkerFail.Content = "Выберите сотрудника";
                }
                else
                {
                    WorkerFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Статус") == true)
                {
                    StatusRepairFail.Visibility = Visibility.Visible;
                    StatusRepairFail.Content = "Укажите статус ремонта";
                }
                else
                {
                    StatusRepairFail.Visibility = Visibility.Collapsed;
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
            if (checkDataContext() == true)
            {
                if (_CurrentBreakdown.id == 0)
                    AccountingEquipmentEntities.GetContext().Repair.Add(_CurrentBreakdown);

                try
                {
                    AccountingEquipmentEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
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
        private void RepairNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^а-яА-Я0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
