using System.Collections.Generic;

using UnityEngine;

public enum ApartmentStatus
{
    None,
    Ready, // Clean
    NotReady, // Not clean
    Occupied,
    Blocked, // Removed from use
    BeingCleaned
}

[System.Serializable]
public class Apartment : System.IComparable<Apartment>
{
    public int id;

    public string name;
    public string description;
    public string address;
    public float floorArea; // m^2
    public int floor;
    public ApartmentStatus apartmentStatus;

    Texture2D picture;

    public Apartment()
    {
    }

    public Apartment(Apartment ap)
    {
        id = ap.id;
        name = ap.name;
        description = ap.description;
        address = ap.address;
        floorArea = ap.floorArea;
        floor = ap.floor;
        apartmentStatus = ap.apartmentStatus;
        picture = ap.picture;
    }

    public static Apartment GenerateApartment()
    {
        Apartment newAp = new Apartment
        {
            id = 0,
            name = "TEST AP #" + Random.Range(0, 1000),
            description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas quis orci eros. Ut odio arcu, commodo et dui et, imperdiet congue arcu. Proin id odio luctus, iaculis justo at, sodales mi. Duis feugiat, augue ut pharetra tincidunt, sapien sem maximus erat, ac blandit elit neque a nibh. Curabitur vehicula lacus sed elit sollicitudin, sed vehicula metus volutpat.",
            address = "Lorem ipsum dolor " + Random.Range(1, 100) + " g.",
            floorArea = Random.Range(10, 100),
            floor = Random.Range(1, 10),
            apartmentStatus = (ApartmentStatus)(Random.Range(1, 5)),
            picture = null

        };

        return newAp;
    }

    public static List<Apartment> GenerateApartmentList(int amount)
    {
        List<Apartment> newList = new List<Apartment>();

        for (int i = 0; i < amount; i++)
            newList.Add(GenerateApartment());

        return newList;
    }

    public int CompareTo(Apartment compareApartment)
    {
        if (name == null || name == "")
            return 1;
        else
            return name.CompareTo(compareApartment.name);
    }

    public static Apartment ReadFromAPI(APIApartment ap)
    {
        Apartment newAp = new Apartment
        {
            id = ap.apartmentId,
            name = ap.name,
            description = ap.description,
            address = ap.address,
            floorArea = ap.floorArea,
            floor = ap.floor,
            apartmentStatus = (ApartmentStatus)ap.status,
            picture = null
        };

        return newAp;
    }

    public static APIApartment WriteAPIObject(Apartment ap)
    {
        APIApartment newAp = new APIApartment
        {
            apartmentId = ap.id,
            name = ap.name,
            description = ap.description,
            address = ap.address,
            floorArea = ap.floorArea,
            floor = ap.floor,
            status = (int)ap.apartmentStatus
        };

        return newAp;
    }
}

[System.Serializable]
public class ApartmentBundle
{
    public Apartment[] apartments;
}

[System.Serializable]
public class APIApartment
{
    public int apartmentId;
    public string name;
    public string description;
    public string address;
    public float floorArea;
    public int floor;
    public int status;
}

[System.Serializable]
public class APIApartmentBundle
{
    public APIApartment[] APIApartments;
}
