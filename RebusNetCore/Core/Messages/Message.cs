using System;

namespace Core.Messages
{
    public  class Message
    {
        public Guid AggregateRoot { get; set; }
        public DateTime DateTime { get; set; }
        protected Message()
        {
            DateTime = DateTime.Now;
        }
    }
}
