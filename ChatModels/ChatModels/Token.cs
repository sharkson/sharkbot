using System;

namespace ChatModels
{
    [Serializable]
    public class Token
    {
        public string PosTag { get; set; }
        public string NerTag { get; set; }
        public string Lemmas { get; set; }
        public string Word { get; set; }
        public string IncomingDependencyLabel { get; set; }
        public int? Governor { get; set; }
    }
}