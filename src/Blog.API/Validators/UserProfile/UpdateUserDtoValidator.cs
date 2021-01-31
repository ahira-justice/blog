using Blog.Application.Repositories.AuthRepo;
using Blog.Domain.Dtos.UserProfile;
using FluentValidation;

namespace Blog.API.Validators.UserProfile
{
    public class UpdateUserDtoValidator : NullReferenceAbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(dto => dto.FirstName)
                .NotEmpty().WithMessage("FirstName is required")
                .NotNull().WithMessage("FirstName is required");
            RuleFor(dto => dto.LastName)
                .NotEmpty().WithMessage("LastName is required")
                .NotNull().WithMessage("LastName is required");
        }
    }
}
