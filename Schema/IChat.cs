using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ChatModels;

namespace nsChatSchema
{
    public interface IChat
    {
        ConcurrentStack<Message> AllMessages { get; }
        ConcurrentStack<string> AllGroups { get; }

        Message AddMessage(Message message);

        IObservable<Message> Messages(string groupname);
    }

    public class Chat : IChat
    {
        private readonly ISubject<Message> _messageStream = new ReplaySubject<Message>(1);
        private readonly ISubject<string> _groupStream = new ReplaySubject<string>(1);

        public Chat()
        {
            AllMessages = new ConcurrentStack<Message>();
            AllGroups = new ConcurrentStack<string>();
        }

        public ConcurrentStack<Message> AllMessages { get; }
        public ConcurrentStack<string> AllGroups { get; }

        public Message AddMessage(Message message)
        {
            // TODO: validate, check that group exists
            AllMessages.Push(message);
            _messageStream.OnNext(message);
            // TODO: also add to DynamoDB
            return message;
        }

        public IObservable<Message> Messages(string groupname)
        {
            return _messageStream
                .Select(message =>
                {
                    message.GroupName = groupname;
                    return message;
                })
                .AsObservable();
        }
    }
}