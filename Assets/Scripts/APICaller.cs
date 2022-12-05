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
        data.user = new APIUser(Random.Range(0, int.MaxValue - 100), email, pass, "", "", 0, 0);
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
        public int userType;

        public APIUser() { }
        public APIUser(int userId, string email, string password, string name, string surname, int phonenumber, int userType)
        {
            this.userId = userId;
            this.email = email;
            this.password = password;
            this.name = name;
            this.surname = surname;
            this.phonenumber = phonenumber;
            this.userType = userType;
        }
    }

    public delegate void OnFetchMessages(bool success, APIMessage[] messages);
    OnFetchMessages onFetchMessages;
    public static void FetchMessages(string email, OnFetchMessages callback)
    {
        singleton.onFetchMessages = callback;

        singleton.StartCoroutine(nameof(singleton.FetchMessagesEnum), singleton.apiUrl + "/api/NotifMessage/ReceiveMessages?receiverEmail=" + email);
    }

    IEnumerator FetchMessagesEnum(string url)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);

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

                string downloaderText = webRequest.downloadHandler.text;

                if (downloaderText == "" || downloaderText.Length < 10)
                { 
                    onFetchMessages.Invoke(true, null);
                    break;
                }

                string downloadedData = "{\"messages\":" + downloaderText + "}";

                try
                {
                    APIMessagesBundle bundle = (APIMessagesBundle)JsonUtility.FromJson(downloadedData, typeof(APIMessagesBundle));
                    Debug.Log(JsonUtility.ToJson(bundle));

                    if(bundle != null && bundle.messages != null)
                        onFetchMessages.Invoke(true, bundle.messages);
                    else
                        onFetchMessages.Invoke(false, null); // FAIL

                    break;
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning(ex);
                }

                onFetchMessages.Invoke(false, null); // FAIL

                break;
        }
    }

    [System.Serializable]
    public class APIMessage
    { 
        public int messageId;
        public string messageText;
        public string sendersEmail;
        public string receiversEmail;
        public string messageCreationDateTime;
        public int messageApartmentId;
    }

    [System.Serializable]
    public class APIMessagesBundle
    {
        public APIMessage[] messages;
    }

    public static void DeleteMessages(string email)
    {
        singleton.StartCoroutine(nameof(singleton.DeleteMessagesEnum), singleton.apiUrl + "/api/NotifMessage/DeleteMessageReceiver?receiverEmail=" + email);
    }

    IEnumerator DeleteMessagesEnum(string url)
    {
        UnityWebRequest webRequest = UnityWebRequest.Delete(url);

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
                break;
        }
    }

    public delegate void OnModifyApartment(bool success);
    OnModifyApartment onModifyApartment;

    struct ModifyApartmentStruct
    {
        public APIApartment ap;
        public string url;
    }

    public static void ModifyApartmentData(Apartment ap, OnModifyApartment callback)
    {
        if(ap == null)
        {
            Debug.LogError("Modify apartment is null!");
            return;
        }

        singleton.onModifyApartment = callback;

        ModifyApartmentStruct data;
        data.url = singleton.apiUrl + "/api/Apartment/"+ ap.id;
        data.ap = Apartment.WriteAPIObject(ap);
        singleton.StartCoroutine(nameof(singleton.ModifyApartmentDataEnum), data);
    }

    IEnumerator ModifyApartmentDataEnum(ModifyApartmentStruct data)
    {
        string json = JsonUtility.ToJson(data.ap);

        UnityWebRequest webRequest = UnityWebRequest.Put(data.url, json);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

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
                break;
        }

        switch (webRequest.result)
        {
            default:
                onModifyApartment.Invoke(false);
                break;
            case UnityWebRequest.Result.Success:
                onModifyApartment.Invoke(true);
                break;
        }
    }

    public delegate void OnChangeStatusApartment(bool success);
    OnChangeStatusApartment onChangeStatus;

    public static void ChangeApartmentStatus(int apartmentID, ApartmentStatus changeToStatus, OnChangeStatusApartment callback)
    {
        singleton.onChangeStatus = callback;

        string url = singleton.apiUrl + "/api/Apartment/ChangedStatus?apartmID="+ apartmentID + "&statusChange=" + (int)changeToStatus;

        singleton.StartCoroutine(nameof(singleton.ChangeApartmentStatusEnum), url);
    }

    IEnumerator ChangeApartmentStatusEnum(string url)
    {
        UnityWebRequest webRequest = UnityWebRequest.Put(url, "");

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("");
        webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = new DownloadHandlerBuffer();

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
                break;
        }

        switch (webRequest.result)
        {
            default:
                onChangeStatus.Invoke(false);
                break;
            case UnityWebRequest.Result.Success:
                onChangeStatus.Invoke(true);
                break;
        }
    }
}
