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
public class Apartment
{
    public string name;
    public string description;
    public string address;
    public float floorArea; // m^2
    public int floor;
    public ApartmentStatus apartmentStatus;

    Texture2D picture;

    public static Apartment GenerateApartment()
    {
        Apartment newAp = new Apartment
        {
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
}

[System.Serializable]
public class ApartmentBundle
{
    public Apartment[] apartments;
}
