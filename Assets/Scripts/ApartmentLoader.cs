using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ApartmentLoader : MonoBehaviour
{
    [SerializeField] TextAsset apartmentData = null;
    [SerializeField] GameObject prefab = null;
    [SerializeField] Transform apartmentHolder = null;

    List<Apartment> apartmentList = new List<Apartment>();
    List<GameObject> apartmentElements = new List<GameObject>();

    void Start()
    {
        FetchApartments();
    }

    void FetchApartments()
    {
        //return ((ApartmentBundle)FileReadWrite.ReadFromFile(apartmentData, typeof(ApartmentBundle))).apartments.ToList();
        APICaller.GetApartments(OnApartmentFetch);
    }

    void OnApartmentFetch(List<Apartment> apartments)
    {
        Debug.Log("Amount of aps: {" + apartments.Count + "}");
        DisplayApartments(apartments);
    }

    void DisplayApartments(List<Apartment> apartments)
    {
        for (int i = 0; i < apartments.Count; i++)
        {
            ApartmentUI apUI = Instantiate(prefab, apartmentHolder).GetComponent<ApartmentUI>();
            apUI.LoadApartment(apartments[i]);
            apartmentElements.Add(apUI.gameObject);
        }
    }

    public void FilterApartments()
    {
        ClearApartments();
        ApartmentStatus apartmentStatus = FilterUI.singleton.GetFilterApartmentStatus();
        List<Apartment> filteredApartments = new List<Apartment>(apartmentList);
        if(apartmentStatus != ApartmentStatus.None)
            for (int i = 0; i < filteredApartments.Count; i++)
                if (filteredApartments[i].apartmentStatus != apartmentStatus)
                {
                    filteredApartments.RemoveAt(i);
                    i--;
                }
        DisplayApartments(filteredApartments);
    }

    void ClearApartments()
    {
        for(int i = 0; i < apartmentElements.Count; i++)
            Destroy(apartmentElements[i]);
        apartmentElements.Clear();
    }

    public void SortApartments()
    {
        ClearApartments();
        ApartmentSortingOption apartmentSortingOption = SortUI.singleton.GetSortApartmentOption();
        List<Apartment> sortedApartments = new List<Apartment>(apartmentList);
        if (apartmentSortingOption == ApartmentSortingOption.NameAscending)
            sortedApartments.Sort();
        else if (apartmentSortingOption == ApartmentSortingOption.NameDescending)
            sortedApartments.Sort((precedingApartment, succeedingApartment) => succeedingApartment.CompareTo(precedingApartment));
        DisplayApartments(sortedApartments);
    }
}
