using UnityEngine;
using System.Collections;

public class VideoPlay : MonoBehaviour
{
    public MovieTexture movie;

	void Start ()
    {
        GetComponent<Renderer>().material.mainTexture = movie;
        movie.loop = true;
        GetComponent<AudioSource>().clip = movie.audioClip;
        movie.Play();
        GetComponent<AudioSource>().Play();
	}
}
