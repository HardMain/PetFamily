using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Api.Envelops
{
    public record Envelop
    {
        private Envelop(object? result, Error? error)
        {
            Result = result;
            ErrorCode = error?.Code;
            ErrorMessage = error?.Message;
            TimeGenerated = DateTime.Now; 
        }

        public object? Result { get; }
        public string? ErrorCode { get; }
        public string? ErrorMessage { get; }
        public DateTime TimeGenerated { get; }

        public static Envelop Ok(object? result) =>
            new Envelop(result, null);

        public static Envelop Error(Error? error) =>
            new Envelop(null, error);
    }
}
