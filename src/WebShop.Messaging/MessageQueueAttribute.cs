using System;
using System.Collections.Generic;
using System.Text;

namespace WebShop.Messaging
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageQueueAttribute : Attribute
    {
        public string Name { get; }

        public MessageQueueAttribute(string name)
        {
            Name = name?.ToLowerInvariant();
        }
    }
}
