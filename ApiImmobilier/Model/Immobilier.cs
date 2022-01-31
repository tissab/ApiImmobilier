using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiImmobilier.Model
{
    public class Immobilier : BaseEntity
    {
        public string Imagename { get; set; }
        public string ImageSrc { get; set; }
        public string Description { get; set; }
        public string Titre { get; set; }
    }
}
