using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary3.DataContract
{
    [DataContract]
   public class config
    {
        [DataMember]
        public string Name;
    }
}
