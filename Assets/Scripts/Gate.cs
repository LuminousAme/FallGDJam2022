using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] GameObject gateGO;

    // Start is called before the first frame update
    void Start()
    {
        if (ProgressManager.instance.GetGateDone()) gateGO.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(ProgressManager.instance.ObjectAquired(2))
        {
            ProgressManager.instance.SetGateDone(true);
            gateGO.SetActive(false);
        }
    }
}
