using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelUI : MonoBehaviour
{
    [SerializeField] GameObject graphics = null;

    private void Awake()
    {
        Enable(false);
    }

    public void Enable(bool var)
    {
        graphics.SetActive(var);
    }
}
