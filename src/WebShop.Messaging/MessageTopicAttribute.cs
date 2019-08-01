using System;
using System.Collections.Generic;
using System.Text;

namespace GetBee.Messaging
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageTopicAttribute : Attribute
    {
        public string Name { get; }

        public MessageTopicAttribute(string name)
        {
            Name = name?.ToLowerInvariant();
        }
    }
}
