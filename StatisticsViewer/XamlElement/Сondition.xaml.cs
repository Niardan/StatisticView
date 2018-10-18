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

namespace StatisticsViewer.XamlElement
{
    /// <summary>
    /// Логика взаимодействия для condition.xaml
    /// </summary>
    public partial class Condition : UserControl
    {
        private double _time;
        private string _text;
        public Condition(string text)
        {
            InitializeComponent();
            _text = text;
            Filter.Content = _text;
        }

        public string GetFilter()
        {
            return _text;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currentTime = Time.Get;
            if (currentTime - _time < 500)
            {
                var array = _text.Split(' ');

                var form = new AddConditionWindow(array[0], array[1], array[2]);
                string text;
                if (form.ShowDialogCondition(out text))
                {
                    _text = text;
                    Filter.Content = _text;
                }
            }

            _time = currentTime;
        }
    }
}
