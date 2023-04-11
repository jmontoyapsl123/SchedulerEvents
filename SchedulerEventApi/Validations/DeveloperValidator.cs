using FluentValidation;
using SchedulerEventCommon.Dtos;

namespace SchedulerEventApi.Validations
{
    public class DeveloperValidator : AbstractValidator<DeveloperDto>
    {
        public DeveloperValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.MobileNumber).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.BirthDay).NotEmpty().Must(ValidateAge).WithMessage("Invalid date or the age must be greater than 18.");
        }
        protected bool ValidateAge(DateTime date)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - date.Year;
            if(date > today.AddYears(-age))
            {
                age--;
            }

            return age > 18;
        }
    }
}