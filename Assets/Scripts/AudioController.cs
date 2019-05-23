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
        Messenger.AddListener(EventStrings.START_SOFT_MUSIC, StartSoft);
        Messenger.AddListener(EventStrings.START_HARD_MUSIC, StartHard);
        Messenger<AudioClip>.AddListener(EventStrings.PLAY_ONE_MUSIC, PlayOneShot);

        StartSoft();
    }

    public void PlayOneShot(AudioClip audio)
    {
        _audioSource.PlayOneShot(audio);
    }

    public void StartSoft()
    {
        _audioSource.clip = _fightSorf;
        _audioSource.Play();
    }

    public void StartHard()
    {
        _audioSource.clip = _fightHard;
        _audioSource.Play();
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(EventStrings.START_SOFT_MUSIC, StartSoft);
        Messenger.RemoveListener(EventStrings.START_HARD_MUSIC, StartHard);
        Messenger<AudioClip>.RemoveListener(EventStrings.PLAY_ONE_MUSIC, PlayOneShot);
    }
}
