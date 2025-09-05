using System.Threading.Channels;
using PetFamily.Application.Messaging;

namespace PetFamily.Infrastructure.MessageQueues
{
    public class InMemoryMessageQueue<TMessage> : IMessageQueue<TMessage> 
    {
        private readonly Channel<TMessage> _channel = Channel.CreateUnbounded<TMessage>();

        public async Task WriteAsync(TMessage paths, CancellationToken cancellationToken)
        {
            await _channel.Writer.WriteAsync(paths, cancellationToken);
        }
         
        public async Task<TMessage> ReadAsync(CancellationToken cancellationToken)
        {
            return await _channel.Reader.ReadAsync(cancellationToken);
        }
    }
}