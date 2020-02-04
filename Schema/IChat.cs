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

        Message AddMessage(Message message);

        IObservable<Message> Messages(string groupname);
    }

        public class Chat : IChat
    {
        private readonly ISubject<Message> _messageStream = new ReplaySubject<Message>(1);

        public Chat()
        {
            AllMessages = new ConcurrentStack<Message>();
        }

        public ConcurrentStack<Message> AllMessages { get; }

        public Message AddMessage(Message message)
        {
            AllMessages.Push(message);
            _messageStream.OnNext(message);
            // also add to DynamoDB
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