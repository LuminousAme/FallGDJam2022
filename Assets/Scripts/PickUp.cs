using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] int objectNumber;
    [SerializeField] GameObject objectGO;

    private void OnTriggerEnter(Collider other)
    {
        ProgressManager.instance.Aquire(objectNumber);
        objectGO.SetActive(false);
    }
}
