using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using StatisticsViewer.Model;

[Serializable]
public class Parametr
{
    private ParametrModel _parametrModel;

    public Parametr()
    {
        LoadParametr();
    }

    private void ItemOnChangeValue()
    {
        SaveParametr();
    }

    private void LoadParametr()
    {

        BinaryFormatter serializer = new BinaryFormatter();
        try
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            using (Stream stream = new FileStream(path + "/config.ini", FileMode.Open))
            {
                _parametrModel = (ParametrModel)serializer.Deserialize(stream);
            }
        }
        catch
        {
            _parametrModel = new ParametrModel();
            _parametrModel.SavedRows = new ObservableCollection<PercentileData>();
            _parametrModel.Filters = "";
            _parametrModel.GroupFields = new Dictionary<string, bool>()
            {
                { "short_message",true },
                { "room",true },
                { "process",true },
                { "full_message",true },
                { "environment",true },
                { "app_name",true },
                { "version",true }
                };
        }
    }

    public void SaveParametr()
    {
        BinaryFormatter serializer = new BinaryFormatter();
        var path = System.AppDomain.CurrentDomain.BaseDirectory;
        using (Stream stream = new FileStream(path + "/config.ini", FileMode.Create))
        {
            serializer.Serialize(stream, _parametrModel);
        }
    }

    public DateTime StartDate
    {
        set
        {
            _parametrModel.StartDate = value;
            SaveParametr();
        }

        get
        {
            return _parametrModel.StartDate;
        }
    }

    public ICollection<double> ListPercentile
    {
        get { return new List<double> {0.1, 0.25, 0.50, 0.75, 0.9, 0.98, 1}; }
    }

    public IDictionary<string, bool> GroupFields
    {
        set
        {
            _parametrModel.GroupFields = value;
            SaveParametr();
        }

        get
        {
            return _parametrModel.GroupFields;
        }
    }

    public void SetGroupFieldParam(string fieldName, bool enabled)
    {
        _parametrModel.GroupFields[fieldName] = enabled;
        SaveParametr();
    }

    public DateTime EndDate
    {
        set
        {
            _parametrModel.EndDate = value;
            SaveParametr();
        }

        get
        {
            return _parametrModel.EndDate;
        }
    }

    public ObservableCollection<PercentileData> SavedRows
    {
        get { return _parametrModel.SavedRows; }
    }

    public void AddSavePercentile (PercentileData percentile)
    {
       _parametrModel.SavedRows.Add(percentile);
        SaveParametr();
    }

    public void RemoveSavePercentile(PercentileData percentile)
    {
        _parametrModel.SavedRows.Remove(percentile);
        SaveParametr();
    }

    public string Filter
    {
        set
        {
            _parametrModel.Filters = value;
            SaveParametr();
        }
        get
        {
            return _parametrModel.Filters;
        }
    }

}

[Serializable]
public class ParametrModel
{
    public DateTime StartDate;
    public DateTime EndDate;
    public IDictionary<string, bool> GroupFields;
    public string Filters;
    public ObservableCollection<PercentileData> SavedRows;
}