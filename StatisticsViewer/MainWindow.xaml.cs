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
using StatisticsViewer.Utils;
using StatisticsViewer.XamlElement;


namespace StatisticsViewer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Parametr _parametr;
        private readonly FilterController _filterController;
        public MainWindow()
        {
            InitializeComponent();
            _parametr = new Parametr();

            _filterController = new FilterController(_parametr, FiltersBloks);
        }

        private void AddCondition_OnClick(object sender, RoutedEventArgs e)
        {
            _filterController.AddCondition();
        }
        
        private void OpenBlock_OnClick(object sender, RoutedEventArgs e)
        {
           _filterController.OpenBlock();
        }

        private void CloseBlock_OnClick(object sender, RoutedEventArgs e)
        {
           _filterController.CloseBlock();
        }


        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
          _filterController.ApplyFilter();
        }
    }
}
