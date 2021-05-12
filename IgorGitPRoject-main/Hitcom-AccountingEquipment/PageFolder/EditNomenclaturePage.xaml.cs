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
    /// Логика взаимодействия для EditNomenclaturePage.xaml
    /// </summary>
    public partial class EditNomenclaturePage : Page
    {
        Nomenclature _currentnomenclature = new Nomenclature();
        /// <summary>
        /// Передача и создание элемента EquipmentCard 
        /// Для DataContext, а так же задание ItemSource для Combobox
        /// </summary>
        public EditNomenclaturePage(Nomenclature SelectedNomeclature)
        {
            InitializeComponent();
            if (SelectedNomeclature != null)
                _currentnomenclature = SelectedNomeclature;
            DataContext = _currentnomenclature;
            ComboTypeNomenclture.ItemsSource = AccountingEquipmentEntities.GetContext().TypeOfNomenclature.ToList();
        }
        /// <summary>
        /// Блок проверки введенных данных
        /// В случае если что-то будет неуказанно
        /// Будет добавленна строка с этим элементом, после чего под нужным элементом появится сообщение
        /// </summary>
        public bool CheckDataContext()
        {
            StringBuilder errors = new StringBuilder();
            if (ComboTypeNomenclture.SelectedItem == null)
                errors.AppendLine("Вид");
            if (string.IsNullOrWhiteSpace(_currentnomenclature.NameOfNomenclature))
                errors.AppendLine("Номенклатура");
            if (errors.Length > 0)
            {
                if (errors.ToString().Contains("Вид") == true)
                {
                    TypeNomenclatureFail.Visibility = Visibility.Visible;
                    TypeNomenclatureFail.Content = "Укажите вид номенклатуры";
                }
                else
                {
                    TypeNomenclatureFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Номенклатура") == true)
                {
                    NomenclatureFail.Visibility = Visibility.Visible;
                    NomenclatureFail.Content = "Введите номенклатуру";
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
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckDataContext() == true)
            {
                if (_currentnomenclature.id == 0)
                    AccountingEquipmentEntities.GetContext().Nomenclature.Add(_currentnomenclature);

                try
                {
                    AccountingEquipmentEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    OperationHystory OHistory = new OperationHystory() { FK_Worker_id = SenderMail.IntId, Operation = "Добавление в таблицу номенклатура", DateTimeOfOperation = DateTime.Now };
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

        private void TxtNomenclature_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex(@"[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
