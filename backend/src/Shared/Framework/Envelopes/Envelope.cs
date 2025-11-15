using SharedKernel.Failures;

namespace Framework.Envelopes
{
    public record Envelope
    {
        private Envelope(object? result, ErrorList? errors)
        {
            Result = result;
            Errors = errors;
            TimeGenerated = DateTime.Now;
        }

        public object? Result { get; }
        public ErrorList? Errors { get; }
        public DateTime TimeGenerated { get; }

        public static Envelope Ok(object? result) =>
            new Envelope(result, null);

        public static Envelope Error(ErrorList? errors) =>
            new(null, errors);
    }
}