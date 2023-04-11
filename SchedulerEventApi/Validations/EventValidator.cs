using FluentValidation;
using SchedulerEventCommon.Dtos;

namespace SchedulerEventApi.Validations
{
    public class EventValidator : AbstractValidator<EventDto>
    {
        public EventValidator()
        {
            RuleFor(x => x.EventName).NotEmpty().MaximumLength(300);
            RuleFor(x => x.EventDate).NotNull();
            RuleFor(x => x.EventType).NotNull();

        }
    }
}