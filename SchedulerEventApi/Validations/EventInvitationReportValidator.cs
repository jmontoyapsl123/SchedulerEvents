using FluentValidation;
using SchedulerEventCommon.Dtos;

namespace SchedulerEventApi.Validations
{
    public class EventInvitationReportValidator : AbstractValidator<ReportParamDto>
    {
        public EventInvitationReportValidator()
        {
            RuleFor(x => x.EventId).GreaterThan(0);
            RuleFor(x => x.StateInvitation).GreaterThan(0);
        }
    }
}