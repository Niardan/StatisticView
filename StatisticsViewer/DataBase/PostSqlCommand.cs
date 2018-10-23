using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Npgsql;

namespace StatisticsViewer.DataBase
{
    public class PostSqlCommand
    {
        private bool _process;
        private Task<DbDataReader> _task;
        private string _connParam;

        public PostSqlCommand(string connParam)
        {
            _connParam = connParam;
        }

        public void ExecuteSqlNoQuery(string sql)
        {
            var con = new NpgsqlConnection(_connParam);
            con.Open();
            NpgsqlCommand com = new NpgsqlCommand(sql, con);
            com.ExecuteNonQuery();
            con.Close();
        }
        
        public async Task<List<string[]>> ExecuteSql(string sql)
        {
            var con = new NpgsqlConnection(_connParam);
            _process = true;
            con.Open();
            NpgsqlCommand com = new NpgsqlCommand(sql, con);
            var result = await com.ExecuteReaderAsync();
            List<string[]> strings = new List<string[]>();
            var listcount = result.FieldCount;

            while (result.Read())
            {
                var item = new string[listcount];
                for (int i = 0; i < listcount; i++)
                {
                    item[i] = result[i].ToString();
                }

                strings.Add(item);
            }
            con.Close();
            _process = false;
            return strings;
        }
    }
}