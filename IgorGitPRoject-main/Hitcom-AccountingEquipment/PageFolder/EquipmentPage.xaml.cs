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

namespace Hitcom_AccountingEquipment
{
    /// <summary>
    /// Логика взаимодействия для EquipmentPage.xaml
    /// </summary>
    public partial class EquipmentPage : Page
    {
        List<Manufacturer> manufacturers = new List<Manufacturer>();
        EquipmentCard _CurrentEquipment = new EquipmentCard();

        /// <summary>
        ///Блок инициализации данных
        /// </summary>
        public EquipmentPage()
        {
            InitializeComponent();
            DataContext = _CurrentEquipment;
            manufacturers= AccountingEquipmentEntities.GetContext().Manufacturer.ToList();
            manufacturers.Insert(0, new Manufacturer { ManufacturerName = "Всё оборудование" });
            FilteCmb.ItemsSource = manufacturers;
        }
        /// <summary>
        /// Блоки перехода для добавление или редактирования данных
        /// в случае редактирования будут переданны данные выбраного элемента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new PageFolder.EditEquipmentPage((sender as Button).DataContext as EquipmentCard));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new PageFolder.EditEquipmentPage(null));
        }
        /// <summary>
        /// Блок проверки разрешения на удаление
        /// </summary>
        public bool AccessToDelBtn(int RowToDel)
        {
            if (RowToDel > 0 && SenderMail.PositionName.Contains("Администратор"))
            {
                return true;
            }
            else
            {
                return false;
            }
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
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().EquipmentCard.ToList();
            }
        }
        /// <summary>
        /// Обработчик нажатия кнопки удаление данных
        /// в случае если проверка на удаление пройдет
        /// то программа уточнит хочет ли пользователь удалить эти данные
        /// </summary>
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var EquipmentForRemoving = DgridMyPage.SelectedItems.Cast<EquipmentCard>().ToList();
            if (AccessToDelBtn(EquipmentForRemoving.Count) == true)
            {
                if (MessageBox.Show($"Вы точно хотите удалить следующие {EquipmentForRemoving.Count} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        AccountingEquipmentEntities.GetContext().EquipmentCard.RemoveRange(EquipmentForRemoving);
                        AccountingEquipmentEntities.GetContext().SaveChanges();
                        MessageBox.Show("Данные удалены");
                        OperationHystory OHistory = new OperationHystory() { FK_Worker_id = SenderMail.IntId, Operation = "Удаление из таблицы оборудование", DateTimeOfOperation = DateTime.Now };
                        AccountingEquipmentEntities.GetContext().OperationHystory.Add(OHistory);
                        AccountingEquipmentEntities.GetContext().SaveChanges();
                        DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().EquipmentCard.ToList();
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
        /// <summary>
        /// Блок поиска данных по введенным данным
        /// </summary>
        private void SearchTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTxt.Text == "")
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().EquipmentCard.ToList();
            }
            else if(FilteCmb.SelectedIndex == 0)
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().EquipmentCard.Where(w => w.SerialNumber.StartsWith(SearchTxt.Text)).ToList();
            }
            else
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().
                    EquipmentCard.Where(w => w.SerialNumber.StartsWith(SearchTxt.Text) && w.Equipment.Manufacturer.ManufacturerName == FilteCmb.Text).ToList();
            }
        }
        /// <summary>
        /// Блок поиска данных по выбранному значению в combobox
        /// </summary>

        private void FilteCmb_DropDownClosed(object sender, EventArgs e)
        {
            if (FilteCmb.SelectedIndex == 0)
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().EquipmentCard.ToList();
            }
            else if (SearchTxt.Text == "")
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().EquipmentCard.Where(w => w.Equipment.Manufacturer.ManufacturerName == FilteCmb.Text).ToList();
            }
            else
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().
                    EquipmentCard.Where(w => w.SerialNumber.StartsWith(SearchTxt.Text) && w.Equipment.Manufacturer.ManufacturerName == FilteCmb.Text).ToList();
            }
        }

        private void BtnAddEquipment_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new PageFolder.EditNewEquipmentPage());
        }
        /// <summary>
        /// Смена статуса по нажатию кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOut_Click(object sender, RoutedEventArgs e)
        {
            EquipmentCard Card = DgridMyPage.SelectedItem as EquipmentCard;
            Card.FK_StatusOfEquipment_id = 7;
            AccountingEquipmentEntities.GetContext().SaveChanges();
        }
    }
}
