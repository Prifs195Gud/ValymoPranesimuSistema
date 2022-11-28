using UnityEngine;
using UnityEngine.UI;

public class ApartmentUI : MonoBehaviour
{
    [SerializeField] Text apartmentName = null;

    Apartment myApartment;
    public void LoadApartment(Apartment apartment)
    {
        myApartment = apartment;
        apartmentName.text = apartment.name;
    }

    public void OpenView()
    {
        if (myApartment == null)
            return;

        ApartmentViewUI.singleton.ShowApartmentInfo(myApartment);
    }
}
