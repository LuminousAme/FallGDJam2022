using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource source;
    [SerializeField] List<AudioClip> tracks;
    int currentTrack;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.Stop();
        currentTrack = -1;
    }

    public void PlayTrack(int tracknum)
    {
        if(currentTrack != tracknum || tracknum < 0 || tracknum > tracks.Count -1)
        {
            source.Stop();
            source.clip = tracks[tracknum];
            currentTrack = tracknum;
            source.Play();
        }
    }

    public void Stop()
    {
        source.Stop();
        currentTrack = -1;
    }
}
