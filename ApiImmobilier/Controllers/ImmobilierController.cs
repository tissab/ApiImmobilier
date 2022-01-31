using ApiImmobilier.Hubs;
using ApiImmobilier.IRepository;
using ApiImmobilier.Model;
using ApiImmobilier.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiImmobilier.Controllers
{
    [ApiController]
    public class ImmobilierController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private readonly IDbQuery _dbQuery;
        private readonly IEntityRepository<Immobilier> _repository;
        private Guid _Utilisateur_Id;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly FileManager _fileManager;
        public ImmobilierController(IConfiguration configuration, IHubContext<NotificationHub> hubContext, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            _repository = new EntityRepository<Immobilier>(Configuration.GetConnectionString("ReactDb"));
            _dbQuery = new DbQuery(Configuration.GetConnectionString("ReactDb"));
            _hostEnvironment = hostEnvironment;

            _fileManager = new FileManager(_hostEnvironment);
            _hubContext = hubContext;
        }

        [HttpDelete]
        [Route("api/DeleteImmobilier")]
        public async Task<bool> DeleteModel(Guid guid)
        {
            var response = await _repository.Delete(guid);
            if (response)
            {
                await _hubContext.Clients.All.SendAsync("UpdateData");
                return true;
            }
            return false;
        }

        [HttpGet]
        [Route("api/GetListImmobilier")]
        public async Task<string> GetListTmodel()
        {
                var response = await _dbQuery.GetData($@"select array_to_json(array_agg(i)) 
                                                        from (select 
	                                                          i.jsondata->>'Entity_id' as ""Entity_id"",
	                                                          i.jsondata->>'Description' as ""Description"",
                                                              i.jsondata->>'Imagename' as ""Imagename"",
	                                                          i.jsondata->>'Titre' as ""Titre"",
                                                              i.jsondata->>'DateCrea' as ""DateCrea""
 	                                                          from Immobilier i
                                                              order by ""DateCrea"") i");
            return response;
        }

        [HttpGet]
        [Route("api/GetImmobilier")]
        public async Task<string> GetTmodel(Guid guid)
        {

            var res = await _repository.GetModel(guid);

            return res;
        }

        [HttpPost]
        [Route("api/PostImmobilier")]
        public async Task<bool> PostModel([FromForm] EntityFile entityFile)
        {
            var data = JsonConvert.DeserializeObject<Immobilier>(entityFile.Stringify);

            if (entityFile.file != null)
            {
                var NameFile = await _fileManager.SaveImage(entityFile.file);
                var PathFile = string.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, NameFile);

                data.Imagename = NameFile;
                data.ImageSrc = PathFile;
            }

            var model = data;
            model.DateCrea = DateTime.Now;
           
            var res = await _repository.Save(model);

            if (res)
            {
                await _hubContext.Clients.All.SendAsync("UpdateData");
            }

            return res;
        }


        [HttpPut]
        [Route("api/UpdateImmobilier")]
        public async Task<bool> UpdateModel(Guid guid, [FromForm] EntityFile entityFile)
        {
            var model = JsonConvert.DeserializeObject<Immobilier>(entityFile.Stringify);

            var response = await _repository.GetByGuidModel(guid);

            var objUpdate = JsonConvert.DeserializeObject<Immobilier>(response);

            model.DateModif = DateTime.Now;

            objUpdate.Titre = model.Titre;
            objUpdate.Description = model.Description;
            objUpdate.Entity_id = guid;

            if (entityFile.file != null)
            {
                var existFile = objUpdate.Imagename;

                if (existFile is not null)
                {
                    _fileManager.DeleteImage(objUpdate.Imagename);
                }

                var NameFile = await _fileManager.SaveImage(entityFile.file);
                model.Imagename = NameFile;

                var PathFile = string.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, NameFile);
                model.ImageSrc = PathFile;

                objUpdate.Imagename = model.Imagename;
                objUpdate.ImageSrc = model.ImageSrc;
            }

            var res = await _repository.Update(guid, objUpdate);

            if (res)
            {
                await _hubContext.Clients.All.SendAsync("UpdateData");
            }

            return res;

        }
    }
}
