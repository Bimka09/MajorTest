using MajorTest.Model;
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

namespace MajorTest.View
{
    /// <summary>
    /// Логика взаимодействия для EditOrderView.xaml
    /// </summary>
    public partial class EditOrderView : UserControl
    {
        private SolidColorBrush orangeColor = new SolidColorBrush(Colors.Orange);
        public EditOrderView()
        {
            InitializeComponent();
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            OrderDataTable.SelectedItem = ((OrderData)e.Row.Item).Id;
            e.Row.Background = orangeColor;
        }

        private void DataGrid_PreparingCellForEdit_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Column.Header.ToString() == "Обновлено") return;
            var status = ((OrderData)e.Row.Item).Status;
            if (status != "Новая")
            {
                MessageBox.Show("Нельзя редактировать заявки, статус которых отличен от \"Новая\"", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Cancel = true;
            }
        }
    }
}
