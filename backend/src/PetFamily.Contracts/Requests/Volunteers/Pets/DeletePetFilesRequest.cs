namespace PetFamily.Contracts.Requests.Volunteers.Pets
{
    public record DeletePetFilesRequest(IEnumerable<string> ObjectNameList);
}