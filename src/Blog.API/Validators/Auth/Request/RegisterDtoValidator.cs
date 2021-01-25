using System.Threading;
using System.Threading.Tasks;
using Blog.Application.Repositories.AuthRepo;
using Blog.Domain.Dtos.Auth.Request;
using FluentValidation;

namespace Blog.API.Validators.Auth.Request
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
