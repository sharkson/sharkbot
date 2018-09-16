﻿using ChatModels;
using System.Collections.Generic;
using System.Linq;

namespace UserService
{
    public class UserDerivedPropertyService
    {
        public List<UserProperty> GetDerivedProperties(AnalyzedChat analyzedChat, UserProperty givenProperty, UserData userData)
        {
            var properties = new List<UserProperty>();

            properties.AddRange(getSex(analyzedChat, givenProperty, userData)); //TODO: add more of these, address, etc.

            return properties;
        }

        private List<string> sexMale = new List<string>() { "male", "boy", "man", "chap", "dude" };
        private List<string> sexFemale = new List<string>() { "female", "girl", "woman", "lady" };

        private List<string> sexMaleMatch = new List<string>() { "i have a penis", "i have a dick", "my penis", "my dick" };
        private List<string> sexFemaleMatch = new List<string>() { "i have a vagina", "i have boobs", "my vagina", "my boobs" }; //TODO: create regex for matching 2 lists, "I have", "my" then "vagina", "boobs"
        private List<UserProperty> getSex(AnalyzedChat analyzedChat, UserProperty givenProperty, UserData userData)
        {
            var properties = new List<UserProperty>();

            if (!string.IsNullOrEmpty(givenProperty.name) && !string.IsNullOrEmpty(givenProperty.value))
            {
                if(givenProperty.name == "self")
                {
                    if(sexMale.Any(m => m == givenProperty.value.ToLower()))
                    {
                        properties.Add(new UserProperty() { name = "sex", value = "male", source = givenProperty.source, time = givenProperty.time });
                        return properties;
                    }
                    if (sexFemale.Any(m => m == givenProperty.value.ToLower()))
                    {
                        properties.Add(new UserProperty() { name = "sex", value = "female", source = givenProperty.source, time = givenProperty.time });
                        return properties;
                    }
                }
            }

            if(sexMaleMatch.Any(m => analyzedChat.chat.message.ToLower().Contains(m)))
            {
                properties.Add(new UserProperty() { name = "sex", value = "male", source = analyzedChat.chat.user, time = analyzedChat.chat.time });
                return properties;
            }

            if (sexFemaleMatch.Any(m => analyzedChat.chat.message.ToLower().Contains(m)))
            {
                properties.Add(new UserProperty() { name = "sex", value = "female", source = analyzedChat.chat.user, time = analyzedChat.chat.time });
                return properties;
            }

            return properties;
        }
    }
}