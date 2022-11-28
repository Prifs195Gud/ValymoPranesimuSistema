using UnityEngine;
using UnityEngine.UI;

public class SignInUI : MonoBehaviour
{
    [SerializeField] PageManager pageManager = null;

    [SerializeField] InfoText errorText = null;

    [SerializeField] InputField inputID = null;
    [SerializeField] InputField inputPass = null;

    public void SignIn()
    {
        if (inputID.text == "a" && inputPass.text == "a") // backdoor
            OnSignInCallback(true);
        else
            APICaller.CanSignIn(inputID.text, inputPass.text, OnSignInCallback);
    }

    void OnSignInCallback(bool success)
    {
        if (success)
            pageManager.SwitchPage(2);
        else
            errorText.Activate();
    }
}
