using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagesManager : MonoBehaviour
{
    public static MessagesManager singleton;

    private void Awake()
    {
        singleton = this;
    }

    float waitTime = 0;
    private void Update()
    {
        if (UserLogInData.userEmail == "" || !UserLogInData.isUserLoggedIn)
            return;

        if (waitTime <= 0f)
        {
            waitTime = 15f; // Timeout time
            APICaller.FetchMessages(UserLogInData.userEmail, OnFetchMessages);
        }
        else
            waitTime -= Time.deltaTime;
    }

    void OnFetchMessages(bool success, APICaller.APIMessage[] messages)
    {
        waitTime = 6f; // Update time

        if (!success)
        { 
            Debug.LogWarning("Failed to receive messages");
            return;
        }

        if (messages == null || messages.Length == 0 || messages[0] == null)
        {
            Debug.Log("No messages...");
            return; 
        }

        NotificationManager.SendNotification("Apartment has been cleaned");
        //Debug.Log("Notification would be sent!");

        APICaller.DeleteMessages(UserLogInData.userEmail);
    }
}
