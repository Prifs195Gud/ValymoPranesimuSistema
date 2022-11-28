using UnityEngine;
using UnityEngine.UI;

public class ApartmentViewUI : MonoBehaviour
{
    [SerializeField] Text apartmentName = null;
    [SerializeField] Text apartmentInfo = null;

    [SerializeField] PageManager pageManager = null;

    public static ApartmentViewUI singleton;

    private void Awake()
    {
        singleton = this;
    }

    public void ShowApartmentInfo(Apartment ap)
    {
        apartmentName.text = ap.name;
        apartmentInfo.text = "Address: " + ap.address 
            + "\nDescription: " + ap.description
            + "\nStatus: " + ap.apartmentStatus.ToString();

        pageManager.SwitchPage(5);
    }

    public void MarkForCleaning()
    {

    }
}
