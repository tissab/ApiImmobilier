using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiImmobilier.Model
{
    public class EntityFile
    {
        public IFormFile file { get; set; }
        public string Stringify { get; set; }
    }
}
