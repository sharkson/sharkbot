using System;

namespace ChatModels
{
    [Serializable]
    public class UserProperty
    {
        public UserProperty()
        {
            name = string.Empty;
            value = string.Empty;
            source = string.Empty;
        }

        public string name { get; set; }
        public string value { get; set; }
        public string source { get; set; }
    }
}