using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using StatisticsViewer.DataBase;
using StatisticsViewer.Model;
using StatisticsViewer.Utils;
using StatisticsViewer.XamlElement;
using Condition = StatisticsViewer.XamlElement.Condition;

namespace StatisticsViewer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Parametr _parametr;
        private readonly FilterController _filterController;
        private readonly AsyncBaseLoader _loader;
        private readonly PostSql _postSql;
        private readonly string _table = "ldoe";

        private Condition _selected;

        public MainWindow()
        {
            InitializeComponent();
            _parametr = new Parametr();
            _postSql = new PostSql("192.168.1.55", 5432, "postgres", "AASSxxzz1", "postgres");
            _loader = new AsyncBaseLoader(_postSql);
            _filterController = new FilterController(_parametr, FiltersBloks, _loader, _table);
            _filterController.SelectCondition += FilterControllerOnSelectCondition;
            _filterController.DeselectCondition += FilterControllerOnDeselectCondition;
            GroupPanel.Children.Add(new GroupBoxElement(_parametr));
            StartDate.Value = _parametr.StartDate;
            EndDate.Value = _parametr.EndDate;
            SavedRows.SetSource(_parametr.SavedRows);
        }

        private void FilterControllerOnDeselectCondition(Condition sender)
        {
            if (_selected != null && Equals(_selected, sender))
            {
                _selected = null;
            }
        }

        private void FilterControllerOnSelectCondition(Condition sender)
        {
            if (_selected != null && !Equals(_selected, sender))
            {
                _selected.Deselect();
            }

            _selected = sender;
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
            MainTable.GenTable(_parametr);
            LoadIndicator.Visibility = Visibility.Visible;
            _loader.AsyncGetPercentileList(_table, StartDate.Value ?? DateTime.Now, EndDate.Value ?? DateTime.Now, _parametr, PercentileCallback);
            _filterController.ApplyFilter();
        }

        private void PercentileCallback(List<PercentileData> data)
        {
            MainTable.Dispatcher.BeginInvoke(new Action(() =>
            {
                MainTable.SetSource(new ObservableCollection<PercentileData>(data));
                LoadIndicator.Visibility = Visibility.Hidden;
            }));
            LoadIndicator.Dispatcher.BeginInvoke((Action)(() => LoadIndicator.Visibility = Visibility.Hidden));
        }

        private void DeleteFilter_OnClick(object sender, RoutedEventArgs e)
        {
            _selected?.Delete();
        }

        private void StartDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _parametr.StartDate = StartDate.Value ?? DateTime.Now;
        }

        private void EndDate_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _parametr.EndDate = EndDate.Value ?? DateTime.Now;
        }


        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = MainTable.SelectedItem;
            if (selectedItem != null)
            {
                _parametr.AddSavePercentile(selectedItem);
            }
        }

        private void DeleteFromSaved_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = SavedRows.SelectedItem;
            if (selectedItem != null)
            {
                _parametr.RemoveSavePercentile(selectedItem);
            }
        }
    }
}
