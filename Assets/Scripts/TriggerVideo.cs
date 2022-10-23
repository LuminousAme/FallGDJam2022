using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

public class TriggerVideo : MonoBehaviour
{
    private VideoPlayer vp;
    PlayerController controller;
    Lantern lantern;

    public Action OnVideoFinished;

    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        controller = FindObjectOfType<PlayerController>();
        if (controller != null) controller.setControlsState(false);
        lantern = FindObjectOfType<Lantern>();
        if (lantern != null) lantern.gameObject.SetActive(false);
        if (MusicManager.instance != null) MusicManager.instance.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (vp.frame.ToString() == (vp.frameCount - 1).ToString())
        {
            // Put anything you want to happen after the video is done here.
            // For example Destroy(gameobject) if you just want to put this script on each video
            if (controller != null) controller.setControlsState(true);
            if (lantern != null) lantern.gameObject.SetActive(true);
            if (OnVideoFinished != null) OnVideoFinished.Invoke();
            else if (MusicManager.instance != null) MusicManager.instance.PlayTrack(0);
            Destroy(gameObject);
        }
    }
}
