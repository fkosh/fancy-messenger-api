using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FancyMessengerApi.Entities;

namespace FancyMessengerApi.Repository
{
    public class MessageRepository
    {
        private readonly List<MessageEntity> _messages;

        public MessageRepository()
        {
            _messages = new List<MessageEntity>();
        }

        public async Task<string> InsertOneAsync(MessageEntity instance, CancellationToken ct)
        {
            return await Task<string>.Factory.StartNew(
                () => {
                    instance.Id = Guid.NewGuid().ToString();
                    instance.CreatedAt = DateTime.UtcNow;
                    
                    _messages.Add(instance);
                
                    return instance.Id;
                },
                ct
            );
        }
        
        public async Task<IEnumerable<MessageEntity>> FindAsync(
            string firstParticipantId, string secondParticipantId, CancellationToken ct)
        {
            return await Task<IEnumerable<MessageEntity>>.Factory.StartNew(
                () => _messages.Where(message => 
                    (message.SenderId == firstParticipantId || message.ReceiverId == firstParticipantId)
                    && (message.SenderId == secondParticipantId || message.ReceiverId == secondParticipantId)
                ), 
                ct
            );
        }
    }
}