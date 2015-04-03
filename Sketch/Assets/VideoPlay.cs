using UnityEngine;
using System.Collections;

public class VideoPlay : MonoBehaviour
{
    public MovieTexture movie;

    public GameObject door;
    private float timer = 15;

	void Start ()
    {
        GetComponent<Renderer>().material.mainTexture = movie;
        movie.loop = true;
        GetComponent<AudioSource>().clip = movie.audioClip;
        movie.Play();
        GetComponent<AudioSource>().Play();
	}

    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            door.SetActive(true);
    }
}
