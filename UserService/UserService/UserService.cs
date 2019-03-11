using ChatModels;
using System.Linq;
using UserDatabase.Services;

namespace UserService
{
    public class UserService
    {
        private readonly UserNickNameService _userNickNameService;
        private readonly UserPropertyService _userPropertyService;
        private readonly OtherUserPropertyService _otherUserPropertyService;
        private readonly UserDerivedPropertyService _userDerivedPropertyService;
        private readonly UserSaveService _userSaveService;

        public UserService(UserNickNameService userNickNameService, UserPropertyService userPropertyService, OtherUserPropertyService otherUserPropertyService, UserDerivedPropertyService userDerivedPropertyService, UserSaveService userSaveService)
        {
            _userNickNameService = userNickNameService;
            _userPropertyService = userPropertyService;
            _otherUserPropertyService = otherUserPropertyService;
            _userDerivedPropertyService =userDerivedPropertyService;
            _userSaveService = userSaveService;
        }

        public void UpdateUsers(AnalyzedChat userResponse, AnalyzedChat question)
        {
            var nickName = _userNickNameService.GetNickName(userResponse, question);
            //TODO: remove nickname ex. "don't call me XXX"
            var property = _userPropertyService.GetProperty(userResponse, question);

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
                userData.derivedProperties.AddRange(_userDerivedPropertyService.GetDerivedProperties(userResponse, property, userData));

                _userSaveService.SaveUserData(userData);
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
                    userData.derivedProperties.AddRange(_userDerivedPropertyService.GetDerivedProperties(userResponse, property, userData));

                    UserDatabase.UserDatabase.userDatabase.Add(userData);
                    _userSaveService.SaveUserData(userData);
                }
            }

            var otherUserProperty = _otherUserPropertyService.GetOtherUserProperty(userResponse, UserDatabase.UserDatabase.userDatabase);
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
                _userSaveService.SaveUserData(userData);
            }
        }
    }
}
