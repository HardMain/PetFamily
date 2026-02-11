using System.Collections;

namespace SharedKernel.Failures
{
    public class ErrorList : IEnumerable<Error>
    {
        private readonly List<Error> _errors = [];
        public bool IsEmpty => _errors.Count == 0;

        public ErrorList(IEnumerable<Error> errors)
        {
            _errors = errors.ToList();
        }

        public IEnumerator<Error> GetEnumerator()
        {
            return _errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _errors.GetEnumerator();
        }


        public static implicit operator ErrorList(List<Error> errors) =>
            new(errors);


        public static implicit operator ErrorList(Error error) =>
            new([error]);
    }
}
