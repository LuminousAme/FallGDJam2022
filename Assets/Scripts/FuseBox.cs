using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FuseBox : MonoBehaviour
{
    [SerializeField] TriggerVideo v1;

    private void Start()
    {
        v1.OnVideoFinished += Win;
    }

    private void OnTriggerEnter(Collider other)
    {
        v1.gameObject.SetActive(true);
    }

    private void Win()
    {
        SceneManager.LoadScene("You Win");
    }
}
