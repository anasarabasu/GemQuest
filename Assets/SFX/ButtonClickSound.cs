using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickSound : MonoBehaviour{
    [SerializeField] AudioClip click;
    [SerializeField] AudioSource audioSource;
    public void _Click () {
        audioSource.PlayOneShot(click);
    }    // Start is called before the first frame update
}
