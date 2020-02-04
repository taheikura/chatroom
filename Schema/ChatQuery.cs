using System.Linq;
using GraphQL.Types;

namespace nsChatSchema
{
    public class ChatQuery : ObjectGraphType<object>
    {
        public ChatQuery(IChat chat)
        {
            Field<ListGraphType<ChatGraphQL.MessageType>>("messages", resolve: context => chat.AllMessages.Take(100));
        }
    }
}
