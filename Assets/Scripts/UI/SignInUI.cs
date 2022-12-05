using UnityEngine;
using UnityEngine.UI;

public class SignInUI : MonoBehaviour
{
    [SerializeField] PageManager pageManager = null;

    [SerializeField] InfoText errorText = null;

    [SerializeField] InputField inputID = null;
    [SerializeField] InputField inputPass = null;

    string lastSignInEmail = "";
    public void SignIn()
    {
        lastSignInEmail = inputID.text;

        if (lastSignInEmail == "a" && inputPass.text == "a") // backdoor
            OnSignInCallback(true);
        else
            APICaller.CanSignIn(lastSignInEmail, inputPass.text, OnSignInCallback);
    }

    void OnSignInCallback(bool success)
    {
        if (success)
        {
            UserLogInData.isUserLoggedIn = true;
            UserLogInData.userEmail = lastSignInEmail;
            pageManager.SwitchPage(2);
        }
        else
            errorText.Activate();
    }
}
