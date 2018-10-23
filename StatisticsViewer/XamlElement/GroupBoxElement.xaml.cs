using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace StatisticsViewer.XamlElement
{
    /// <summary>
    /// Логика взаимодействия для GroupBoxElement.xaml
    /// </summary>
    public partial class GroupBoxElement : UserControl
    {
        private Parametr _parametr;
        public GroupBoxElement(Parametr parametr)
        {
            _parametr = parametr;
            InitializeComponent();
            GenElements();
        }

        public void GenElements()
        {
            foreach (var field in _parametr.GroupFields)
            {
                var check = new CheckBox();
                check.Content = field.Key;
                check.IsChecked = field.Value;
                check.Checked += CheckOnChecked;
                check.Unchecked += CheckOnChecked;
                GroupCheck.Children.Add(check);
            }
        }

        private void CheckOnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox check)
            {
                _parametr.SetGroupFieldParam(check.Content as string, check.IsChecked ?? false);
            }
        }

    }
}
