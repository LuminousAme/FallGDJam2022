using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStatue : MonoBehaviour
{
    bool firstInteractionDone = false;
    [SerializeField] GameObject statueGO;
    [SerializeField] int objectNumber;
    [SerializeField] TriggerVideo v1, v2;

    // Start is called before the first frame update
    void Start()
    {
        ObjectStatueProgress prog = ProgressManager.instance.GetObjectStatueProgress(objectNumber);
        firstInteractionDone = prog.firstInteractionDone;
        if(prog.secondInteractionDone) statueGO.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!firstInteractionDone)
        {
            v1.gameObject.SetActive(true);
            firstInteractionDone = true;
            ProgressManager.instance.FinishFirstObjectStatueInteraction(objectNumber);
        }
        else if (ProgressManager.instance.ObjectAquired(objectNumber))
        {
            v2.gameObject.SetActive(true);
            ProgressManager.instance.FinishSecondObjectStatueInteraction(objectNumber);
            statueGO.SetActive(false);
        }
    }
}

public class ObjectStatueProgress
{
    public bool firstInteractionDone;
    public bool secondInteractionDone;

    public ObjectStatueProgress(bool a, bool b)
    {
        firstInteractionDone = a;
        secondInteractionDone = b;
    }
}
