using System.Threading;
using System.Threading.Tasks;
using Blog.Application.Repositories.AuthRepo;
using Blog.Domain.Dtos.Auth.Request;
using FluentValidation;

namespace Blog.API.Validators.Auth
{
    public class RegisterDtoValidator : NullReferenceAbstractValidator<RegisterDto>
    {
        private readonly IAuthRepository _repo;
        public RegisterDtoValidator(IAuthRepository repo)
        {
            _repo = repo;

            RuleFor(dto => dto.Username)
                .NotEmpty().WithMessage("Username is required")
                .NotNull().WithMessage("Username is required")
                .MustAsync(UserDoesNotExist).WithMessage("Username already exists");
            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Password is required")
                .NotNull().WithMessage("Password is required")
                .Must(IsValidPassword).WithMessage("Password must be at least 8 characters long");
            RuleFor(dto => dto.FirstName)
                .NotEmpty().WithMessage("FirstName is required")
                .NotNull().WithMessage("FirstName is required");
            RuleFor(dto => dto.LastName)
                .NotEmpty().WithMessage("LastName is required")
                .NotNull().WithMessage("LastName is required");
        }

        private async Task<bool> UserDoesNotExist(string username, CancellationToken cancellationToken)
        {
            return username == null ? false : !await _repo.UserExists(username.ToLower());
        }

        private bool IsValidPassword(string password)
        {
            return password == null ? false : password.Length >= 8;
        }
    }
}
