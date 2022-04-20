using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace realstate.models.ViewModels.VMModels
{
    
	//DataContract for Serializing Data - required to serve in JSON format
	[DataContract]
	public class AreaGraph
	{
        [DataMember(Name = "label")]
        public string label { get; set; }
        [DataMember(Name = "y")]
        public Nullable<double> y = null;
        public AreaGraph(string label,double y)
        {
            this.label= label;
            this.y = y;
        }

    }
}
