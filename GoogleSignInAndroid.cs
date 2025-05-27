
using Firebase.Auth;
using Google;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class GoogleSignInAndroid : IAuthService
{
    private FirebaseAuth auth;
    private GoogleSignInConfiguration config;

    public GoogleSignInAndroid()
    {
        auth = FirebaseAuth.DefaultInstance;

        config = new GoogleSignInConfiguration
        {
            WebClientId = "873257189805-ga919sj8vev61uqggsl0us4jb0pudg7h.apps.googleusercontent.com",
            RequestIdToken = true
        };

        GoogleSignIn.Configuration = config;
    }

    public async Task<string> SignInAsync()
    {
        if (auth.CurrentUser != null)
        {
            return auth.CurrentUser.UserId;
        }

        try
        {
            var googleUser = await GoogleSignIn.DefaultInstance.SignIn();
            if (googleUser == null || string.IsNullOrEmpty(googleUser.IdToken))
                throw new Exception("Google Sign-In failed or cancelled.");

            var credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);
            FirebaseUser firebaseUser = await auth.SignInWithCredentialAsync(credential);

            return firebaseUser.UserId;
        }
        catch (Exception ex)
        {
            throw new Exception("SignInAsync error: " + ex.Message);
        }
    }

    public void SignOut()
    {
        auth.SignOut();
        GoogleSignIn.DefaultInstance.SignOut();
    }
}

