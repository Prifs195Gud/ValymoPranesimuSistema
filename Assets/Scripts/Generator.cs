using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[ExecuteInEditMode]
public class Generator : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField, Range(1, 10)] int aparmentsToGenerate = 3;
    [SerializeField] bool generateApartments = false;

    void Update()
    {
        if(generateApartments)
        {
            generateApartments = false;
            GenerateAparments();
        }
    }

    void GenerateAparments()
    {
        ApartmentBundle aps = new ApartmentBundle();
        aps.apartments = Apartment.GenerateApartmentList(aparmentsToGenerate).ToArray();
        FileReadWrite.WriteToFile(aps, "Apartments");
        Debug.Log("Apartments generated!");
    }
#endif
}
