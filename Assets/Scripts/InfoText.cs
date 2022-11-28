using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    [SerializeField] Text myText = null;
    [SerializeField] float timeOn = 2f;

    float timeLeft = 0f;

    private void Awake()
    {
        if(timeLeft <= 0f)
            myText.enabled = false;
    }

    public void Activate()
    {
        myText.enabled = true;
        timeLeft = timeOn;
        StartCoroutine(nameof(Tick));
    }

    IEnumerator Tick()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        myText.enabled = false;
    }

    private void OnDisable()
    {
        myText.enabled = false;
    }
}
