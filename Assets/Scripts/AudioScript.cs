using UnityEngine;

public class AudioScript : MonoBehaviour {
    public AudioClip[] music;
    public AudioClip[] soundFX;

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        audioSource = GetComponent<AudioSource>();

        if (gameObject.name == "Music") {
            audioSource.clip = music[0];
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update() {
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
    }

    public void ChangeMusic(string type) {
        if (type == "Title") {
            audioSource.clip = music[0];
        }
        else if (type == "Level") {
            audioSource.clip = music[1];
        }
    }

    public void PlaySound(string type) { 
        if (type == "Step") {
            int num = Random.Range(0, 11);
            audioSource.PlayOneShot(soundFX[num]);
        }
        else if (type == "Goal") {
            audioSource.PlayOneShot(soundFX[11]);
        }
        else if (type == "Win") {
            audioSource.PlayOneShot(soundFX[12]);
        }
    }
}
