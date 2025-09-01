namespace PetFamily.Application.VolunteersOperations.PetsOperations.Delete
{
    public record DeletePetCommand(Guid VolunteerId, Guid PetId);
}