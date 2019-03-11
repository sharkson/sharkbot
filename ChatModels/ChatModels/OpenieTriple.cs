using System;
using System.Collections.Generic;
using System.Text;

namespace ChatModels
{
    [Serializable]
    public class OpenieTriple
    {
        public string Subject { get; set; }
        public string Object { get; set; }
        public string Relation { get; set; }
    }
}
