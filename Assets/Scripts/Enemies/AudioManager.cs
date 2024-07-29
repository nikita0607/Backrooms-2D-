using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource _source;
    [SerializeField] private AudioClip[] _audios;


    private void Awake() {
        _source = GetComponent<AudioSource>();
    }

       // Update is called once per frame
    public void PlaySound(int index) {
        _source.PlayOneShot(_audios[index]);
    }
}
