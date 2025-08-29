namespace PetFamily.Application.VolunteersOperations.PetsOperations.Restore
{
    public record RestorePetCommand(Guid VolunteerId, Guid PetId);
}
