using UnityEngine;


public class AudioController : MonoBehaviour {

    // Audio Clips for main menu and game process
    [SerializeField] private AudioClip _fightHard;
    [SerializeField] private AudioClip _fightSorf;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    // Adding event listeners and starting playing
    // the main menu music
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

    // Methods of changing music
    public void StartSoft()
    {
        _audioSource.clip = _fightSorf;
        StartPlaying();
    }

    public void StartHard()
    {
        _audioSource.clip = _fightHard;
        StartPlaying();
    }

    private void StartPlaying()
    {
        if (!_audioSource.isPlaying)
            _audioSource.Play();
    }

    // Removing listeners on destroy
    private void OnDestroy()
    {
        Messenger.RemoveListener(EventStrings.START_SOFT_MUSIC, StartSoft);
        Messenger.RemoveListener(EventStrings.START_HARD_MUSIC, StartHard);
        Messenger<AudioClip>.RemoveListener(EventStrings.PLAY_ONE_MUSIC, PlayOneShot);
    }
}
