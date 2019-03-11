using ConversationMatcher.Services;
using Xunit;

namespace ConversationMatcher.Tests
{
    public class GroupChatConfidenceService_GetGroupChatConfidence
    {
        [Fact]
        public void GetGroupChatConfidence()
        {
            var service = new GroupChatConfidenceService();

            var result = service.GetGroupChatConfidence(true, true);
            Assert.Equal(1, result);

            result = service.GetGroupChatConfidence(false, false);
            Assert.Equal(1, result);

            result = service.GetGroupChatConfidence(true, false);
            Assert.Equal(.5, result);

            result = service.GetGroupChatConfidence(false, true);
            Assert.Equal(0, result);
        }
    }
}
