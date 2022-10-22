using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPath : MonoBehaviour
{
    [SerializeField] List<Transform> wayPoints;
    [SerializeField] float speed = 1f;

    private int currentIndex = 0, nextIndex = 0;
    private float totalTime = 0f;
    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        NewIndices();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime >= totalTime) NewIndices();

        Vector3 p1 = wayPoints[currentIndex].position;
        Vector3 p2 = wayPoints[nextIndex].position;
        //Vector3 dir = (p2 - p1).normalized;

        float t = currentTime / totalTime;
        transform.position = Vector3.Lerp(p1, p2, t);

        transform.LookAt(p2);

        currentTime += Time.deltaTime;
    }

    private void NewIndices()
    {
        currentIndex++;
        if (currentIndex >= wayPoints.Count) currentIndex = 0;
        nextIndex = currentIndex + 1;
        if (nextIndex >= wayPoints.Count) nextIndex = 0;

        //v = d/t; v = speed, d = distance, t = time //d = vt // 
        float distance = Vector3.Distance(wayPoints[currentIndex].position, wayPoints[nextIndex].position);
        totalTime = distance / speed;

        currentTime = 0f;
    }
}
