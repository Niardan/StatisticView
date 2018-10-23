using System;
using System.Collections.Generic;

namespace StatisticsViewer.Model
{
    [Serializable]
    public class PercentileData
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string version { get; set; }
        public string short_message { get; set; }
        public string room { get; set; }
        public string process { get; set; }
        public string full_message { get; set; }
        public string environment { get; set; }
        public string app_name { get; set; }
        public int count { get; set; }
        public double fulltime { get; set; }
        public double Percentile_10 { get; set; }
        public double Percentile_25 { get; set; }
        public double Percentile_50 { get; set; }
        public double Percentile_75 { get; set; }
        public double Percentile_90 { get; set; }
        public double Percentile_98 { get; set; }
        public double Percentile_100 { get; set; }
    }

    public class ConverterMessage
    {
        public List<PercentileData> GetPercentileModels(ICollection<FieldGroupModel> fileldNames, IList<double> listPercentile, IList<string[]> strings)
        {
            List<PercentileData> percentiles = new List<PercentileData>();
            foreach (var item in strings)
            {
                percentiles.Add(GetModel(fileldNames, listPercentile, item));
            }

            return percentiles;
        }

        private PercentileData GetModel(ICollection<FieldGroupModel> fileldNames, IList<double> listPercentile, string[] strings)
        {
            var percentile = new PercentileData();
            var dict = new Dictionary<string, string>();
            int count = 0;
            foreach (var fileldName in fileldNames)
            {
                if (fileldName.Enabled)
                {
                    dict.Add(fileldName.FileldName, strings[count]);
                    count++;
                }
            }

            foreach (var item in listPercentile)
            {
                dict.Add($"percentile_{ (int)(item * 100)}", strings[count]);
                count++;
            }
            dict.Add("count", strings[count]);
            count++;
            dict.Add("fulltime", strings[count]);
            count++;
            dict.Add("minTime", strings[count]);
            count++;
            dict.Add("maxTime", strings[count]);

            string value;
            if (dict.TryGetValue("version", out value))
                percentile.version = value;
            if (dict.TryGetValue("short_message", out value))
                percentile.short_message = value;
            if (dict.TryGetValue("room", out value))
                percentile.room = value;
            if (dict.TryGetValue("process", out value))
                percentile.process = value;
            if (dict.TryGetValue("full_message", out value))
                percentile.full_message = value;
            if (dict.TryGetValue("environment", out value))
                percentile.environment = value;
            if (dict.TryGetValue("app_name", out value))
                percentile.app_name = value;
            if (dict.TryGetValue("percentile_10", out value))
                percentile.Percentile_10 = Math.Round(Convert.ToDouble(value), 4);
            if (dict.TryGetValue("percentile_25", out value))
                percentile.Percentile_25 = Math.Round(Convert.ToDouble(value), 4);
            if (dict.TryGetValue("percentile_50", out value))
                percentile.Percentile_50 = Math.Round(Convert.ToDouble(value), 4);
            if (dict.TryGetValue("percentile_75", out value))
                percentile.Percentile_75 = Math.Round(Convert.ToDouble(value), 4);
            if (dict.TryGetValue("percentile_90", out value))
                percentile.Percentile_90 = Math.Round(Convert.ToDouble(value), 4);
            if (dict.TryGetValue("percentile_98", out value))
                percentile.Percentile_98 = Math.Round(Convert.ToDouble(value), 4);
            if (dict.TryGetValue("percentile_100", out value))
                percentile.Percentile_100 = Math.Round(Convert.ToDouble(value), 4);
            if (dict.TryGetValue("count", out value))
                percentile.count = Convert.ToInt32(value);
            if (dict.TryGetValue("fulltime", out value))
                percentile.fulltime = Math.Round(Convert.ToDouble(value), 2);
            if (dict.TryGetValue("minTime", out value))
                percentile.StartTime = UnixTimeStampToDateTime(value);
            if (dict.TryGetValue("maxTime", out value))
                percentile.EndTime = UnixTimeStampToDateTime(value);
            return percentile;

        }
        public DateTime UnixTimeStampToDateTime(string unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(Convert.ToDouble(unixTimeStamp)).ToLocalTime();
            return dtDateTime;
        }
    }
}