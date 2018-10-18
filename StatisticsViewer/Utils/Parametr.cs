using System;
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
            _parametrModel.Filters = "";
            _parametrModel.GroupFields = new ObservableCollection<FieldGroupModel>
                {
                    new FieldGroupModel("short_message"),
                    new FieldGroupModel("room"),
                    new FieldGroupModel("process"),
                    new FieldGroupModel("full_message"),
                    new FieldGroupModel("environment"),
                    new FieldGroupModel("app_name"),
                    new FieldGroupModel("version")
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
        get { return _parametrModel.StartDate; }
    }

    public ObservableCollection<FieldGroupModel> GroupFields
    {
        set
        {
            _parametrModel.GroupFields = value;
            SaveParametr();
        }
        get { return _parametrModel.GroupFields; }
    }

    public DateTime EndDate
    {
        set
        {
            _parametrModel.EndDate = value;
            SaveParametr();
        }
        get { return _parametrModel.EndDate; }
    }

    public string Filter
    {
        set
        {
            _parametrModel.Filters = value;
            SaveParametr();
        }
        get { return _parametrModel.Filters; }
    }

}

[Serializable]
public class ParametrModel
{
    public DateTime StartDate;
    public DateTime EndDate;
    public ObservableCollection<FieldGroupModel> GroupFields;
    public string Filters;
}