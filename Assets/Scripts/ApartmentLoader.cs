using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ApartmentLoader : MonoBehaviour
{
    [SerializeField] TextAsset apartmentData = null;
    [SerializeField] GameObject prefab = null;
    [SerializeField] Transform apartmentHolder = null;

    void Start()
    {
        DisplayApartments(FetchApartments());
    }

    List<Apartment> FetchApartments()
    {
        return ((ApartmentBundle)FileReadWrite.ReadFromFile(apartmentData, typeof(ApartmentBundle))).apartments.ToList();
    }

    void DisplayApartments(List<Apartment> apartments)
    {
        for (int i = 0; i < apartments.Count; i++)
        {
            ApartmentUI apUI = Instantiate(prefab, apartmentHolder).GetComponent<ApartmentUI>();
            apUI.LoadApartment(apartments[i]);
        }
    }
}
