using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour
{
    public static CreateManager instance;

    private void Awake()
    {
        instance = this;
    }
}
