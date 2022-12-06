using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    [SerializeField] Dropdown languageDropdown = null;
    [SerializeField] LanguageData[] languageData = new LanguageData[0];

    [System.Serializable]
    struct LanguageKeyPair
    {
        public string key;
        [TextArea(3,10)] public string text;

        public override string ToString()
        {
            return key;
        }
    }

    [System.Serializable]
    struct LanguageData
    {
        public string languageName;
        public LanguageKeyPair[] languageData;

        public override string ToString()
        {
            return languageName;
        }
    }

    public static LanguageManager singleton;

    private void Awake()
    {
        singleton = this;

        string key;
        for (int i = 0; i < languageData.Length; i++)
        {
            string err = "Duplicate keys for language " + languageData[i].languageName + ":\n";
            bool errHappened = false;
            Language newLang = new Language();

            for (int j = 0; j < languageData[i].languageData.Length; j++)
            {
                key = languageData[i].languageData[j].key;

                if(newLang.keyToText.ContainsKey(key))
                {
                    err += key + "\n";
                    errHappened = true;
                    continue;
                }

                newLang.keyToText.Add(key, languageData[i].languageData[j].text);
            }

            languages.Add(newLang);

            if(errHappened)
                Debug.LogWarning(err);
        }

        if(languages.Count > 0)
            currentLanguage = languages[0];
    }

    List<Language> languages = new List<Language>();
    Language currentLanguage = null;
    public void ApplyLanguage(int which)
    {
        currentLanguage = languages[which];

        string key, err="Didn't have data for:\n";
        bool errHappened = false;
        for (int i = 0; i < languageChildren.Count; i++)
        {
            key = languageChildren[i].GetLanguageKey();
            if (currentLanguage.keyToText.ContainsKey(key))
                languageChildren[i].ApplyLanguage(currentLanguage.keyToText[key]);
            else
            {
                err += key + "\n";
                errHappened = true;
            }
        }

        if (errHappened)
            Debug.LogWarning(err);
    }

    public string FetchLanguageText(string key)
    {
        if (currentLanguage.keyToText.ContainsKey(key))
            return currentLanguage.keyToText[key];
        else
            return "";
    }

    List<LanguageChild> languageChildren = new List<LanguageChild>();
    public static void RegisterLanguageChild(LanguageChild child)
    {
        singleton.languageChildren.Add(child);
    }

    class Language
    {
        public Dictionary<string, string> keyToText = new Dictionary<string, string>();
    }
}
