using UnityEngine;
using UnityEngine.UI;

public class LanguageChild : MonoBehaviour
{
    [SerializeField] Text myText = null;
    [SerializeField] string languageKey = "";

    private void Start()
    {
        LanguageManager.RegisterLanguageChild(this);
    }

    public void ApplyLanguage(string text)
    {
        myText.text = text;
    }

    public string GetLanguageKey()
    {
        return languageKey;
    }

    private void OnEnable()
    {
        string s = LanguageManager.singleton.FetchLanguageText(languageKey);

        if (s == "")
            return;

        myText.text = s;
    }
}
