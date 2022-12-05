using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApartmentListUI : MonoBehaviour
{
    [SerializeField] LayoutGroup scrollLayout = null;
    [SerializeField] GameObject[] scrollLockObjs = new GameObject[0];

    [SerializeField] float mouseScrollSpeed = 1f;

    Vector3 scrollObjLocalOrigin;
    private void Awake()
    {
        scrollObjLocalOrigin = scrollLayout.transform.localPosition;
    }

    private void Update()
    {
        HandleScroll();
    }

    void HandleScroll()
    {
        for (int i = 0; i < scrollLockObjs.Length; i++)
            if (scrollLockObjs[i].activeSelf)
                return;

        Vector3 pos = scrollLayout.transform.localPosition;

        // Mouse scrollwheel
        pos.y -= Input.GetAxis("Mouse ScrollWheel") * mouseScrollSpeed * Time.deltaTime;

        // Android touch
        if(Input.touchCount > 0)
            pos.y += Input.touches[0].deltaPosition.y;

        float offset = scrollLayout.preferredHeight - 1750f;
        float maxPos = scrollObjLocalOrigin.y + Mathf.Clamp(offset, 0, offset);

        if (pos.y < scrollObjLocalOrigin.y)
            pos.y = scrollObjLocalOrigin.y;
        else if (pos.y > maxPos)
            pos.y = maxPos;

        scrollLayout.transform.localPosition = pos;
    }

    public void LogOff()
    {
        UserLogInData.isUserLoggedIn = false;
        UserLogInData.userEmail = "";
    }
}
