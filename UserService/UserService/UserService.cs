using ChatModels;
using UserDatabase.Services;

namespace UserService
{
    public class UserService
    {
        private UserNickNameService userNickNameService;
        private UserPropertyService userPropertyService;
        private UserDerivedPropertyService userDerivedPropertyService;
        private UserSaveService userSaveService;

        public UserService()
        {
            userNickNameService = new UserNickNameService();
            userPropertyService = new UserPropertyService();
            userDerivedPropertyService = new UserDerivedPropertyService();
            userSaveService = new UserSaveService();
        }

        public void UpdateUsers(AnalyzedChat analyzedChat)
        {
            var usersData = UserDatabase.UserDatabase.userDatabase;

            var nickName = userNickNameService.GetNickName(analyzedChat);
            var property = userPropertyService.GetProperty(analyzedChat);

            var index = usersData.FindIndex(ud => ud != null && ud.userName != null && ud.userName == analyzedChat.chat.user);
            if (index >= 0)
            {
                if (!string.IsNullOrEmpty(nickName))
                {
                    if (!usersData[index].nickNames.Contains(nickName))
                    {
                        usersData[index].nickNames.Add(nickName);
                    }
                }
                if (!string.IsNullOrEmpty(property.name) && !string.IsNullOrEmpty(property.value) && !string.IsNullOrEmpty(property.source))
                {
                    usersData[index].properties.Add(property);
                }
                usersData[index].derivedProperties.AddRange(userDerivedPropertyService.GetDerivedProperties(analyzedChat, property, usersData[index]));

                userSaveService.SaveUserData(usersData[index]);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(analyzedChat.chat.user))
                {
                    var userData = new UserData(analyzedChat.chat.user);
                    if (!string.IsNullOrEmpty(nickName))
                    {
                        if (!userData.nickNames.Contains(nickName))
                        {
                            userData.nickNames.Add(nickName);
                        }
                    }
                    userData.derivedProperties.AddRange(userDerivedPropertyService.GetDerivedProperties(analyzedChat, property, userData));

                    usersData.Add(userData);
                    userSaveService.SaveUserData(userData);
                }
            }

            var otherUserProperty = userPropertyService.GetOtherUserProperty(analyzedChat, usersData);
            index = usersData.FindIndex(ud => ud != null && ud.userName != null && ud.userName == otherUserProperty.userName);
            if (index == -1)
            {
                index = usersData.FindIndex(ud => ud != null && ud.userName != null && ud.nickNames.Contains(otherUserProperty.userName));
            }
            if (index >= 0)
            {
                if (!string.IsNullOrEmpty(otherUserProperty.userProperty.name) && !string.IsNullOrEmpty(otherUserProperty.userProperty.value) && !string.IsNullOrEmpty(otherUserProperty.userProperty.source))
                {
                    usersData[index].properties.Add(otherUserProperty.userProperty);
                }
                userSaveService.SaveUserData(usersData[index]);
            }
        }
    }
}
