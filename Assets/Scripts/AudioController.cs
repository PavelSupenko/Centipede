using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    [SerializeField] private AudioClip _fightHard;
    [SerializeField] private AudioClip _fightSorf;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    private void Start () {
        OnStart();
    }

    public void OnStart()
    {
        _audioSource.clip = _fightSorf;
        _audioSource.Play();
    }

    public void OnPlay()
    {
        _audioSource.clip = _fightHard;
        _audioSource.Play();
    }
}
