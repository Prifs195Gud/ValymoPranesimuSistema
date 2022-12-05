using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUI : MonoBehaviour
{
    [SerializeField] PageManager pageManager = null;

    [SerializeField] InputField emailInput = null;
    [SerializeField] InputField passInput = null;

    public void Register()
    {
        if (emailInput.text == "a" && passInput.text == "a") // backdoor
            OnRegisterCallback(true);
        else
            APICaller.Register(emailInput.text, passInput.text, OnRegisterCallback);
    }

    void OnRegisterCallback(bool success)
    {
        if (success)
            pageManager.SwitchPage(0);
    }
}
