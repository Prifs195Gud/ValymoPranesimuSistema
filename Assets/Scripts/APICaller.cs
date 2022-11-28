using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APICaller : MonoBehaviour
{
    [SerializeField] string apiUrl = "https://valymopranesimuapi20221122202627.azurewebsites.net";

    public static APICaller singleton;

    private void Awake()
    {
        singleton = this;
    }

    public delegate void OnApartmentFetchDelegate(List<Apartment> apartments);
    OnApartmentFetchDelegate GetApartmentsCallback;
    public static void GetApartments(OnApartmentFetchDelegate callback)
    {
        singleton.GetApartmentsCallback = callback;
        singleton.StartCoroutine(nameof(singleton.GetApartmentsEnum), singleton.apiUrl + "/api/Apartment");
    }

    IEnumerator GetApartmentsEnum(string url)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);

        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        string[] pages = url.Split('/');
        int page = pages.Length - 1;

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                string data = "{\"APIApartments\":" + webRequest.downloadHandler.text + "}";

                // Debug.Log(pages[page] + ":\nReceived: " + data);

                List<Apartment> apartments = new List<Apartment>();

                try
                {
                    APIApartmentBundle bundle = (APIApartmentBundle)JsonUtility.FromJson(data, typeof(APIApartmentBundle));
                    //Debug.Log(JsonUtility.ToJson(bundle));

                    foreach (APIApartment apiAp in bundle.APIApartments)
                        apartments.Add(Apartment.ReadFromAPI(apiAp));
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning(ex);
                }

                if (GetApartmentsCallback != null)
                    GetApartmentsCallback.Invoke(apartments);

                break;
        }
    }

    public delegate void OnSignInResponseDelegate(bool success);
    OnSignInResponseDelegate onSignInResponseDelegate;
    public static void CanSignIn(string id, string pass, OnSignInResponseDelegate callback)
    {
        singleton.onSignInResponseDelegate = callback;

        string url = singleton.apiUrl + "/api/User/login?email=" + id + "&password=" + pass;
        singleton.StartCoroutine(nameof(singleton.CanSignInEnum), url);
    }

    IEnumerator CanSignInEnum(string url)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);

        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("HTTP Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                //Debug.Log("Response: " + webRequest.responseCode);

                if (onSignInResponseDelegate != null)
                {
                    if(webRequest.responseCode == 204)
                        onSignInResponseDelegate.Invoke(false);
                    else
                        onSignInResponseDelegate.Invoke(true);
                }
                    
                break;
        }
    }


    struct RegisterData
    {
        public APIUser user;
        public string url;
    }

    public delegate void OnRegisterResponseDelegate(bool success);
    OnRegisterResponseDelegate onRegisterResponseDelegate;
    public static void Register(string email, string pass, OnRegisterResponseDelegate callback)
    {
        singleton.onRegisterResponseDelegate = callback;

        RegisterData data;
        data.user = new APIUser(Random.Range(0, int.MaxValue - 100), email, pass, "", "", 0);
        data.url = singleton.apiUrl + "/api/User/Register";

        singleton.StartCoroutine(nameof(singleton.RegisterEnum), data);
    }

    IEnumerator RegisterEnum(RegisterData data)
    {
        string json = JsonUtility.ToJson(data.user);

        //Debug.Log(json);

        UnityWebRequest webRequest = UnityWebRequest.Post(data.url, json);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("HTTP Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                //Debug.Log("Response: " + webRequest.responseCode);

                onRegisterResponseDelegate.Invoke(true);

                break;
        }
    }

    [System.Serializable]
    class APIUser
    {
        public int userId;
        public string email;
        public string password;
        public string name;
        public string surname;
        public int phonenumber;

        public APIUser() { }
        public APIUser(int userId, string email, string password, string name, string surname, int phonenumber)
        {
            this.userId = userId;
            this.email = email;
            this.password = password;
            this.name = name;
            this.surname = surname;
            this.phonenumber = phonenumber;
        }
    }
}
