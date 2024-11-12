using DiceMaster3600.Core.Enum;
using DiceMaster3600.ViewModel.Control;
using FluentValidation;
using System;

namespace DiceMaster3600.Model.Validators
{
    class EntryFormValidator : AbstractValidator<EntryFormViewModel>
    {
        public EntryFormValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is required.");

            RuleFor(x => x.DateOfBirth).Must(d => DateTime.Now.Year - d.Year >= 15).WithMessage("You must be at least 15 years old.");
            RuleFor(x => x.University).NotEqual(UniversityType.None).WithMessage("Please select a university.");
            RuleFor(x => x.Faculty).NotEqual(FacultyType.None).WithMessage("Please select a faculty.");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                                 .EmailAddress().WithMessage("Please enter a valid email.");
        }
    }
}
