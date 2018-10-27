using ChatModels;
using System.Collections.Generic;
using System.Linq;
using UserDatabase.Services;

namespace UserService
{
    public class UserService
    {
        private UserNickNameService userNickNameService;
        private UserPropertyService userPropertyService;
        private OtherUserPropertyService otherUserPropertyService;
        private UserDerivedPropertyService userDerivedPropertyService;
        private UserSaveService userSaveService;

        public UserService()
        {
            userNickNameService = new UserNickNameService();
            userPropertyService = new UserPropertyService();
            otherUserPropertyService = new OtherUserPropertyService();
            userDerivedPropertyService = new UserDerivedPropertyService();
            userSaveService = new UserSaveService();
        }

        public void UpdateUsers(AnalyzedChat userResponse, AnalyzedChat question)
        {
            var nickName = userNickNameService.GetNickName(userResponse, question);
            //TODO: remove nickname ex. "don't call me XXX"
            var property = userPropertyService.GetProperty(userResponse, question);

            var userData = UserDatabase.UserDatabase.userDatabase.FirstOrDefault(ud => ud != null && ud.userName != null && ud.userName == userResponse.chat.user);
            if (userData != null)
            {
                if (!string.IsNullOrEmpty(nickName))
                {
                    if (!userData.nickNames.Contains(nickName))
                    {
                        userData.nickNames.Add(nickName);
                    }
                }
                if (!string.IsNullOrEmpty(property.name) && !string.IsNullOrEmpty(property.value) && !string.IsNullOrEmpty(property.source))
                {
                    userData.properties.Add(property);
                }
                userData.derivedProperties.AddRange(userDerivedPropertyService.GetDerivedProperties(userResponse, property, userData));

                userSaveService.SaveUserData(userData);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(userResponse.chat.user))
                {
                    userData = new UserData(userResponse.chat.user);
                    if (!string.IsNullOrEmpty(nickName))
                    {
                        if (!userData.nickNames.Contains(nickName))
                        {
                            userData.nickNames.Add(nickName);
                        }
                    }
                    userData.derivedProperties.AddRange(userDerivedPropertyService.GetDerivedProperties(userResponse, property, userData));

                    UserDatabase.UserDatabase.userDatabase.Add(userData);
                    userSaveService.SaveUserData(userData);
                }
            }

            var otherUserProperty = otherUserPropertyService.GetOtherUserProperty(userResponse, UserDatabase.UserDatabase.userDatabase);
            userData = UserDatabase.UserDatabase.userDatabase.FirstOrDefault(ud => ud != null && ud.userName != null && ud.userName == otherUserProperty.userName);
            if (userData == null)
            {
                userData = UserDatabase.UserDatabase.userDatabase.FirstOrDefault(ud => ud != null && ud.userName != null && ud.nickNames.Contains(otherUserProperty.userName));
            }
            if (userData != null)
            {
                if (!string.IsNullOrEmpty(otherUserProperty.userProperty.name) && !string.IsNullOrEmpty(otherUserProperty.userProperty.value) && !string.IsNullOrEmpty(otherUserProperty.userProperty.source))
                {
                    userData.properties.Add(otherUserProperty.userProperty);
                }
                userSaveService.SaveUserData(userData);
            }
        }
    }
}
