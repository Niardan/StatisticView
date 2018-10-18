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
    class PostSql
    {
        private NpgsqlConnection _con;
        private ConverterMessage _converter = new ConverterMessage();
        public PostSql(string host, int port, string login, string password, string database)
        {
            string connParam = $"Server={host};Port={port};User Id={login};Password={password};Database={database};";

            _con = new NpgsqlConnection(connParam);


        }

        public void CreateTable(string tableName)
        {

            _con.Open();
            string sqlCreate =
                $"CREATE TABLE \"public\".\"{tableName}\"(\"id\" serial8, \"version\" varchar(10),\"timestamp\" int8,\"short_message\" varchar(50), \"room\" varchar(50), \"process\" int8, \"level\" varchar(20), \"host\" varchar(20), \"full_message\" varchar(100), \"environment\" varchar(20), \"elapsed\" float8, \"app_name\" varchar(20));";
            NpgsqlCommand com = new NpgsqlCommand(sqlCreate, _con);

            com.ExecuteNonQuery();
            string sql = $"ALTER TABLE \"public\".\"{tableName}\" ADD PRIMARY KEY (\"id\");";
            com = new NpgsqlCommand(sql, _con);
            com.ExecuteNonQuery();
            _con.Close();
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

        public async Task<List<PercentileData>> GetPercentile(string tablename, ICollection<FieldGroupModel> fieldnames, DateTime start, DateTime end, ICollection<FieldFilterModel> filters, IList<double> listPercentile)
        {
            long startStamp = (long)(start.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            long endStamp = (long)(end.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            StringBuilder builder = new StringBuilder("SELECT ");
            foreach (var field in fieldnames)
            {
                if (field.Enabled)
                {
                    builder.Append(field.FileldName + ", ");
                }
            }

            foreach (var percentile in listPercentile)
            {
                builder.Append(
                    $"percentile_cont({percentile.ToString(CultureInfo.InvariantCulture)}) within group(order by elapsed asc) as percentile_{(int)(percentile * 100)}, ");
            }


            builder.Append("count(elapsed), sum(elapsed), min(timestamp), max(timestamp) ");

            builder.Append($"FROM \"public\".\"{tablename}\" ");
            builder.Append("WHERE ");
            builder.Append($"timestamp >{startStamp} AND timestamp<{endStamp} ");
            if (filters != null)
            {

                foreach (var filter in filters)
                {
                    if (filter.Enabled)
                    {
                        builder.Append($"{filter.TypeAdd} {filter.FilterName}{filter.Type}'{filter.Filter}' ");
                    }
                }
            }


            builder.Append("GROUP BY ");
            foreach (var field in fieldnames)
            {
                if (field.Enabled)
                {
                    builder.Append(field.FileldName + ", ");
                }

            }
            builder.Remove(builder.Length - 2, 1);
            var table = await GetTable(builder.ToString());

            return _converter.GetPercentileModels(fieldnames, listPercentile, table);
        }

        private async Task<List<string[]>> GetTable(string sql)
        {
            _con.Open();
            NpgsqlCommand com = new NpgsqlCommand(sql, _con);

            var rezult = await com.ExecuteReaderAsync();

            List<string[]> strings = new List<string[]>();
            var listcount = rezult.FieldCount;
            while (rezult.Read())
            {
                var item = new string[listcount];
                for (int i = 0; i < listcount; i++)
                {
                    item[i] = rezult[i].ToString();

                }
                strings.Add(item);
            }
            _con.Close();
            return strings;
        }
    }
}
