using System;

namespace ChatModels
{
    public class Message
    {
        public string GroupName { get; set; }

        public DateTime PostedAt { get; set; }

        public string UserName { get; set; }

        public string Msg { get; set; }

    }
}