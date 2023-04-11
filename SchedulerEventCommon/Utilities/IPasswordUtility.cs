namespace SchedulerEventCommon.Utilities;
public interface  IPasswordUtility
{
    string HashPasword(string password, out byte[] salt);
    bool VerifyPassword(string password, string hash, byte[] salt);
}