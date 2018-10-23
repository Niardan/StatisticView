using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using StatisticsViewer.Model;

namespace StatisticsViewer.XamlElement
{
    /// <summary>
    /// Логика взаимодействия для DataGridPercentileTable.xaml
    /// </summary>
    public partial class DataGridPercentileTable : UserControl
    {
        public DataGridPercentileTable()
        {
            InitializeComponent();
        }

        public void SetSource(ObservableCollection<PercentileData> data)
        {
            MainTable.Dispatcher.BeginInvoke(new Action(() =>
            {
                MainTable.ItemsSource = data;
            }));
        }

        public void GenTable(Parametr parametr)
        {
            foreach (var collumn in MainTable.Columns)
            {
                var dict = parametr.GroupFields;
                if (collumn is ContextDataGridTextCollumn contextCollumn)
                {
                    contextCollumn.Visibility = dict[contextCollumn.Context] ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        public PercentileData SelectedItem
        {
            get { return MainTable.SelectedItem as PercentileData;}
        }
    }
}
