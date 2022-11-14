using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestScript
{
    [Test]
    public void SekmingasBustuSarasoPateikimas()
    {
        string json = JsonUtility.ToJson(Apartment.GenerateApartment());

        try
        {
            Apartment ap = (Apartment)JsonUtility.FromJson(json, typeof(Apartment));
        }
        catch(System.Exception ex)
        {
            Assert.Fail("Fail " + ex);
        }
    }

    [Test]
    public void NesekmingasBustuSarasoPateikimas()
    {
        string json = JsonUtility.ToJson(Apartment.GenerateApartment()) + "test";

        try
        {
            Apartment ap = (Apartment)JsonUtility.FromJson(json, typeof(Apartment));
            Assert.Fail("Fail, nebuvo klaidos");
        }
        catch
        {
            
        }
    }

    [Test]
    public void SekmingasFiltravimas()
    {
    }

    [Test]
    public void SekmingasRikiavimas()
    {
    }
}
