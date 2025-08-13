﻿using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.Entities;

namespace PetFamily.Application.Volunteers.Restore
{
    public class RestoreVolunteerCommandValidator : AbstractValidator<RestoreVolunteerCommand>
    {
        public RestoreVolunteerCommandValidator()
        {
            RuleFor(v => v.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        }
    }
}
