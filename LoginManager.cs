using CodeMonkey;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using System.Xml.XPath;

public class fUser //�׽�Ʈ��
{
    public string id;
    public string name;
}
public class LoginManager : MonoBehaviour
{
    private IAuthService signInService;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        signInService = new GoogleSignInAndroid();
#endif

#if UNITY_EDITOR
        signInService = new GoogleSignInEditor();
#endif
    }

    public async void Login()
    {
        try
        {
            string uid = await signInService.SignInAsync();
            Debug.Log($"�α��� ����, uid : {uid}");

            FirebaseManager fm = Manager.Instance.FirebaseManager;
            fUser user = new fUser();
            user.id = uid;
            user.name = "test";

            Debug.Log("������ ���� ��...");

            //������ ����
            string result = await fm.SaveData("user",uid, user);
            Debug.Log(result);

            //������ �ҷ�����
            string dataResult = await fm.LoadData("user", uid);
            Debug.Log($"data : {dataResult}");

        }
        catch(System.Exception ex)
        {
            Debug.LogError($"�α��� ����: {ex.Message}");
        }
    }
}
