using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LearnFunctionApp
{
    public class MessageModel
    {
        [DataMember(Name ="sampleMessage")]
        public string SampleMessage { get; set; }
        [DataMember(Name = "makeDate")]
        public DateTime MakeDate { get; set; }
    }
}
