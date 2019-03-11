using System;

namespace ChatModels
{
    [Serializable]
    public class Triplets
    {
        public Subject subject { get; set; }
        public Predicate predicate { get; set; }
        public ObjectTriplet objectTriplet { get; set; }
    }
}