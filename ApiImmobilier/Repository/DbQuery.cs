using ApiImmobilier.IRepository;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiImmobilier.Repository
{
    public class DbQuery : IDbQuery
    {
        private readonly string cnnxString;
        public DbQuery(string _cnnxString) => cnnxString = _cnnxString;

        public async Task<string> GetData(string sql, Action<NpgsqlCommand> callBack)
        {

            var con = new NpgsqlConnection(cnnxString);
            try
            {
                var cmd = new NpgsqlCommand(sql, con);

                callBack(cmd);

                await con.OpenAsync();
                var jsonResult = new StringBuilder();
                var reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    jsonResult.Append(reader.GetValue(0).ToString());
                }
                con.Close();

                var temoin = jsonResult.ToString();

                return jsonResult.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }

            finally
            {
                con.Dispose();
            }



        }

        public async Task<string> GetDataNested(string sql, Action<NpgsqlCommand> callBack)
        {

            var con = new NpgsqlConnection(cnnxString);
            try
            {
                var cmd = new NpgsqlCommand($@" select row_to_json(e) 
                                                from({sql}) as e", con);
                callBack(cmd);

                await con.OpenAsync();
                var jsonResult = new StringBuilder();
                var reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    jsonResult.Append(reader.GetValue(0).ToString());
                }
                con.Close();

                var temoin = jsonResult.ToString();

                return jsonResult.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }

            finally
            {
                con.Dispose();
            }

        }

        public async Task<string> GetDataNested(string sql)
        {
            var con = new NpgsqlConnection(cnnxString);
            try
            {
                var cmd = new NpgsqlCommand($@" select row_to_json(e) 
                                                from({sql}) as e", con);

                await con.OpenAsync();
                var jsonResult = new StringBuilder();
                var reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    jsonResult.Append(reader.GetValue(0).ToString());
                }
                con.Close();

                var temoin = jsonResult.ToString();

                return jsonResult.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }

            finally
            {
                con.Dispose();
            }
        }

        public async Task<string> GetData(string sql)
        {

            var con = new NpgsqlConnection(cnnxString);

            try
            {
                var cmd = new NpgsqlCommand(sql, con);

                await con.OpenAsync();
                var jsonResult = new StringBuilder();
                var reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    jsonResult.Append(reader.GetValue(0).ToString());
                }
                con.Close();

                var temoin = jsonResult.ToString();

                return jsonResult.ToString();
            }
            catch (Exception)
            {
                return null;
            }

            finally
            {
                con.Dispose(); // Vider le garbage collector
            }



        }

        public async Task<bool> PostData(string sql, Action<NpgsqlCommand> callBack)
        {

            var con = new NpgsqlConnection(cnnxString);
            // SqlTransaction tx = con.BeginTransaction();
            var cmd = new NpgsqlCommand(sql, con);
            callBack(cmd);
            try
            {
                //  cmd.Transaction = tx;
                await con.OpenAsync();
                int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                // tx.Commit();
                return true;
            }
            catch (Exception ex)
            {
                // tx.Rollback();
                return false;
            }

            finally
            {
                con.Dispose(); // Vider le garbage collector
            }


        }

        public async Task<bool> PostData(string sql)
        {

            var con = new NpgsqlConnection(cnnxString);
            // SqlTransaction tx = con.BeginTransaction();
            var cmd = new NpgsqlCommand(sql, con);

            try
            {
                //  cmd.Transaction = tx;
                await con.OpenAsync();
                int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                // tx.Commit();
                return true;
            }
            catch (Exception ex)
            {
                // tx.Rollback();
                return false;
            }

            finally
            {
                con.Dispose(); // Vider le garbage collector
            }


        }
    }
}
