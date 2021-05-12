using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    /// Логика взаимодействия для EditWorkerPage.xaml
    /// </summary>
    public partial class EditWorkerPage : Page
    {
        Worker _CurrentWorker = new Worker() {Login = "testtest",Password = "tasdasd", CheckFirstVisit = false };
        /// <summary>
        /// Передача и создание элемента EquipmentCard 
        /// Для DataContext, а так же задание ItemSource для Combobox
        /// </summary>
        public EditWorkerPage(Worker SeletedWorker)
        {
            InitializeComponent(); if (SeletedWorker != null)
                _CurrentWorker = SeletedWorker;
            DataContext = _CurrentWorker;
            ComboGender.ItemsSource = AccountingEquipmentEntities.GetContext().Gender.ToList();
            ComboPosition.ItemsSource = AccountingEquipmentEntities.GetContext().Position.ToList();
            ComboStatus.ItemsSource = AccountingEquipmentEntities.GetContext().StatusOfWorker.ToList();
        }



        /// <summary>
        /// Обработка собыитя нажатия кнопки сохранить
        /// После нажатия проверяется все ли данные введены
        /// В случае если нет, то редактирование или сохранение отменится
        /// В противном случае данные будут внесены в бд
        /// </summary>

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_CurrentWorker.FirstName))
                errors.AppendLine("Фамилия");
            if (string.IsNullOrWhiteSpace(_CurrentWorker.LastName))
                errors.AppendLine("Имя");
            if (string.IsNullOrWhiteSpace(_CurrentWorker.MiddleName))
                errors.AppendLine("Отчество");
            if (ComboGender.SelectedItem == null)
                errors.AppendLine("Пол");
            if (string.IsNullOrWhiteSpace(_CurrentWorker.PhoneNumber))
                errors.AppendLine("Телефон");
            if (string.IsNullOrWhiteSpace(_CurrentWorker.EmailOfWorker))
                errors.AppendLine("Почту");
            if (ComboPosition.SelectedItem==null)
                errors.AppendLine("Должность");
            if (ComboStatus.SelectedItem==null)
                errors.AppendLine("Статус");
            if (errors.Length > 0)
            {
                if (errors.ToString().Contains("Фамилия") == true)
                {
                    FnameFail.Visibility = Visibility.Visible;
                    FnameFail.Content = "Введите Фамилию";
                }
                else
                {
                    FnameFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Имя") == true)
                {
                    LnameFail.Visibility = Visibility.Visible;
                    LnameFail.Content = "Введите Имя";
                }
                else
                {
                    LnameFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Отчество") == true)
                {
                    MnameFail.Visibility = Visibility.Visible;
                    MnameFail.Content = "Введите отчество";
                }
                else
                {
                    MnameFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Пол") == true)
                {
                    GenderFail.Visibility = Visibility.Visible;
                    GenderFail.Content = "Выберите пол";
                }
                else
                {
                    GenderFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Телефон") == true)
                {
                    PhoneFail.Visibility = Visibility.Visible;
                    PhoneFail.Content = "Введите номер телефона";
                }
                else
                {
                    PhoneFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Почту") == true)
                {
                    EmailFail.Visibility = Visibility.Visible;
                    EmailFail.Content = "Введите почту";
                }
                else
                {
                    EmailFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Должность") == true)
                {
                    PositionFail.Visibility = Visibility.Visible;
                    PositionFail.Content = "Выберите должность";
                }
                else
                {
                    PositionFail.Visibility = Visibility.Collapsed;
                }
                if (errors.ToString().Contains("Статус") == true)
                {
                    StatusWorkerFail.Visibility = Visibility.Visible;
                    StatusWorkerFail.Content = "Выберите статус";
                }
                else
                {
                    StatusWorkerFail.Visibility = Visibility.Collapsed;
                }
                return;
            }
            if (_CurrentWorker.id == 0)
                AccountingEquipmentEntities.GetContext().Worker.Add(_CurrentWorker);
            try
            {
                AccountingEquipmentEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");

                OperationHystory OHistory = new OperationHystory() { FK_Worker_id = SenderMail.IntId, Operation = "Добавление в таблицу Сотрудники", DateTimeOfOperation = DateTime.Now };
                AccountingEquipmentEntities.GetContext().OperationHystory.Add(OHistory);
                AccountingEquipmentEntities.GetContext().SaveChanges();
                FrameManager.MainFrame.GoBack();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DialogToPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image (*.bmp, *.jpg, *.ico, .png)|.bmp; *.jpg; *.gif; .png|All (.)|.*";
            openFileDialog.ShowDialog();
            string ext = System.IO.Path.GetExtension(openFileDialog.FileName);
            if (ext == ".bmp" || ext == ".ico" || ext == ".png" || ext == ".jpg")
            {
                PhotoWorker.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                _CurrentWorker.Photo = ImageToByte(openFileDialog.FileName);
            }
            else
            {
                MessageBox.Show("Вы выбрали изображение", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public byte[] ImageToByte(string Path)
        {
            byte[] image;
            image = File.ReadAllBytes(Path);
            return image;
        }
        /// <summary>
        /// Проверка на вводимые символы в Textbox
        /// В случае если символ введен неверный, то запись в TextBox не произойдет
        /// </summary>

        private void FName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex(@"[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void LName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex(@"[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void MName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex(@"[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[^0-9.(.).+.-]");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
