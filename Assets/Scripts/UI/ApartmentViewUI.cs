using UnityEngine;
using UnityEngine.UI;

public class ApartmentViewUI : MonoBehaviour
{
    [SerializeField] Text apartmentName = null;
    [SerializeField] Text apartmentInfo = null;

    [SerializeField] PageManager pageManager = null;

    [SerializeField] Dropdown stateInput = null;

    public static ApartmentViewUI singleton;

    private void Awake()
    {
        singleton = this;
    }

    Apartment viewApartment;
    public void ShowApartmentInfo(Apartment ap)
    {
        viewApartment = new Apartment(ap);

        apartmentName.text = ap.name;
        apartmentInfo.text = "Address: " + ap.address 
            + "\nDescription: " + ap.description
            + "\nStatus: " + ap.apartmentStatus.ToString();

        pageManager.SwitchPage(5);
    }

    public void ChangeState()
    {
        APICaller.ChangeApartmentStatus(viewApartment.id, (ApartmentStatus)stateInput.value, OnApartmentModify);
    }

    void OnApartmentModify(bool success)
    {
        if (success)
        {
            viewApartment.apartmentStatus = (ApartmentStatus)stateInput.value;
            ApartmentLoader.singleton.ModifyApartment(viewApartment);
            ShowApartmentInfo(viewApartment);
        }
    }
}
