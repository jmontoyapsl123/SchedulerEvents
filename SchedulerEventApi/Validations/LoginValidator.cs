using FluentValidation;
using SchedulerEventCommon.Dtos;

namespace SchedulerEventApi.Validations
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

        }
    }
}