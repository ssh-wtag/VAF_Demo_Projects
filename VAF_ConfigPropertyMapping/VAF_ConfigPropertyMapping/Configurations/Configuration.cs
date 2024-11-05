using MFiles.VAF.Configuration;
using System.Runtime.Serialization;

namespace VAF_ConfigPropertyMapping.Configurations
{
    [DataContract]
    public class Configuration
    {
        [DataMember]
        [JsonConfEditor(DefaultValue = false)]
        public bool Accepted { get; set; } = false;

    }
}