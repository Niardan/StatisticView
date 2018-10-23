using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StatisticsViewer.DataBase;
using StatisticsViewer.Model;

namespace StatisticsViewer.Utils
{
    public delegate void StringCollectionFieldHandler(string fieldName, ICollection<string> collection);
    public delegate void StringCollectionHandler(ICollection<string> collection);
    public delegate void PercentileListHandler(List<PercentileData> data);

    public class AsyncBaseLoader
    {
        private readonly PostSql _postSql;

        public AsyncBaseLoader(PostSql postSql)
        {
            _postSql = postSql;
        }

        public async void AsyncGetFileldsValue(string fieldName, string tableName, StringCollectionFieldHandler filesCallback)
        {
            var result = await _postSql.GetUniqueValue(fieldName, tableName);
            filesCallback.Invoke(fieldName, result);
        }

        public async void AsyncGetPercentileList(string tablename, DateTime start, DateTime end, Parametr parametr, PercentileListHandler  percentileCallback)
        {
            var result = await _postSql.GetPercentile(tablename, start, end, parametr.GroupFields, parametr.Filter, parametr.ListPercentile);
            percentileCallback?.Invoke(result);
        }
    }
}