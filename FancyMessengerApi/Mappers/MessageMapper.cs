using FancyMessengerApi.Dto;
using FancyMessengerApi.Entities;

namespace FancyMessengerApi.Mappers
{
    public static class MessageMapper
    {
        public static MessageDto ToDto(this MessageEntity instance)
        {
            return new MessageDto {
                Id = instance.Id,
                Text = instance.Text,
                SenderId = instance.SenderId,
                CreatedAt = instance.CreatedAt
            };
        }
    }
}