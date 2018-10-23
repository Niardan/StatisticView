using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using StatisticsViewer.Model;
using ConverterMessage = StatisticsViewer.Utils.ConverterMessage;

namespace StatisticsViewer.DataBase
{
    public class PostSql
    {
        private readonly PostSqlCommand _command;
        private readonly ConverterMessage _converter = new ConverterMessage();

        public PostSql(string host, int port, string login, string password, string database)
        {
            string connParam = $"Server={host};Port={port};User Id={login};Password={password};Database={database};";

            _command = new PostSqlCommand(connParam);
        }

        public void CreateTable(string tableName)
        {

            string sqlCreate =
                $"CREATE TABLE \"public\".\"{tableName}\"(\"id\" serial8, \"version\" varchar(10),\"timestamp\" int8,\"short_message\" varchar(50), \"room\" varchar(50), \"process\" int8, \"level\" varchar(20), \"host\" varchar(20), \"full_message\" varchar(100), \"environment\" varchar(20), \"elapsed\" float8, \"app_name\" varchar(20));";
            _command.ExecuteSqlNoQuery(sqlCreate);
            string sql = $"ALTER TABLE \"public\".\"{tableName}\" ADD PRIMARY KEY (\"id\");";
            _command.ExecuteSqlNoQuery(sql);
        }

        public List<string> ListTables()
        {
            var listTable = new List<string>();
            string sql =
                "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE';";

            var table = GetTable(sql).Result;
            foreach (var row in table)
            {
                listTable.Add(row[0].ToString());
            }

            return listTable;
        }

        public async Task<List<PercentileData>> GetPercentile(string tablename,  DateTime start, DateTime end, IDictionary<string, bool> fields, string filters, ICollection<double> listPercentile)
        {
            long startStamp = (long)start.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            long endStamp = (long)end.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            StringBuilder builder = new StringBuilder("SELECT ");
            builder.Append(GetGroupFilter(fields));
            builder.Append(", ");
            foreach (var percentile in listPercentile)
            {
                builder.Append(
                    $"percentile_cont({percentile.ToString(CultureInfo.InvariantCulture)}) within group(order by elapsed asc) as percentile_{(int)(percentile * 100)}, ");
            }

            builder.Append("count(elapsed), sum(elapsed), min(timestamp), max(timestamp) ");

            builder.Append($"FROM \"public\".\"{tablename}\" ");
            builder.Append("WHERE ");
            builder.Append($"timestamp > {startStamp} AND timestamp < {endStamp}");
            if (filters != null)
            {
                builder.Append($" AND {filters}");
            }

            builder.Append(" GROUP BY ");
            builder.Append(GetGroupFilter(fields));

            var table = await GetTable(builder.ToString());
            return _converter.GetPercentileModels(fields, listPercentile, table);
        }

        public string GetGroupFilter(IDictionary<string, bool> fields)
        {
            var builder = new StringBuilder();
            bool first = true;
            foreach (var field in fields)
            {
                if (field.Value)
                {
                    if (first)
                    {
                        first = false;
                        builder.Append(field.Key);
                    }
                    else
                    {
                        builder.Append($", {field.Key}");
                    }
                }
            }
            return builder.ToString();
        }

        private async Task<List<string[]>> GetTable(string sql)
        {
            return await _command.ExecuteSql(sql);
        }

        public async Task<List<string>> GetUniqueValue(string filedName, string table)
        {
            var sql = $"SELECT DISTINCT {filedName} FROM \"public\".\"{table}\" "; 
            var result = await _command.ExecuteSql(sql);

            List<string> strings = new List<string>();

            foreach (var item in result)
            {
                strings.Add(item[0]);
            }
            return strings;
        }
    }
}
