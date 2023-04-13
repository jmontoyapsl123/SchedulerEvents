using FluentValidation;
using SchedulerEventCommon.Dtos;

namespace SchedulerEventApi.Validations
{
    public class EventInvitationReportValidator : AbstractValidator<ReportParamDto>
    {
        public EventInvitationReportValidator()
        {
            RuleFor(x => x.EventId).NotNull();
            RuleFor(x => x.StateInvitation).NotNull();
        }
    }
}