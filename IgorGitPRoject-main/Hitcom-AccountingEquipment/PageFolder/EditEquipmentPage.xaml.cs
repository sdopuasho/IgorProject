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
    /// Логика взаимодействия для EditEquipmentPage.xaml
    /// </summary>
    public partial class EditEquipmentPage : Page
    {
        private EquipmentCard _currentEquipmentCard = new EquipmentCard() { FK_StatusOfEquipment_id =6};
        /// <summary>
        /// Передача и создание элемента EquipmentCard 
        /// Для DataContext, а так же задание ItemSource для Combobox
        /// </summary>
        public EditEquipmentPage(EquipmentCard SelectedEQC)
        {
            InitializeComponent();
            if (SelectedEQC != null)
                _currentEquipmentCard = SelectedEQC;

            DataContext = _currentEquipmentCard;
            ComboNomenclature.ItemsSource = AccountingEquipmentEntities.GetContext().Nomenclature.ToList();
            ComboManufacturer.ItemsSource = AccountingEquipmentEntities.GetContext().Manufacturer.ToList();
            ComboModel.ItemsSource = AccountingEquipmentEntities.GetContext().Equipment.ToList();
        }
        /// <summary>
        /// Блок проверки введенных данных
        /// В случае если что-то будет неуказанно
        /// Будет добавленна строка с этим элементом, после чего под нужным элементом появится сообщение
        /// </summary>
        public bool CheckDataContext()
        {

            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentEquipmentCard.Equipment.Model))
                errors.AppendLine("Модель");
            if (string.IsNullOrWhiteSpace(_currentEquipmentCard.InventNumber))
                errors.AppendLine("Инвентарный");
            if (string.IsNullOrWhiteSpace(_currentEquipmentCard.Equipment.Manufacturer.ManufacturerName))
                errors.AppendLine("Производителя");
            if (_currentEquipmentCard.DateOfDelivery == null)
                errors.AppendLine("Дата");
            if (_currentEquipmentCard.Equipment.Nomenclature.NameOfNomenclature == null)
                errors.AppendLine("Номенклатура");
            if (errors.Length > 0)
            {
                if (errors.ToString().Contains("Модель") == true)
                {
                    ModelFail.Visibility = Visibility.Visible;
                    ModelFail.Content = "Укажите модель оборудования";
                }
                else
                {
                    ModelFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Инвентарный") == true)
                {
                    SerialNumberFail.Visibility = Visibility.Visible;
                    SerialNumberFail.Content = "Укажите инвентарный номер";
                }
                else
                {
                    SerialNumberFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Производителя") == true)
                {
                    ManufacturerFail.Visibility = Visibility.Visible;
                    ManufacturerFail.Content = "Укажите название производителя";
                }
                else
                {
                    ManufacturerFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Дата") == true)
                {
                    DateFail.Visibility = Visibility.Visible;
                    DateFail.Content = "Укажите дату получения оборудования ";
                }
                else
                {
                    DateFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Номенклатура") == true)
                {
                    NomenclatureFail.Visibility = Visibility.Visible;
                    NomenclatureFail.Content = "Выберите номенклатуру";
                }
                else
                {
                    NomenclatureFail.Visibility = Visibility.Collapsed;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckDataContext() == true)
            {
                if (_currentEquipmentCard.id == 0)
                    AccountingEquipmentEntities.GetContext().EquipmentCard.Add(_currentEquipmentCard);

                try
                {
                    AccountingEquipmentEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    OperationHystory OHistory = new OperationHystory() { FK_Worker_id = SenderMail.IntId, Operation = "Добавление в таблицу оборудование", DateTimeOfOperation = DateTime.Now };
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
