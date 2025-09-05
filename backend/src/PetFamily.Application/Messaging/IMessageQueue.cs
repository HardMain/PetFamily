namespace PetFamily.Application.Messaging
{
    public interface IMessageQueue<TMessage>
    {
        public Task WriteAsync(TMessage paths, CancellationToken cancellationToken);

        public Task<TMessage> ReadAsync(CancellationToken cancellationToken);
    }
}
