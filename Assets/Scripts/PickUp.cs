using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] int objectNumber;
    [SerializeField] GameObject objectGO;
    [SerializeField] AudioSource audiosource;

    private void Start()
    {
        if(ProgressManager.instance.ObjectAquired(objectNumber)) objectGO.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (audiosource != null) audiosource.Play();
        ProgressManager.instance.Aquire(objectNumber);
        objectGO.SetActive(false);
    }
}
