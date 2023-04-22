using Models.Constants;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class BlockModel
    {
        [JsonProperty("id")]
        public int Id;
        
        [JsonProperty("subject")]
        public string Subject;
        
        [JsonProperty("grade")]
        public string Grade;

        [JsonProperty("mastery")]
        public BlockTypes Mastery;

        [JsonProperty("domainid")]
        public string DomainId;

        [JsonProperty("domain")]
        public string Domain;

        [JsonProperty("cluster")]
        public string Cluster;

        [JsonProperty("standardid")]
        public string StandardId;

        [JsonProperty("standarddescription")]
        public string StandardDescription;
    }
}
