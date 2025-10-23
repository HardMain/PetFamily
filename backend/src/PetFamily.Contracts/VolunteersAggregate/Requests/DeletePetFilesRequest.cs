namespace PetFamily.Contracts.VolunteersAggregate.Requests
{
    public record DeletePetFilesRequest(IEnumerable<string> ObjectNameList);
}