using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
        private FieldsValue _fieldsValue;
        public AddConditionWindow(FieldsValue fieldsValue) : this(fieldsValue, null, null, null)
        {
        }

        public AddConditionWindow(FieldsValue fieldsValue, string filedName, string compasion, string text)
        {
            InitializeComponent();
            _fieldsValue = fieldsValue;
            foreach (var item in _fieldsValue.FieldsName)
            {
                FieldName.Items.Add(item);
            }

            foreach (var item in _fieldsValue.TypeCompasion)
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
                _text = $"'{FieldText.Text}'";
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
            _fieldName = ((ComboBox)sender).SelectedItem.ToString();
            FieldText.Items.Clear();
            FieldText.Text = null;
            if (!_fieldsValue.GetFieldValue(_fieldName, Collection))
            {
                LoadField.Visibility = Visibility.Visible;
            }
        }

        private void Collection(ICollection<string> collection)
        {
            LoadField.Visibility = Visibility.Hidden;
            ;
            foreach (var item in collection)
            {
                FieldText.Items.Add(item);
            }
        }

        private void TypeComparison_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _compasionName = ((ComboBox)sender).SelectedItem.ToString();
        }

        private void FieldText_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _text = ((ComboBox)sender).SelectedItem?.ToString();
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

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _fieldsValue.CloseHandler();
        }
    }
}
