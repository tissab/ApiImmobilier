using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiImmobilier.IRepository
{
    public interface IDbQuery
    {
        Task<string> GetData(string sql, Action<NpgsqlCommand> callBack);
        Task<string> GetDataNested(string sql, Action<NpgsqlCommand> callBack);
        Task<string> GetData(string sql);
        Task<string> GetDataNested(string sql);
        Task<bool> PostData(string tmodel, Action<NpgsqlCommand> callBack);
        Task<bool> PostData(string tmodel);
    }
}
