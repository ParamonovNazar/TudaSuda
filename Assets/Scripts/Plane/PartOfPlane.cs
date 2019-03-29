using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfPlane : MonoBehaviour
{
    private PlaneController plane;

    public PlaneController Plane
    {
        get => plane;
        set => plane = value;
    }
}

