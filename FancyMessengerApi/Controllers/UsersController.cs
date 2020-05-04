using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FancyMessengerApi.Dto;
using FancyMessengerApi.Entities;
using FancyMessengerApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FancyMessengerApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class UsersController : AuthorizedUserController
    {
        private readonly UserRepository _userRepository;
        private readonly MessageRepository _messageRepository;
        
        public UsersController(
            UserRepository userRepository, MessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }
        
        /// <summary>
        /// TODO except requester
        /// TODO paging
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(CancellationToken ct)
        {
            return Ok(
                (await _userRepository.FindAllAsync(ct)).Select(
                    user => new UserDto { Id = user.Id, Username = user.Username }
                )
            );
        }
        
        /// <summary>
        /// Get conversation messages between authrorized user and target user.
        /// TODO paging!
        /// </summary>
        /// <returns></returns>
        [HttpGet("{userId}/conversation")]
        [ProducesResponseType(typeof(MessageEntity[]), StatusCodes.Status200OK)] // TODO DTO
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetConversationAsync(
            [FromRoute] string userId,
            [FromQuery, Required] int skip,
            [FromQuery, Required] int take,
            CancellationToken ct)
        {
            var messages = await _messageRepository.FindAsync(AuthorizedUserId, userId, ct);

            var pagedMessages = messages.OrderBy(
                message => message.CreatedAt
            ).Skip(skip).Take(take).ToArray();

            if (pagedMessages.Length == 0)
                return NoContent();
            
            return Ok(pagedMessages);
        }
        
        /// <summary>
        /// Add conversation message from authrorized user to target user.
        /// </summary>
        [HttpPost("{userId}/conversation/messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddConversationMessageAsync(
            [FromRoute] string userId, 
            [FromBody, Required] MessageDto message, 
            CancellationToken ct)
        {
            var messageId = await _messageRepository.InsertOneAsync(
                new MessageEntity {
                    SenderId = AuthorizedUserId,
                    ReceiverId = userId,
                    Text = message.Text
                }, 
                ct
            );

            return Ok(messageId); // TODO swagger + time of insert
        }
    }
}