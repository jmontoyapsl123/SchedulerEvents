using FluentValidation;
using SchedulerEventCommon.Dtos;

namespace SchedulerEventApi.Validations
{
    public class EventInvitationValidator : AbstractValidator<EventInvitationDto>
    {
        public EventInvitationValidator()
        {
            RuleFor(x => x.EventId).NotNull();
            RuleFor(x => x.DeveloperId).NotNull();
        }
    }
}