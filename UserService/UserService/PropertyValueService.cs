using ChatModels;
using System.Linq;

namespace UserService
{
    public class PropertyValueService
    {
        public UserProperty getPropertyByValue(string property, UserData userData)
        {
            var match = userData.properties.Where(p => p.name == property && p.source == userData.userName).LastOrDefault();
            if (match == null)
            {
                match = userData.properties.Where(p => p.name == property).LastOrDefault();
            }
            if (match == null)
            {
                return new UserProperty();
            }
            return match;
        }

        public UserProperty getSelfPropertyByValue(string propertyValue, UserData userData)
        {
            var match = userData.properties.Where(p => p.name == "self" && p.value == propertyValue && p.source == userData.userName).LastOrDefault();
            if (match == null)
            {
                match = userData.properties.Where(p => p.name == "self" && p.value == propertyValue).LastOrDefault();
            }
            if (match == null)
            {
                return new UserProperty();
            }
            return match;
        }
    }
}
