using System.Collections.ObjectModel;

namespace StatisticsViewer.Utils
{
    public class FieldsValue
    {
        private readonly ObservableCollection<string> _typeCompasion;
        private readonly ObservableCollection<string> _fieldName;

        public FieldsValue()
        {
            _typeCompasion = new ObservableCollection<string> { "=", "!=", ">", "<" }; 
            _fieldName = new ObservableCollection<string> { "short_message", "room", "process", "full_message", "environment", "app_name", "version" }; 
        }

        public ObservableCollection<string> TypeCompasion
        {
            get { return _typeCompasion; }
        }

        public ObservableCollection<string> FieldName
        {
            get { return _fieldName; }
        }
    }
}