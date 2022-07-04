using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public bool m_musicEnabled = true;
    public bool m_fxEnabled = true;

    [Range(0, 1)]
    public float m_musicVolume = 1.0f;

    [Range(0, 1)]
    public float m_fxVolume = 1.0f;

    public AudioClip m_clearRowSound;
    public AudioClip m_moveSound;
    public AudioClip m_errorSound;
    public AudioClip m_dropSound;
    public AudioClip m_gameOverSound;
    public AudioClip m_gameOverVocal;

    public AudioSource m_musicSource;

    public AudioClip[] m_musicClips;
    public AudioClip[] m_vocalClips;

    public IconToggle m_musicIconToggle;
    public IconToggle m_fxIconToggle;

    // Start is called before the first frame update
    void Start()
    {
        PlayBackgroundMusic(GetRandomClip(m_musicClips));
    }

    public AudioClip GetRandomClip(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (!m_musicEnabled || !musicClip || !m_musicSource) return;

        // Stop current song if its playing
        m_musicSource.Stop();

        // Configure background music
        m_musicSource.clip = musicClip;
        m_musicSource.volume = m_musicVolume;
        m_musicSource.loop = true;

        // Start new song
        m_musicSource.Play();
    }

    void UpdateMusic()
    {
        // Needs to update
        if (m_musicSource.isPlaying != m_musicEnabled)
        {
            // Turn it on or off depending on settings
            if (m_musicEnabled)
            {
                PlayBackgroundMusic(GetRandomClip(m_musicClips));
            }
            else
            {
                m_musicSource.Stop();
            }
        }
    }

    public void ToggleMusic()
    {
        m_musicEnabled = !m_musicEnabled;
        UpdateMusic();

        if (!m_musicIconToggle) return;

        m_musicIconToggle.ToggleIcon(m_musicEnabled);
    }

    public void ToggleFx()
    {
        m_fxEnabled = !m_fxEnabled;

        if (!m_fxIconToggle) return;

        m_fxIconToggle.ToggleIcon(m_fxEnabled);
    }
}
