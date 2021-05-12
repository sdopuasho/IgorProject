using Hitcom_AccountingEquipment.PageFolder;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ChromelessWindow
    {
        /// <summary>
        /// Блок инициализации данных
        /// </summary>
        public MainWindow()
        {
            Syncfusion.SfSkinManager.SfSkinManager.ApplyStylesOnApplication = true;
            InitializeComponent();
            FrameMW.Navigate(new InventarizationPage());
            FrameManager.MainFrame = FrameMW;
            FrameManager.MainFrameContext = FrameMWContent;
            loadWorker();
        }
        /// <summary>
        /// Блок инициализации отображения данных по уровню доступа
        /// </summary>
        public void loadWorker()
        {
            Worker Wrk = AccountingEquipmentEntities.GetContext().Worker.Where(w => w.id == SenderMail.IntId).FirstOrDefault();
            UIasd.FioTxt.Text = Wrk.FIO;
            UIasd.PositionTxt.Text = Wrk.Position.PostionName;
            UIasd.ImageByte = LoadImage(Wrk.Photo);
            if (SenderMail.PositionName == "Менеджер по персонал")
            {
                BtnAll.Visibility = Visibility.Collapsed;
                BtnEquip.Visibility = Visibility.Collapsed;
                BtnWorkers.Visibility = Visibility.Visible;
                SenderMail.DelVisibility = Visibility.Collapsed;
                LoadWorker();
            }
            else if (SenderMail.PositionName.Contains("Администратор"))
            {
                BtnAll.Visibility = Visibility.Visible;
                BtnEquip.Visibility = Visibility.Visible;
                BtnWorkers.Visibility = Visibility.Visible;
                SenderMail.DelVisibility = Visibility.Visible;
            }
            else
            {
                BtnAll.Visibility = Visibility.Visible;
                BtnEquip.Visibility = Visibility.Visible;
                BtnWorkers.Visibility = Visibility.Collapsed;
                SenderMail.DelVisibility = Visibility.Collapsed;
            }
            if (Wrk.CheckFirstVisit == true)
            {
                PasswordWindow psd = new PasswordWindow();
                psd.ShowDialog();
            }
        }
        /// <summary>
        /// Блок преобразование картинки из Bitmap
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
        /// <summary>
        /// Блок обработки возрата на предыдущую страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.GoBack();
        }

        /// <summary>
        /// Блок обработки видимости кнопки возращения назад
        /// </summary>
        private void FrameMW_ContentRendered(object sender, EventArgs e)
        {
            if (FrameManager.MainFrame.CanGoBack)
            {
                BtnBack.Visibility = Visibility.Visible;
            }
            else
            {
                BtnBack.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        /// Блоки загрузки данных в элементы страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEquip_Click(object sender, RoutedEventArgs e)
        {
            BtnPage1.Content = "Оборудование";
            BtnPage2.Content = "Ремонт";
            BtnPage3.Visibility = Visibility.Hidden;
            BtnPage1.Click -= new RoutedEventHandler(Load3);
            BtnPage1.Click -= new RoutedEventHandler(Load5);
            BtnPage1.Click += new RoutedEventHandler(Load1);
            BtnPage2.Click -= new RoutedEventHandler(Load4);
            BtnPage2.Click -= new RoutedEventHandler(Load6);
            BtnPage2.Click += new RoutedEventHandler(Load2);
            BtnPage2.Click -= new RoutedEventHandler(Load7);
            FrameManager.MainFrame.Navigate(new EquipmentPage());
        }
        public void BtnPage3_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new InventarizationPage());
        }
        public void Load1(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EquipmentPage());
        }

        public void Load2(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new RepairPage());
        }
        public void Load3(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new WorkerPage());
        }
        public void Load4(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new WorkerOperationsHistory());
        }

        public void Load5(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new RoomPage());
        }
        public void Load6(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new NomenclaturePage());
        }
        public void Load7(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new InventarizationPage());
        }

        public void LoadWorker()
        {
            BtnPage1.Content = "Работники"; FrameManager.MainFrame.Navigate(new WorkerPage());
            BtnPage2.Content = "История";
            BtnPage3.Visibility = Visibility.Hidden;
            BtnPage1.Click += new RoutedEventHandler(Load3);
            BtnPage1.Click -= new RoutedEventHandler(Load5);
            BtnPage1.Click -= new RoutedEventHandler(Load1);
            BtnPage2.Click += new RoutedEventHandler(Load4);
            BtnPage2.Click -= new RoutedEventHandler(Load6);
            BtnPage2.Click -= new RoutedEventHandler(Load2);
            BtnPage2.Click -= new RoutedEventHandler(Load7);

            FrameManager.MainFrame.Navigate(new WorkerPage());
        }
        private void BtnWorkers_Click(object sender, RoutedEventArgs e)
        {
            LoadWorker();
        }

        private void BtnAll_Click(object sender, RoutedEventArgs e)
        {
            BtnPage1.Content = "Помещения";
            BtnPage2.Content = "Номенклатура";
            BtnPage3.Content = "Инвентаризация";
            BtnPage3.Visibility = Visibility.Visible; FrameManager.MainFrame.Navigate(new RoomPage());
            BtnPage1.Click -= new RoutedEventHandler(Load3);
            BtnPage1.Click += new RoutedEventHandler(Load5);
            BtnPage1.Click -= new RoutedEventHandler(Load1);
            BtnPage2.Click -= new RoutedEventHandler(Load4);
            BtnPage2.Click += new RoutedEventHandler(Load6);
            BtnPage2.Click -= new RoutedEventHandler(Load2);
            BtnPage3.Click += new RoutedEventHandler(Load7);
        }


       
    }
}
