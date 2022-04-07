using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.models.ViewModels
{
   public class Plot
    {
        public int PlotId { get; set; }
        public int SocietyId { get; set; }
        public double Area { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(SocietyId))]
        public Society SocietyTBL { get; set; }


    }
}
