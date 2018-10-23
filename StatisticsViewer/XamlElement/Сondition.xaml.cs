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

    public delegate void ConditionHandler(Condition sender);
    public partial class Condition : UserControl
    {
        private double _time;
        private string _text;
        private FieldsValue _fieldsValue;

        public event ConditionHandler Selected;
        public event ConditionHandler Deselected;
        public event ConditionHandler Deleted;
        public event ConditionHandler Changed;
        
        SolidColorBrush brushSelect = new SolidColorBrush();
        SolidColorBrush brushUnselect = new SolidColorBrush();

        public Condition(string text, FieldsValue fieldsValue)
        {
            InitializeComponent();
            _text = text;
            _fieldsValue = fieldsValue;
            Filter.Content = _text;


            brushSelect.Color = Colors.Black;
            brushSelect.Opacity = 0.2;

            brushUnselect.Color = Colors.White;
            brushUnselect.Opacity = 0.4;
        }

        public BlockControl ParentBlock { get; set; }

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

                var form = new AddConditionWindow(_fieldsValue, array[0], array[1], array[2]);
                if (form.ShowDialogCondition(out var text))
                {
                    _text = text;
                    Filter.Content = _text;
                    Changed?.Invoke(this);
                }
            }

            Select();
            _time = currentTime;
        }

        public void Select()
        {
            SelectedBorder.BorderBrush = brushSelect;
            Selected?.Invoke(this);
        }

        public void Deselect()
        {
            SelectedBorder.BorderBrush = brushUnselect;
            Deselected?.Invoke(this);
        }

        public void Delete()
        {
            Deleted?.Invoke(this);
        }
    }
}
