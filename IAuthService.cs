using System.Threading.Tasks;

public interface IAuthService
{
    Task<string> SignInAsync();       // ���� �� Firebase UID ����
    void SignOut();               
}