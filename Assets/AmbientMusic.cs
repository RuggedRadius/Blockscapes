using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientMusic : MonoBehaviour
{
    public AudioClip[] tracks;
    public bool playRandomTrack;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    void Start()
    {
        StartCoroutine(playlist());
    }

    private IEnumerator playlist()
    {
        int index = -1;
        if (playRandomTrack)
        {
            index = Random.Range(0, tracks.Length);
        }

        while (this.gameObject.activeSelf)
        {
            // Load next track
            index++;
            if (index >= tracks.Length)
            {
                index = 0;
            }
            audioSource.clip = tracks[index];

            // Play track 
            audioSource.Play();

            // wait for track to finish
            while (audioSource.isPlaying)
            {
                yield return new WaitForSeconds(1f);
            }

            yield return null;
        }
        yield return null;
    }
}
