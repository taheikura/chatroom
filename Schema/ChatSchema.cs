using GraphQL.Types;

namespace ChatSchema
{
    public class ChatSchema : Schema
    {
        public ChatSchema(IChat chat)
        {
            Query = new ChatQuery(chat);
            Mutation = new ChatMutation(chat);
            Subscription = new ChatSubscription(chat);
        }
    }
}