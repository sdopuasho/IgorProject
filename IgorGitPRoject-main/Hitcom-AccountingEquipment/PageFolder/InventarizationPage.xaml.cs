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
using System.Data.Entity;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Win32;

namespace Hitcom_AccountingEquipment.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для InventarizationPage.xaml
    /// </summary>
    public partial class InventarizationPage : Page
    {
        Inventory _CurrentInventory = new Inventory();
        int Switchjopbl;
        /// <summary>
        ///Блок инициализации данных
        /// </summary>
        public InventarizationPage()
        {
            InitializeComponent();
            List<StatusOfInventory> statusOfs = new List<StatusOfInventory>();
            statusOfs = AccountingEquipmentEntities.GetContext().StatusOfInventory.ToList();
            statusOfs.Insert(0, new StatusOfInventory { InventoryStatus = "Весь список" });
            FilteCmb.ItemsSource = statusOfs;
            DataContext = _CurrentInventory;
            DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Inventory.ToList();
        }
        /// <summary>
        /// Блоки перехода для добавление или редактирования данных
        /// в случае редактирования будут переданны данные выбраного элемента
        /// </summary>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EditInvantarizationPage((sender as Button).DataContext as Inventory));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EditInvantarizationPage(null));
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
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Inventory.ToList();
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки удаление данных
        /// в случае если проверка на удаление пройдет
        /// то программа уточнит хочет ли пользователь удалить эти данные
        /// </summary>
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var EquipmentForRemoving = DgridMyPage.SelectedItems.Cast<Inventory>().ToList(); 
            if (AccessToDelBtn(EquipmentForRemoving.Count) == true)
            {
                if (MessageBox.Show($"Вы точно хотите удалить следующие {EquipmentForRemoving.Count} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        AccountingEquipmentEntities.GetContext().Inventory.RemoveRange(EquipmentForRemoving);
                        AccountingEquipmentEntities.GetContext().SaveChanges();
                        MessageBox.Show("Данные удалены");
                        OperationHystory OHistory = new OperationHystory() { FK_Worker_id = SenderMail.IntId, Operation = "Удаление из таблицы инвентаризация", DateTimeOfOperation = DateTime.Now };
                        AccountingEquipmentEntities.GetContext().OperationHystory.Add(OHistory);
                        AccountingEquipmentEntities.GetContext().SaveChanges();
                        DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Inventory.ToList();
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
        /// Блок поиска и замены данных в документе
        /// </summary>
        private void ReplaceWordStub(string stubToReplace, string text, Word.Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);

        }
        /// <summary>
        /// Блок заготовки данных для замены в документе
        /// </summary>
        private void InventarizationOfReport_DropDownClosed(object sender, EventArgs e)
        {
            if (InventarizationOfReport.Text == "Приказ")
                Switchjopbl = 1;
            if (InventarizationOfReport.Text == "Акт")
                Switchjopbl = 2;
            if (DgridMyPage.SelectedItem ==null)
            {
                return;
            }
            else
            {
                switch (Switchjopbl)
                {
                    case 1:
                        try
                        {
                            SaveFileDialog openDlg = new SaveFileDialog();
                            openDlg.FileName = "Отчет №";
                            openDlg.Filter = "Word (.doc)|.doc |Word (.docx)|.docx |All files (.)|.";
                            openDlg.FilterIndex = 2;
                            openDlg.RestoreDirectory = true;
                            if (openDlg.ShowDialog() == true)
                            {
                                Word.Application word = new Microsoft.Office.Interop.Word.Application();

                                Word.Document doc = word.Documents.Open(Environment.CurrentDirectory + @"\Prikaz_o_provedenii_inventarizatsii.docx");
                                var TestCaseUse = DgridMyPage.SelectedItems.Cast<Inventory>().FirstOrDefault() as Inventory;
                                var Worker1 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 1).Select(s => s.FirstName + " " + s.MiddleName.Substring(0, 1) + " " + s.LastName.Substring(0, 1)).FirstOrDefault();
                                var Worker2 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 5).Select(s => s.FirstName + " " + s.MiddleName.Substring(0, 1) + " " + s.LastName.Substring(0, 1)).FirstOrDefault();
                                var Worker3 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 9).Select(s => s.FirstName + " " + s.MiddleName.Substring(0, 1) + " " + s.LastName.Substring(0, 1)).FirstOrDefault();
                                var Worker4 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 41).Select(s => s.FirstName + " " + s.MiddleName.Substring(0, 1) + " " + s.LastName.Substring(0, 1)).FirstOrDefault();
                                var Position1 = AccountingEquipmentEntities.GetContext().Worker.Include(i => i.Position).Where(w => w.id == 1).Select(s => s.Position.PostionName).FirstOrDefault();
                                var Position2 = AccountingEquipmentEntities.GetContext().Worker.Include(i => i.Position).Where(w => w.id == 5).Select(s => s.Position.PostionName).FirstOrDefault();
                                var Position3 = AccountingEquipmentEntities.GetContext().Worker.Include(i => i.Position).Where(w => w.id == 9).Select(s => s.Position.PostionName).FirstOrDefault();
                                var Position4 = AccountingEquipmentEntities.GetContext().Worker.Include(i => i.Position).Where(w => w.id == 41).Select(s => s.Position.PostionName).FirstOrDefault();
                                var Nomencl = TestCaseUse.EquipmentCard.Equipment.Nomenclature.NameOfNomenclature;
                                var Mdl = TestCaseUse.EquipmentCard.Equipment.Model;
                                var SerNumb = TestCaseUse.EquipmentCard.SerialNumber;
                                var InvetNumber = TestCaseUse.EquipmentCard.InventNumber;
                                var StatusEquip = TestCaseUse.EquipmentCard.StatusOfEquipment.NameOfStatus;
                                var GetDate = DateTime.Today.ToShortDateString();
                                var datein = TestCaseUse.InventoryDate.ToShortDateString();
                                var iddoc = TestCaseUse.id;
                                var dateout = DateTime.Now.AddDays(3).ToShortDateString();
                                var dateoutBuh = DateTime.Now.AddDays(7).ToShortDateString();
                                var Manufac = TestCaseUse.EquipmentCard.Equipment.Manufacturer.ManufacturerName;

                                ReplaceWordStub("{FIO1}", Worker1, doc);
                                ReplaceWordStub("{FIO2}", Worker2, doc);
                                ReplaceWordStub("{FIO3}", Worker3, doc);
                                ReplaceWordStub("{FIO4}", Worker4, doc);
                                ReplaceWordStub("{Position1}", Position1, doc);
                                ReplaceWordStub("{Position2}", Position2, doc);
                                ReplaceWordStub("{Position3}", Position3, doc);
                                ReplaceWordStub("{Position4}", Position4, doc);
                                ReplaceWordStub("{InventNumber}", Mdl, doc);
                                ReplaceWordStub("{Model}", InvetNumber, doc);
                                ReplaceWordStub("{GetDate}", GetDate, doc);
                                ReplaceWordStub("{Nomenclature}", Nomencl, doc);
                                ReplaceWordStub("{SerialNumber}", SerNumb, doc);
                                ReplaceWordStub("{GetDate1}", datein, doc);
                                ReplaceWordStub("{GetDate2}", dateout, doc);
                                ReplaceWordStub("{GetDate3}", dateoutBuh, doc);
                                ReplaceWordStub("{id}", iddoc.ToString(), doc);
                                ReplaceWordStub("{Manufacturer}", Manufac.ToString(), doc);

                                doc.SaveAs2(openDlg.FileName); 
                                doc.Close();
                            }     
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("" + ex);
                        }
                        break;
                    case 2:
                        try
                        {
                            SaveFileDialog openDlg = new SaveFileDialog();
                            openDlg.FileName = "Отчет №";
                            openDlg.Filter = "Word (.doc)|.doc |Word (.docx)|.docx |All files (.)|.";
                            openDlg.FilterIndex = 2;
                            openDlg.RestoreDirectory = true;
                            if (openDlg.ShowDialog() == true)
                            {
                                Word.Application word2 = new Microsoft.Office.Interop.Word.Application();
                                Word.Document doc2 = word2.Documents.Open(Environment.CurrentDirectory + @"\Akt_o_rezultatakh_inventarizatsii.docx");
                                var TestCaseUse2 = DgridMyPage.SelectedItems.Cast<Inventory>().FirstOrDefault() as Inventory;
                                var Worker11 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 1).Select(s => s.LastName.Substring(0, 1) + " " + s.MiddleName.Substring(0, 1) + " " + s.FirstName).FirstOrDefault();
                                var Worker21 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 5).Select(s => s.LastName.Substring(0, 1) + " " + s.MiddleName.Substring(0, 1) + " " + s.FirstName).FirstOrDefault();
                                var Worker31 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 9).Select(s => s.LastName.Substring(0, 1) + " " + s.MiddleName.Substring(0, 1) + " " + s.FirstName).FirstOrDefault();
                                var Worker41 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 41).Select(s => s.LastName.Substring(0, 1) + " " + s.MiddleName.Substring(0, 1) + " " + s.FirstName).FirstOrDefault();
                                var Worker51 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 29).Select(s => s.LastName.Substring(0, 1) + " " + s.MiddleName.Substring(0, 1) + " " + s.FirstName).FirstOrDefault();
                                var Worker61 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 35).Select(s => s.LastName.Substring(0, 1) + " " + s.MiddleName.Substring(0, 1) + " " + s.FirstName).FirstOrDefault();
                                var Worker81 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 19).Select(s => s.LastName.Substring(0, 1) + " " + s.MiddleName.Substring(0, 1) + " " + s.FirstName).FirstOrDefault();
                                var Position21 = AccountingEquipmentEntities.GetContext().Worker.Include(i => i.Position).Where(w => w.id == 5).Select(s => s.Position.PostionName).FirstOrDefault();
                                var XHuman71 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 23).Select(s => s.LastName.Substring(0, 1) + " " + s.MiddleName.Substring(0, 1) + " " + s.FirstName).FirstOrDefault();
                                var Position31 = AccountingEquipmentEntities.GetContext().Worker.Include(i => i.Position).Where(w => w.id == 9).Select(s => s.Position.PostionName).FirstOrDefault();
                                var Position41 = AccountingEquipmentEntities.GetContext().Worker.Include(i => i.Position).Where(w => w.id == 41).Select(s => s.Position.PostionName).FirstOrDefault();
                                var Position51 = AccountingEquipmentEntities.GetContext().Worker.Include(i => i.Position).Where(w => w.id == 29).Select(s => s.Position.PostionName).FirstOrDefault();
                                var Position61 = AccountingEquipmentEntities.GetContext().Worker.Include(i => i.Position).Where(w => w.id == 23).Select(s => s.Position.PostionName).FirstOrDefault();
                                var GetDate1 = DateTime.Today.ToShortDateString();
                                var datein1 = TestCaseUse2.InventoryDate.ToShortDateString();
                                var iddoc1 = TestCaseUse2.id;
                                var dateout1 = DateTime.Now.AddDays(3).ToShortDateString();
                                var dateoutBuh1 = DateTime.Now.AddDays(7).ToShortDateString();
                                var dateoutBuh2 = DateTime.Now.AddDays(10).ToShortDateString();
                                ReplaceWordStub("{Worker}", Worker81, doc2);
                                ReplaceWordStub("{Worker1}", Worker11, doc2);
                                ReplaceWordStub("{F.M.Lname1}", Worker21, doc2);
                                ReplaceWordStub("{F.M.Lname2}", Worker31, doc2);
                                ReplaceWordStub("{F.M.Lname3}", Worker41, doc2);
                                ReplaceWordStub("{F.M.Lname4}", Worker51, doc2);
                                ReplaceWordStub("{F.M.Lname4}", Worker61, doc2);
                                ReplaceWordStub("{Position1}", Position21, doc2);
                                ReplaceWordStub("{F.M.Lname5}", Worker61, doc2);
                                ReplaceWordStub("{Position2}", Position31, doc2);
                                ReplaceWordStub("{Position3}", Position41, doc2);
                                ReplaceWordStub("{Position4}", Position51, doc2);
                                ReplaceWordStub("{Position5}", Position61, doc2);
                                ReplaceWordStub("{Date1}", GetDate1, doc2);
                                ReplaceWordStub("{InventoryDate}", datein1, doc2);
                                ReplaceWordStub("{GetDate1}", datein1, doc2);
                                ReplaceWordStub("{GetDate2}", dateout1, doc2);
                                ReplaceWordStub("{GetDate3}", dateoutBuh1, doc2);
                                ReplaceWordStub("{GetDate4}", dateoutBuh2, doc2);
                                ReplaceWordStub("{GetDate5}", dateoutBuh2, doc2);
                                ReplaceWordStub("{idInvent}", iddoc1.ToString(), doc2);
                                ReplaceWordStub("{id}", iddoc1.ToString(), doc2);
                                ReplaceWordStub("{id1}", iddoc1.ToString(), doc2);
                                ReplaceWordStub("{GetDate5}", GetDate1, doc2);
                                ReplaceWordStub("{FirstComm}", Worker21, doc2);
                                ReplaceWordStub("{SecondComm}", Worker31, doc2);
                                ReplaceWordStub("{ThreeComm}", Worker41, doc2);
                                ReplaceWordStub("{Comment}", TestCaseUse2.Comment, doc2);

                                doc2.SaveAs2(openDlg.FileName);
                                doc2.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("" + ex);
                        }
                        break;
                }
            }

          
        }
        /// <summary>
        /// Блок поиска данных по выбранному значению в combobox
        /// </summary>
        
        private void FilteCmb_DropDownClosed(object sender, EventArgs e)
        {
            if(FilteCmb.SelectedIndex == 0)
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Inventory.ToList();
            }
            else if(SearchTxt.Text == "")
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Inventory.Where(w=>w.StatusOfInventory.InventoryStatus==FilteCmb.Text).ToList();
            }
            else
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().
                    Inventory.Where(w=>w.EquipmentCard.Equipment.Nomenclature.NameOfNomenclature.StartsWith(SearchTxt.Text) && w.StatusOfInventory.InventoryStatus == FilteCmb.Text).ToList();
            }
        }
        /// <summary>
        /// Блок поиска данных по введенным данным
        /// </summary>

        private void SearchTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTxt.Text == "")
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Inventory.ToList();
            }
            else if (FilteCmb.SelectedIndex == 0)
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Inventory.Where(w => w.EquipmentCard.Equipment.Nomenclature.NameOfNomenclature.StartsWith(SearchTxt.Text)).ToList();
            }
            else
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().
                    Inventory.Where(w => w.EquipmentCard.Equipment.Nomenclature.NameOfNomenclature.StartsWith(SearchTxt.Text) && w.StatusOfInventory.InventoryStatus == FilteCmb.Text).ToList();
            }
        }
    }
}
