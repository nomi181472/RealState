using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.models.ViewModels.VMModels
{
   public class ListPlotsToAnyone
    {
        public List<Photo> Photos { get; set; }
        public Plot  _Plot { get; set; }
        public bool IsVerified { get; set; }
       
    }
}
