using CodeMonkey;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using System.Xml.XPath;

public class fUser //테스트용
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
            Debug.Log($"로그인 성공, uid : {uid}");

            FirebaseManager fm = Manager.Instance.FirebaseManager;
            fUser user = new fUser();
            user.id = uid;
            user.name = "test";

            Debug.Log("데이터 저장 중...");

            //데이터 저장
            string result = await fm.SaveData("user",uid, user);
            Debug.Log(result);

            //데이터 불러오기
            string dataResult = await fm.LoadData("user", uid);
            Debug.Log($"data : {dataResult}");

        }
        catch(System.Exception ex)
        {
            Debug.LogError($"로그인 실패: {ex.Message}");
        }
    }
}
