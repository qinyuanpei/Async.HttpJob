using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AsyncHttpJob.Models
{
    [Serializable]
    [DataContract]
    public class HttpJobDescriptor
    {
        [DataMember]
        public string Corn { get; set; }
        [DataMember]
        public string HttpUrl { get; set; }
        [DataMember]
        public string JobName { get; set; }
        [DataMember]
        public string HttpMethod { get; set; }
        [DataMember]
        public int? DelayInMinute { get; set; } = 5;
        [DataMember]
        public Dictionary<string, string> JobParameter { get; set; }
    }
}
