namespace SchedulerEventCommon.Dtos;

public class ResponseDto<T>
{
    public bool HasError { get; set; }
    public T Result { get; set; }
    public string Errors { get; set;}
}