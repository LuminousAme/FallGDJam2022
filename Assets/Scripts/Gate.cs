using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] GameObject gateGO;
    [SerializeField] AudioSource audiosource;

    // Start is called before the first frame update
    void Start()
    {
        if (ProgressManager.instance.GetGateDone()) gateGO.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(ProgressManager.instance.ObjectAquired(2))
        {
            if (audiosource != null) audiosource.Play();
            ProgressManager.instance.SetGateDone(true);
            gateGO.SetActive(false);
        }
    }
}
