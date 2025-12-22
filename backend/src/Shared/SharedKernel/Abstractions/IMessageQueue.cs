namespace SharedKernel.Abstractions
{
    public interface IMessageQueue<TMessage>
    {
        public Task WriteAsync(TMessage paths, CancellationToken cancellationToken);

        public Task<TMessage> ReadAsync(CancellationToken cancellationToken);
    }
}
