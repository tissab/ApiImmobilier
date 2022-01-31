using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiImmobilier.Model
{
    public class BaseEntity
    {
        public Guid? Entity_id { get; set; }
        public Guid? UtilCrea { get; set; }
        public Guid? UtilModif { get; set; }
        public DateTime? DateCrea { get; set; }
        public DateTime? DateModif { get; set; }
    }
}
