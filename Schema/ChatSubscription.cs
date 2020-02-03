using System;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Claims;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Server.Transports.Subscriptions.Abstractions;
using GraphQL.Subscription;
using GraphQL.Types;

namespace ChatSchema
{
    public class ChatSubscription : ObjectGraphType<object>
    {
        private readonly IChat _chat;

        public ChatSubscription(IChat chat)
        {
            _chat = chat;
            AddField(new EventStreamFieldType
            {
                Name = "messageAddedToGroup",
                Arguments = new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "group" }
                ),
                Type = typeof(ChatGraphQL.MessageType),
                Resolver = new FuncFieldResolver<ChatModels.Message>(ResolveMessage),
                Subscriber = new EventStreamResolver<ChatModels.Message>(SubscribeByGroup)
            });
        }

        private IObservable<ChatModels.Message> SubscribeByGroup(ResolveEventStreamContext context)
        {
            var group = context.GetArgument<string>("group");
            return _chat.Messages(group).Where(message => message.GroupName == group);
        }

        private ChatModels.Message ResolveMessage(ResolveFieldContext context)
        {
            return context.Source as ChatModels.Message;
        }
    }
}
