using GraphQL.Types;

namespace ChatSchema
{
    public class ChatMutation : ObjectGraphType<object>
    {
        public ChatMutation(IChat chat)
        {
            Field<ChatGraphQL.MessageType>("addMessage",
                arguments: new QueryArguments(
                    new QueryArgument<MessageInputType> { Name = "message" }
                ),
                resolve: context =>
                {
                    var receivedMessage = context.GetArgument<ChatModels.Message>("message");
                    var message = chat.AddMessage(receivedMessage);
                    return message;
                });
        }
    }

    public class MessageInputType : InputObjectGraphType
    {
        public MessageInputType()
        {
            Field<StringGraphType>("UserName");
            Field<StringGraphType>("GroupName");
            Field<StringGraphType>("Msg");
            Field<DateGraphType>("PostedAt");
        }
    }
}
