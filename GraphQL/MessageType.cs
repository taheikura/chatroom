using ChatModels;
using GraphQL.Types;

namespace ChatGraphQL
{
    public class MessageType : ObjectGraphType<Message>
    {
        public MessageType()
        {
            Field(o => o.GroupName);
            Field(o => o.PostedAt);
            Field(o => o.UserName);
            Field(o => o.Msg);
        }
    }
}
