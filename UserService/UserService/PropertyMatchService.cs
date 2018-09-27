using ChatModels;
using System.Text.RegularExpressions;

namespace UserService
{
    public class PropertyMatchService
    {
        public UserProperty getPropertyMatchNameValue(string message, string regex)
        {
            var property = new UserProperty();
            var match = Regex.Match(message, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count == 3)
            {
                property.name = match.Groups[1].Value;
                property.value = match.Groups[2].Value;
            }
            return property;
        }

        public UserProperty getPropertyMatchValueName(string message, string regex)
        {
            var property = new UserProperty();
            var match = Regex.Match(message, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count == 3)
            {
                property.value = match.Groups[1].Value;
                property.name = match.Groups[2].Value;
            }
            return property;
        }

        public UserProperty getSelfPropertyMatchValue(string message, string regex)
        {
            var property = new UserProperty();
            var match = Regex.Match(message, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count == 2)
            {
                property.value = match.Groups[1].Value;
                property.name = "self";
            }
            return property;
        }

        public UserNameAndProperty getPropertyMatchUserValueName(string message, string regex)
        {
            var userName = string.Empty;
            var property = new UserProperty();
            var match = Regex.Match(message, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count == 4)
            {
                userName = match.Groups[1].Value;
                property.value = match.Groups[2].Value;
                property.name = match.Groups[3].Value;
            }
            return new UserNameAndProperty() { userName = userName, userProperty = property };
        }

        public UserNameAndProperty getSelfPropertyMatchUserValueName(string message, string regex)
        {
            var userName = string.Empty;
            var property = new UserProperty();
            var match = Regex.Match(message, regex, RegexOptions.IgnoreCase);
            if (match.Groups.Count == 3)
            {
                userName = match.Groups[1].Value;
                property.value = match.Groups[2].Value;
                property.name = "self";
            }
            return new UserNameAndProperty() { userName = userName, userProperty = property };
        }
    }
}
