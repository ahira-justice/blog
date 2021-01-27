using Blog.Application.Repositories.AuthRepo;
using Blog.Domain.Dtos.Auth.Request;
using FluentValidation;

namespace Blog.API.Validators.Auth.Request
{
    public class LoginDtoValidator : NullReferenceAbstractValidator<LoginDto>
    {
        private readonly IAuthRepository _repo;
        public LoginDtoValidator(IAuthRepository repo)
        {
            _repo = repo;

            RuleFor(dto => dto.Username)
                .NotEmpty().WithMessage("Username is required")
                .NotNull().WithMessage("Username is required");
            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Password is required")
                .NotNull().WithMessage("Password is required");
        }
    }
}
