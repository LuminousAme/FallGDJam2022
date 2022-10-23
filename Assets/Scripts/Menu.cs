using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] int trackNum = -1;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if(trackNum != -1 && MusicManager.instance != null) MusicManager.instance.PlayTrack(trackNum);
    }

    public void Restart()
    {
        if (ProgressManager.instance != null) ProgressManager.instance.Restart();
        Continue();
    }

    public void Continue()
    {
        if(MusicManager.instance != null) MusicManager.instance.Stop();
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
