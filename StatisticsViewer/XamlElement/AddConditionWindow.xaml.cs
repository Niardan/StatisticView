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
using System.Windows.Shapes;
using StatisticsViewer.Utils;

namespace StatisticsViewer.XamlElement
{
    /// <summary>
    /// Логика взаимодействия для AddConditionWindow.xaml
    /// </summary>
    public partial class AddConditionWindow : Window
    {
        private string _fieldName;
        private string _compasionName;
        private string _text;

        public AddConditionWindow() : this(null, null, null)
        {
        }
        public AddConditionWindow(string filedName, string compasion, string text)
        {
            InitializeComponent();
            var filedsValue = new FieldsValue();
            foreach (var item in filedsValue.FieldName)
            {
                FieldName.Items.Add(item);
            }
            foreach (var item in filedsValue.TypeCompasion)
            {
                TypeComparison.Items.Add(item);
            }

            if (!string.IsNullOrEmpty(filedName))
            {
                FieldName.SelectedValue = filedName;
            }
            else
            {
                FieldName.SelectedIndex = 0;
            }
            if (!string.IsNullOrEmpty(compasion))
            {
                TypeComparison.SelectedValue = compasion;
            }
            else
            {
                TypeComparison.SelectedIndex = 0;
            }

            if (!string.IsNullOrEmpty(text))
            {
                FieldText.Text = text;
            }

        }
        public bool ShowDialogCondition(out string text)
        {
            if (ShowDialog() == true)
            {
                _text = FieldText.Text;
                _fieldName = FieldName.Text;
                _compasionName = TypeComparison.Text;
                text = _fieldName + " " + _compasionName + " " + _text;
                return true;
            }

            text = null;
            return false;
        }

        private void FieldName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _fieldName = (string)((ComboBox)sender).SelectedItem;
        }

        private void TypeComparison_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _compasionName = (string)((ComboBox)sender).SelectedItem;
        }

        private void FieldText_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _text = (string)((ComboBox)sender).SelectedItem;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FieldText.Text) || string.IsNullOrEmpty(FieldName.Text) ||
                string.IsNullOrEmpty(TypeComparison.Text))
            {
                MessageBox.Show("Не все поля заполнены!");
                return;
            }
            this.DialogResult = true;
            Close();
        }
    }
}
