using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApartmentUI : MonoBehaviour
{
    [SerializeField] Text apartmentName = null; 

    public void LoadApartment(Apartment apartment)
    {
        apartmentName.text = apartment.name;
    }
}
