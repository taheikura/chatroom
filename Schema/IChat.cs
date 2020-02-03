using System;
using System.Collections.Concurrent;
using ChatModels;

namespace ChatSchema
{
    public interface IChat
    {
        ConcurrentStack<Message> AllMessages { get; }

        Message AddMessage(Message message);

        IObservable<Message> Messages(string groupname);
    }
}