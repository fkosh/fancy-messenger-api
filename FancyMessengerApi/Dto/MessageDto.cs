using System;

namespace FancyMessengerApi.Dto
{
    public class MessageDto
    {
        public string Id { get; set; }

        public string SenderId { get; set; }

        public string Text { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}