using ApiImmobilier.IRepository;
using ApiImmobilier.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiImmobilier.Repository
{
    public class EntityRepository<Model> : IEntityRepository<Model> where Model : BaseEntity
    {
        private readonly DbQuery dbQuery;
        public EntityRepository(string cnx)
        {
            dbQuery = new DbQuery(cnx);
        }

        public async Task<bool> Save(Model model)
        {

            model.DateCrea = DateTime.Now;
            model.Entity_id = Guid.NewGuid();

            var modelSerialize = JsonConvert.SerializeObject(model);

            var res = await dbQuery.PostData($@"INSERT INTO {typeof(Model).Name} (Entity_id, jsondata) 
                                                    VALUES('{model.Entity_id}', $£Þ${modelSerialize}$£Þ$) ");

            return res;
        }

        public async Task<bool> Update(Guid guid, Model model)
        {
            string response = string.Empty;

            response = await GetByGuidModel(guid);

            var objUpdate = JsonConvert.DeserializeObject<Model>(response);

            objUpdate.Entity_id = guid;

            objUpdate.DateModif = DateTime.Now;

            objUpdate = model;

            var modelSerialize = JsonConvert.SerializeObject(objUpdate);

            var res = await dbQuery.PostData($@"update {typeof(Model).Name} e set jsondata = e.jsondata || $£Þ${modelSerialize}$£Þ$ 
                                                             where e.Entity_id = @entity_id", cmd => {
                cmd.Parameters.AddWithValue("@entity_id", guid);
            });

            return res;
        }

        public async Task<bool> Update(Guid guid, Func<string> callBack)
        {
            string response = string.Empty;

            response = await GetByGuidModel(guid);

            var modelSerialize = callBack();

            var res = await dbQuery.PostData($@"update {typeof(Model).Name} e set jsondata = e.jsondata || $£Þ${modelSerialize}$£Þ$ 
                                                             where e.Entity_id = @entity_id", cmd => {
                cmd.Parameters.AddWithValue("@entity_id", guid);
            });

            return res;
        }

        public async Task<bool> Delete(Guid guid)
        {

            var res = await dbQuery.PostData($@"DELETE FROM {typeof(Model).Name} WHERE 
                                 {typeof(Model).Name}.{typeof(Model).GetProperty("Entity_id").Name} = @entity_id", cmd =>
            {
                cmd.Parameters.AddWithValue("@entity_id", guid);
            });

            return res;
        }

        public async Task<string> GetModel(Guid guid)
        {
            var res = await dbQuery.GetDataNested($@"SELECT e.jsondata as  ""{typeof(Model).Name}""
                                                        FROM {typeof(Model).Name} e
                                                        WHERE e.Entity_id = @entity_id ", cmd => {
                cmd.Parameters.AddWithValue("@entity_id", guid);
            });

            return res;
        }

        public async Task<string> GetByGuidModel(Guid guid)
        {

            var response = await dbQuery.GetData($@"SELECT e.jsondata 
                                                         FROM {typeof(Model).Name} e
                                                         WHERE e.Entity_id = @entity_id", cmd => {
                cmd.Parameters.AddWithValue("@entity_id", guid);
            });


            return response;
        }
    }
}
