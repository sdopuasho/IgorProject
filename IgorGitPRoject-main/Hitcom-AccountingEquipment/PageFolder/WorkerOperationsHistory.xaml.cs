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
    /// Логика взаимодействия для WorkerOperationsHistory.xaml
    /// </summary>
    public partial class WorkerOperationsHistory : Page
    {
        /// <summary>
        ///Блок инициализации данных
        /// </summary>
        public WorkerOperationsHistory()
        {
            InitializeComponent();
            DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().OperationHystory.ToList();

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
                DgridMyPage.ItemsSource = AccountingEquipmentEntities.GetContext().OperationHystory.ToList();
            }
        }
    }
}
