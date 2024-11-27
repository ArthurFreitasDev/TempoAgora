using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoAgora.Models;

namespace TempoAgora.Helpers
{
    public class HelpersTempo
    {
        readonly SQLiteAsyncConnection _conn;
        public HelpersTempo(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<HistoricoTempo>().Wait();
        }

        public Task<int> InsertTempo(HistoricoTempo a)
        {
            return _conn.InsertAsync(a);
        }
        public Task<List<HistoricoTempo>> GetAllTempo()
        {
            return _conn.Table<HistoricoTempo>().ToListAsync();
        }
    }
}
