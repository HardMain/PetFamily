namespace Volunteers.Contracts.Requests
{
    public record DeletePetFilesRequest(IEnumerable<string> ObjectNameList);
}