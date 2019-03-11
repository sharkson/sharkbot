namespace ChatModels
{
    public class UserNameAndProperty
    {
        public UserNameAndProperty()
        {
            userName = string.Empty;
            userProperty = new UserProperty();
        }

        public string userName { get; set; }
        public UserProperty userProperty { get; set; }
    }
}
