namespace ConversationMatcher.Services
{
    public class GroupChatConfidenceService
    {
        public double GetGroupChatConfidence(bool targetGroupChat, bool existingGroupChat)
        {
            if (targetGroupChat == existingGroupChat)
            {
                return 1;
            }
            if (targetGroupChat == true)
            {
                return .5;
            }
            return 0;
        }
    }
}
