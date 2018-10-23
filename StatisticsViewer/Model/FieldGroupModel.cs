using System;

namespace StatisticsViewer.Model
{
    [Serializable]
    public class FieldGroupModel
    {
        public FieldGroupModel(string fileldName)
        {
            FileldName = fileldName;
        }

        public string FileldName { set; get; }

        public bool Enabled { set; get; }
    }
}