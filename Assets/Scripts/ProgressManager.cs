using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance;
    static bool[] objs = { false, false, false }; //book, pineapple, key
    static ObjectStatueProgress[] objectStatues = { new ObjectStatueProgress(false, false), new ObjectStatueProgress(false, false) };
    static bool GateDone = false;
    [SerializeField] float maxOil;
    static float currentOil;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        Restart();
        DontDestroyOnLoad(gameObject);
    }

    public void Restart()
    {
        for (int i = 0; i < objs.Length; i++) objs[i] = false;
        for (int i = 0; i < objectStatues.Length; i++)
        {
            objectStatues[i].firstInteractionDone = false;
            objectStatues[i].secondInteractionDone = false;
        }
        GateDone = false;
        currentOil = maxOil;
    }

    public bool ObjectAquired(int obj)
    {
        if (obj > 2 || obj < 0) return false;
        return objs[obj];
    }

    public void Aquire(int obj)
    {
        if (obj > 2 || obj < 0) return;
        objs[obj] = true;
    }

    public ObjectStatueProgress GetObjectStatueProgress(int objectNumber) => objectStatues[objectNumber];

    public void FinishFirstObjectStatueInteraction(int objectNumber)
    {
        objectStatues[objectNumber].firstInteractionDone = true;
    }

    public void FinishSecondObjectStatueInteraction(int objectNumber)
    {
        objectStatues[objectNumber].secondInteractionDone = true;
    }

    public bool GetGateDone() => GateDone;
    public void SetGateDone(bool done) => GateDone = done;

    public float GetCurrentOil() => currentOil;
    public float GetMaxOil() => maxOil;
}
