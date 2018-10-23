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

namespace StatisticsViewer.XamlElement
{
    /// <summary>
    /// Логика взаимодействия для ComboControll.xaml
    /// </summary>
    public partial class ComboControll : UserControl
    {
        public ComboControll()
        {
            InitializeComponent();
        } 

        public ComboControll(string value) : this()
        {
            FilterBox.Text = value;
        }

        public string GetFilter()
        {
            return FilterBox.Text;
        }
    }
}
