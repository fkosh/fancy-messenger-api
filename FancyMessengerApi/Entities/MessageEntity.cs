using System;

namespace FancyMessengerApi.Entities
{
    public class MessageEntity
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Text { get; set; }
    }
}