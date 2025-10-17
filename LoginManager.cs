using CodeMonkey;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using System.Xml.XPath;

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

    //초기화 및 자동 로그인 확인
        auth = FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            Debug.Log($"자동 로그인: {auth.CurrentUser.UserId}");
            RefreshTokenAsync(auth.CurrentUser);
        }
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

    //자동 재인증 토큰 갱신
    private void RefreshTokenAsync(FirebaseUser user)
    {
        user.TokenAsync(true).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("토큰 갱신 실패: " + task.Exception);
                return;
            }

            string idToken = task.Result;
            Debug.Log("토큰 갱신 완료: " + idToken);
        });
    }
}
