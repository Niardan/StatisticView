using System.Collections.Generic;
using System.Collections.ObjectModel;
using StatisticsViewer.XamlElement;

namespace StatisticsViewer.Utils
{
    public class FieldsValue
    {
        private readonly ICollection<string> _typeCompasion;
        private readonly ICollection<string> _fieldsName;
        private string _tableName;
        private readonly IDictionary<string, ICollection<string>> _fieldsValue = new Dictionary<string, ICollection<string>>();
        private StringCollectionHandler _fieldHandler;
        private string _fieldName;

        private readonly AsyncBaseLoader _asyncLoader;

        public FieldsValue(AsyncBaseLoader asyncLoader, string tableName)
        {
            _tableName = tableName;
            _asyncLoader = asyncLoader;
            _typeCompasion = new List<string> { "=", "!=", ">", "<" };
            _fieldsName = new List<string> { "short_message", "room", "process", "full_message", "environment", "app_name", "version" };
            foreach (var field in _fieldsName)
            {
                _asyncLoader.AsyncGetFileldsValue(field, _tableName, GetListHandler);
            }
        }

        private void GetListHandler(string fieldname, ICollection<string> collection)
        {
            _fieldsValue[fieldname] = collection;
            if (fieldname == _fieldName)
            {
                _fieldHandler?.Invoke(collection);
                _fieldName = null;
                _fieldHandler = null;
            }
        }

        public ICollection<string> TypeCompasion
        {
            get { return _typeCompasion; }
        }

        public ICollection<string> FieldsName
        {
            get { return _fieldsName; }
        }

        public bool GetFieldValue(string filedName, StringCollectionHandler collection)
        {
            if (_fieldsValue.ContainsKey(filedName))
            {
                collection.Invoke(_fieldsValue[filedName]);
                return true;
            }
            else
            {
                _fieldName = filedName;
                _fieldHandler = collection;
                return false;
            }
        }

        public void CloseHandler()
        {
            _fieldHandler = null;
            _fieldName = null;
        }
    }
}