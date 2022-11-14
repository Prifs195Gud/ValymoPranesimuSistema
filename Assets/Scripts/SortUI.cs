using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ApartmentSortingOption
{
    None,
    NameAscending,
    NameDescending,
}

public class SortUI : MonoBehaviour
{
    [SerializeField] Dropdown sortingOption;

    public static SortUI singleton;

    public ApartmentSortingOption GetSortApartmentOption()
    {
        return (ApartmentSortingOption)sortingOption.value;
    }

    private void Awake()
    {
        singleton = this;
    }
}
