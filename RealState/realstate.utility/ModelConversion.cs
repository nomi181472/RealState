using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.utility
{
   public static class ModelConversion
    {
        public static T2 ModelConversionUsingJSON<T1,T2>(T1 value)
        {
          return  JsonConvert.DeserializeObject<T2>( JsonConvert.SerializeObject(value,new JsonSerializerSettings() { NullValueHandling= NullValueHandling .Ignore}));
        }
        public static List<T2> ModelConversionUsingJSONMultple<T1,T2>(List<T1> value)
        {
          return  JsonConvert.DeserializeObject<List<T2>>( JsonConvert.SerializeObject(value,new JsonSerializerSettings() { NullValueHandling= NullValueHandling .Ignore}));
        }
    }
}
