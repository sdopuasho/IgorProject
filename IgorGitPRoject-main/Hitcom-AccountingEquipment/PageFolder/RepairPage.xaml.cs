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
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;

namespace Hitcom_AccountingEquipment.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для RepairPage.xaml
    /// </summary>
    public partial class RepairPage : Excel.Page
    {
        List<Manufacturer> Manufacturers = new List<Manufacturer>();
        int switchNumber;
        /// <summary>
        ///Блок инициализации данных
        /// </summary>
        public RepairPage()
        {
            InitializeComponent();
            load();
            Manufacturers = AccountingEquipmentEntities.GetContext().Manufacturer.ToList();
            Manufacturers.Insert(0, new Manufacturer { ManufacturerName = "Всё оборудование" });
            ManufacturerCmb.ItemsSource = Manufacturers;
        }
        public void load()
        {
            DgridMyPage_Copy.ItemsSource = AccountingEquipmentEntities.GetContext().Repair.ToList();
        }
        /// <summary>
        /// Блоки перехода для добавление или редактирования данных
        /// в случае редактирования будут переданны данные выбраного элемента
        /// </summary>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EditRepairPage((sender as System.Windows.Controls.Button).DataContext as Repair));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EditRepairPage(null));
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
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Repair.ToList();
            }
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
        /// Обработчик нажатия кнопки удаление данных
        /// в случае если проверка на удаление пройдет
        /// то программа уточнит хочет ли пользователь удалить эти данные
        /// </summary>
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var TestCaseUse = DgridMyPage.SelectedItems.Cast<Repair>().ToList();
            if (AccessToDelBtn(TestCaseUse.Count) == true)
            {
            if (MessageBox.Show($"Вы точно хотите удалить следующие {TestCaseUse.Count} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AccountingEquipmentEntities.GetContext().Repair.RemoveRange(TestCaseUse);
                    AccountingEquipmentEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены");
                    OperationHystory OHistory = new OperationHystory() { FK_Worker_id = SenderMail.IntId, Operation = "Удаление из таблицы ремонты", DateTimeOfOperation = DateTime.Now };
                    AccountingEquipmentEntities.GetContext().OperationHystory.Add(OHistory);
                    AccountingEquipmentEntities.GetContext().SaveChanges();
                    DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Repair.ToList();
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
        /// Блок обработчик изменения сотояния ремонта на определенный
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            var TestCaseUse = DgridMyPage.SelectedItems.Cast<Repair>().FirstOrDefault() as Repair;
            TestCaseUse.FK_StatusEquipInRepair_id = 8;
            try
            {
                AccountingEquipmentEntities.GetContext().SaveChanges();
                MessageBox.Show("Возврат оборудования из ремонта оформлен");
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Repair.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }
        }
        /// <summary>
        /// Блок создания отчета в формате excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application app = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;
            var process = Process.GetProcessesByName("EXCEL");

            SaveFileDialog openDlg = new SaveFileDialog();
            openDlg.FileName = "Заявка №";
            openDlg.Filter = "Excel (.xls)|*.xls |Excel (.xlsx)|*.xlsx |All files (*.*)|*.*";
            openDlg.FilterIndex = 2;
            openDlg.RestoreDirectory = true;

            if (openDlg.ShowDialog() == true)
            {
                app = new Excel.Application();
                app.Visible = true;
                app.DisplayAlerts = false;
                wb = app.Workbooks.Add();
                ws = wb.ActiveSheet;
                string path = openDlg.FileName;
                DgridMyPage_Copy.SelectAllCells();
                DgridMyPage_Copy.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, DgridMyPage_Copy);
                ws.Paste();
                ws.Range["A1", "L1"].Font.Bold = true;
                int number1 = ws.UsedRange.Rows.Count;
                Microsoft.Office.Interop.Excel.Range myRange = ws.Range["A1", "L" + number1];
                myRange.Borders.LineStyle = XlLineStyle.xlContinuous;
                myRange.WrapText = false;
                ws.Columns.EntireColumn.AutoFit();
                wb.SaveAs(path);
            }

        }
        public HeaderFooter LeftHeader => throw new NotImplementedException();

        public HeaderFooter CenterHeader => throw new NotImplementedException();

        public HeaderFooter RightHeader => throw new NotImplementedException();

        public HeaderFooter LeftFooter => throw new NotImplementedException();

        public HeaderFooter CenterFooter => throw new NotImplementedException();

        public HeaderFooter RightFooter => throw new NotImplementedException();

        /// <summary>
        /// Блок заготовки данных для замены в документе
        /// </summary>
        private void BtnExcel_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (Cmb.Text == "Акт утилизации")
                switchNumber = 1;
            if (Cmb.Text == "Акт ремонта")
                switchNumber = 2;
            if (Cmb.Text == "Акт приёма-передачи")
                switchNumber = 3;
            if (Cmb.Text == "Акт возврата")
                switchNumber = 4;


            if (DgridMyPage.SelectedItem == null)
            {
                return;
            }
            else
            {
                switch (switchNumber)
                {
                    case 1:
                        SaveFileDialog openDlg = new SaveFileDialog();
                        openDlg.FileName = "Отчет №";
                        openDlg.Filter = "Word (.doc)|.doc |Word (.docx)|.docx |All files (.)|.";
                        openDlg.FilterIndex = 2;
                        openDlg.RestoreDirectory = true;
                        if (openDlg.ShowDialog() == true)
                        {
                            Word.Application word = new Microsoft.Office.Interop.Word.Application();
                            Word.Document doc = word.Documents.Open(Environment.CurrentDirectory + @"\Akt_utilizatsii_oborudovania.docx");
                            var TestCaseUse = DgridMyPage.SelectedItems.Cast<Repair>().FirstOrDefault() as Repair;
                            var XHuman1 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 1).Select(s => s.FirstName + " " + s.MiddleName.Substring(0, 1) + " " + s.LastName.Substring(0, 1)).FirstOrDefault();
                            var XHuman2 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 5).Select(s => s.FirstName + " " + s.MiddleName.Substring(0, 1) + " " + s.LastName.Substring(0, 1)).FirstOrDefault();
                            var XHuman3 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 9).Select(s => s.FirstName + " " + s.MiddleName.Substring(0, 1) + " " + s.LastName.Substring(0, 1)).FirstOrDefault();
                            var Nomencl = TestCaseUse.EquipmentCard.Equipment.Nomenclature.NameOfNomenclature;
                            var Mdl = TestCaseUse.EquipmentCard.Equipment.Model;
                            var SerNumb = TestCaseUse.EquipmentCard.SerialNumber;
                            var StatusEquip = TestCaseUse.EquipmentCard.StatusOfEquipment.NameOfStatus;
                            var Break = TestCaseUse.StatusEquipInRepair.StatusEquipInRepair1;
                            var GetDate = DateTime.Today.ToShortDateString();
                            try
                            {
                                ReplaceWordStub("{FirstComm}", XHuman1, doc);
                                ReplaceWordStub("{SecComm}", XHuman2, doc);
                                ReplaceWordStub("{ThreeComm}", XHuman3, doc);
                                ReplaceWordStub("{GetDate}", GetDate.ToString(), doc);
                                ReplaceWordStub("{Model}", Mdl, doc);
                                ReplaceWordStub("{Nomenclature}", Nomencl, doc);
                                ReplaceWordStub("{SerialNumber}", SerNumb, doc);
                                ReplaceWordStub("{StatusEq}", StatusEquip.ToString(), doc);
                                ReplaceWordStub("{Dead}", Break, doc);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("" + ex);
                            }
                            doc.SaveAs2(openDlg.FileName);
                            doc.Close();
                        }
                        break;
                    case 2:
                        SaveFileDialog openDlg1 = new SaveFileDialog();
                        openDlg1.FileName = "Отчет №";
                        openDlg1.Filter = "Word (.doc)|.doc |Word (.docx)|.docx |All files (.)|.";
                        openDlg1.FilterIndex = 2;
                        openDlg1.RestoreDirectory = true;
                        if (openDlg1.ShowDialog() == true)
                        {
                            Word.Application word2 = new Microsoft.Office.Interop.Word.Application();
                            Word.Document doc2 = word2.Documents.Open(Environment.CurrentDirectory + @"\Akt_remonta_oborudovania.docx");
                            var TestCaseUse2 = DgridMyPage.SelectedItems.Cast<Repair>().FirstOrDefault() as Repair;
                            var HumanFIO = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 6).Select(s => s.FirstName + " " + s.MiddleName.Substring(0, 1) + " " + s.LastName.Substring(0, 1)).FirstOrDefault();
                            var DateDelBerk = TestCaseUse2.EquipmentCard.DateOfDelivery.ToShortDateString();
                            var Manufac = TestCaseUse2.EquipmentCard.Equipment.Manufacturer.ManufacturerName; ;
                            var Nomencl2 = TestCaseUse2.EquipmentCard.Equipment.Nomenclature.NameOfNomenclature;
                            var Mdl2 = TestCaseUse2.EquipmentCard.Equipment.Model;
                            var SerNum2b = TestCaseUse2.EquipmentCard.SerialNumber;
                            var PhoneNumb = TestCaseUse2.Worker.PhoneNumber;
                            var Brk2 = TestCaseUse2.StatusEquipInRepair.StatusEquipInRepair1;
                            var DateCreateBrk = DateTime.Today.ToShortDateString();
                            try
                            {
                                ReplaceWordStub("{DateOfDelivery}", DateDelBerk, doc2);
                                ReplaceWordStub("{Nomenclature}", Nomencl2, doc2);
                                ReplaceWordStub("{Manufacturer}", Manufac, doc2);
                                ReplaceWordStub("{Model}", Mdl2, doc2);
                                ReplaceWordStub("{SerialNumber}", SerNum2b, doc2);
                                ReplaceWordStub("{BreakDown}", Brk2, doc2);
                                ReplaceWordStub("{FirstName}", HumanFIO, doc2);
                                ReplaceWordStub("{Number}", PhoneNumb, doc2);
                                ReplaceWordStub("{Email}", Brk2, doc2);
                                ReplaceWordStub("{DateRepair}", DateCreateBrk, doc2);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("" + ex);
                            }
                            doc2.SaveAs2(openDlg1.FileName); 
                            doc2.Close();
                        }
                        break;
                    case 3:
                        SaveFileDialog openDlg2 = new SaveFileDialog();
                        openDlg2.FileName = "Отчет №";
                        openDlg2.Filter = "Word (.doc)|.doc |Word (.docx)|.docx |All files (.)|.";
                        openDlg2.FilterIndex = 2;
                        openDlg2.RestoreDirectory = true;
                        if (openDlg2.ShowDialog() == true)
                        {
                            Word.Application word3 = new Microsoft.Office.Interop.Word.Application();
                            Word.Document doc3 = word3.Documents.Open(Environment.CurrentDirectory + @"\Akt_priyoma_peredachi_oborudovania_v_remont.docx");
                            var TestCaseUse3 = DgridMyPage.SelectedItems.Cast<Repair>().FirstOrDefault() as Repair;
                            var HumanFIO2 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 6).Select(s => s.LastName.Substring(0, 1) + " " + s.MiddleName.Substring(0, 1) + " " + s.FirstName).FirstOrDefault();
                            var DateDeliverReapir = TestCaseUse3.DateOfDeliveryForRepair.ToShortDateString();
                            var Manufac2 = TestCaseUse3.EquipmentCard.Equipment.Manufacturer.ManufacturerName;
                            var Nomencl3 = TestCaseUse3.EquipmentCard.Equipment.Nomenclature.NameOfNomenclature;
                            var Mdl4 = TestCaseUse3.EquipmentCard.Equipment.Model;
                            var SerNum3b = TestCaseUse3.EquipmentCard.SerialNumber;
                            var PhoneNumb2 = TestCaseUse3.Worker.PhoneNumber;
                            var Mail = TestCaseUse3.Worker.EmailOfWorker;
                            var Positions = TestCaseUse3.Worker.Position.PostionName;
                            var DateCreateBrk3 = TestCaseUse3.DateOfReceiving.ToShortDateString();
                            var Breakd = TestCaseUse3.Breakdown.NameOfBreakdown;
                            var ServiceOrg = TestCaseUse3.ServiceOrganization.NameOfServiceOrganization;
                            try
                            {
                                ReplaceWordStub("{DateOfDeliveryForRepair}", DateDeliverReapir, doc3);
                                ReplaceWordStub("{Nomenclature}", Nomencl3, doc3);
                                ReplaceWordStub("{Manufacturer}", Manufac2, doc3);
                                ReplaceWordStub("{Model}", Mdl4, doc3);
                                ReplaceWordStub("{BreakDown}", Breakd, doc3);
                                ReplaceWordStub("{F.M.LastName}", HumanFIO2, doc3);
                                ReplaceWordStub("{ContactPhone}", PhoneNumb2, doc3);
                                ReplaceWordStub("{Email}", Mail, doc3);
                                ReplaceWordStub("{DateOfDeliveryForRepair}", DateDeliverReapir, doc3);
                                ReplaceWordStub("{DateOfReceiving}", DateCreateBrk3, doc3);
                                ReplaceWordStub("{ServiceOrganization}", ServiceOrg, doc3);
                                ReplaceWordStub("{ServiceOrganization1}", ServiceOrg, doc3);
                                ReplaceWordStub("{Position}", Positions, doc3);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("" + ex);
                            }
                            doc3.SaveAs2(openDlg2.FileName); 
                            doc3.Close();
                        }
                        break;
                    case 4:
                        SaveFileDialog openDlg3 = new SaveFileDialog();
                        openDlg3.FileName = "Отчет №";
                        openDlg3.Filter = "Word (.doc)|.doc |Word (.docx)|.docx |All files (.)|.";
                        openDlg3.FilterIndex = 2;
                        openDlg3.RestoreDirectory = true;
                        if (openDlg3.ShowDialog() == true)
                        {
                            Word.Application word4 = new Microsoft.Office.Interop.Word.Application();
                            Word.Document doc4 = word4.Documents.Open(Environment.CurrentDirectory + @"\Akt_vozvrata_oborudovania_iz_remonta.docx");
                            var TestCaseUse4 = DgridMyPage.SelectedItems.Cast<Repair>().FirstOrDefault() as Repair;
                            var HumanFIO23 = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == 6).Select(s => s.LastName.Substring(0, 1) + " " + s.MiddleName.Substring(0, 1) + " " + s.FirstName).FirstOrDefault();
                            var DateDeliverReapir3 = TestCaseUse4.DateOfDeliveryForRepair.ToShortDateString();
                            var Manufac22 = TestCaseUse4.EquipmentCard.Equipment.Manufacturer.ManufacturerName;
                            var Nomencl33 = TestCaseUse4.EquipmentCard.Equipment.Nomenclature.NameOfNomenclature;
                            var Mdl5 = TestCaseUse4.EquipmentCard.Equipment.Model;
                            var Positions2 = TestCaseUse4.Worker.Position.PostionName;
                            var DateCreateBrk4 = TestCaseUse4.DateOfReceiving.ToShortDateString();
                            var ServiceOrg2 = TestCaseUse4.ServiceOrganization.NameOfServiceOrganization;
                            try
                            {
                                ReplaceWordStub("{Nomenclature}", Nomencl33, doc4);
                                ReplaceWordStub("{Manufacturer}", Manufac22, doc4);
                                ReplaceWordStub("{Model}", Mdl5, doc4);
                                ReplaceWordStub("{F.M.LastName}", HumanFIO23, doc4);
                                ReplaceWordStub("{DateOfDelivery}", DateDeliverReapir3, doc4);
                                ReplaceWordStub("{DateOfReceiving}", DateCreateBrk4, doc4);
                                ReplaceWordStub("{DateOfReceiving1}", DateDeliverReapir3, doc4);
                                ReplaceWordStub("{DateOfReceiving2}", DateCreateBrk4, doc4);
                                ReplaceWordStub("{ServiceOrganization}", ServiceOrg2, doc4);
                                ReplaceWordStub("{ServiceOrganization1}", ServiceOrg2, doc4);
                                ReplaceWordStub("{Position}", Positions2, doc4);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("" + ex);
                            }
                            doc4.SaveAs2(openDlg3.FileName); 
                            doc4.Close();
                        }
                        break;
                                         
                }
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
        /// Блок поиска данных по введенным данным
        /// </summary>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox txt = sender as System.Windows.Controls.TextBox;
            if (txt.Text=="")
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Repair.ToList();
            }
            else if(ManufacturerCmb.SelectedIndex == 0)
            {
                DgridMyPage.ItemsSource= AccountingEquipmentEntities.GetContext().Repair.Where(w => w.EquipmentCard.SerialNumber.StartsWith(txt.Text)).ToList();
            }
            else
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Repair.
                    Where(w => w.EquipmentCard.Equipment.Manufacturer.ManufacturerName == ManufacturerCmb.Text && w.EquipmentCard.SerialNumber.StartsWith(SrchTxt.Text)).ToList();
            }
        }
        /// <summary>
        /// Блок поиска данных по выбранному значению в combobox
        /// </summary>
        private void ManufacturerCmb_DropDownClosed(object sender, EventArgs e)
        {
            if (ManufacturerCmb.SelectedIndex == 0)
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Repair.ToList();
            }
            else if(SrchTxt.Text =="")
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Repair.Where(w => w.EquipmentCard.Equipment.Manufacturer.ManufacturerName == ManufacturerCmb.Text).ToList();
            }
            else
            {
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().Repair.
                    Where(w => w.EquipmentCard.Equipment.Manufacturer.ManufacturerName == ManufacturerCmb.Text && w.EquipmentCard.SerialNumber.StartsWith(SrchTxt.Text)).ToList();
            }
        }

        
    }
}
