using System.Threading.Tasks;

public interface IAuthService
{
    Task<string> SignInAsync();       // 성공 시 Firebase UID 리턴
    void SignOut();               
}