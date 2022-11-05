using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterUI : MonoBehaviour
{
    [SerializeField] Dropdown status = null;

    public static FilterUI singleton;

    public ApartmentStatus GetFilterApartmentStatus()
    {
        return (ApartmentStatus)status.value;
    }

    private void Awake()
    {
        singleton = this;
    }
}
