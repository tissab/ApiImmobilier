using ApiImmobilier.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiImmobilier.IRepository
{
    public interface IEntityRepository<Model> where Model : BaseEntity
    {
        Task<bool> Save(Model model);
        Task<bool> Update(Guid guid, Model model);
        Task<bool> Update(Guid guid, Func<string> callBack);
        Task<bool> Delete(Guid guid);
        Task<string> GetModel(Guid guid);
        Task<string> GetByGuidModel(Guid guid);
    }
}
