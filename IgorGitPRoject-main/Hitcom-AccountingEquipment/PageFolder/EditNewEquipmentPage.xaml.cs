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
    /// Логика взаимодействия для EditNewEquipmentPage.xaml
    /// </summary>
    public partial class EditNewEquipmentPage : Page
    {
        Equipment _currentEquipment = new Equipment();
        /// <summary>
        /// Передача и создание элемента EquipmentCard 
        /// Для DataContext, а так же задание ItemSource для Combobox
        /// </summary>
        public EditNewEquipmentPage()
        {
            InitializeComponent();
            DataContext = _currentEquipment;
            ComboManufacturer.ItemsSource = AccountingEquipmentEntities.GetContext().Manufacturer.ToList();
            ComboNomenclature.ItemsSource = AccountingEquipmentEntities.GetContext().Nomenclature.ToList();
        }
        /// <summary>
        /// Блок проверки введенных данных
        /// В случае если что-то будет неуказанно
        /// Будет добавленна строка с этим элементом, после чего под нужным элементом появится сообщение
        /// </summary>
        public bool CheckDataContext()
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(_currentEquipment.Model))
                errors.Append("Модель");
            if (string.IsNullOrEmpty(LifeT.Text))
                errors.Append("Срок");
            if (ComboNomenclature.SelectedItem == null)
                errors.Append("Номеклатура");
            if (ComboManufacturer.SelectedItem == null)
                errors.Append("Производителя");
            if (ComboManufacturer.SelectedItem == null)
            {
                if (errors.ToString().Contains("Модель") == true)
                {
                    ModelFail.Visibility = Visibility.Visible;
                    ModelFail.Content = "Введите модель";
                }
                else
                {
                    ModelFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Срок") == true)
                {
                    DateWorkFail.Visibility = Visibility.Visible;
                    DateWorkFail.Content = "Укажите срок службы";
                }
                else
                {
                    DateWorkFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Номеклатура") == true)
                {
                    NomenclatureFail.Visibility = Visibility.Visible;
                    NomenclatureFail.Content = "Укажите номеклатуру";
                }
                else
                {
                    NomenclatureFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Производителя") == true)
                {
                    ManufacturerFail.Visibility = Visibility.Visible;
                    ManufacturerFail.Content = "Укажите производителя";
                }
                else
                {
                    ManufacturerFail.Visibility = Visibility.Collapsed;
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
                try
                {
                    AccountingEquipmentEntities.GetContext().Equipment.Add(_currentEquipment);
                    AccountingEquipmentEntities.GetContext().SaveChanges();

                    OperationHystory OHistory = new OperationHystory() { FK_Worker_id = SenderMail.IntId, Operation = "Добавление в таблицу оборудование", DateTimeOfOperation = DateTime.Now };
                    AccountingEquipmentEntities.GetContext().OperationHystory.Add(OHistory);
                    AccountingEquipmentEntities.GetContext().SaveChanges();
                    FrameManager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex);
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
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^а-яА-Я0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
