using System;

namespace StatisticsViewer.Model
{
    public class FieldFilterModel
    {
        public event Action ChangeValue;
        private string _typeAdd;
        private string _filter;
        private string _type;
        private bool _enabled;
        public FieldFilterModel(string filterName)
        {
            FilterName = filterName;
        }

        public string FilterName { set; get; }
        public string Filter
        {
            set
            {
                _filter = value;
                ChangeValue?.Invoke();
            }
            get { return _filter; }

        }
        public string Type
        {
            set
            {
                _type = value;
                ChangeValue?.Invoke();
            }
            get { return _type; }

        }
        public bool Enabled
        {
            set
            {
                _enabled = value;
                ChangeValue?.Invoke();
            }
            get { return _enabled; }

        }

        public string TypeAdd
        {
            get { return _typeAdd; }
            set { _typeAdd = value; }
        }
    }
}