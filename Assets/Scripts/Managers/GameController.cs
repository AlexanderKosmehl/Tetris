using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Reference to board
    Board m_gameBoard;

    // Reference to the terimino spawner
    Spawner m_spawner;

    // Reference to the current active shape the user can move
    Shape m_activeShape;

    // Drop Rate of the piece if no input happens
    public float m_dropInterval = 0.75f;
    float m_timeToDrop;

    // Timing settings
    float m_timeToNextKeyLeftRight;

    [Range(0.02f, 1f)]
    public float m_keyRepeatRateLeftRight = 0.25f;

    float m_timeToNextKeyDown;

    [Range(0.02f, 1f)]
    public float m_keyRepeatRateDown = 0.02f;

    float m_timeToNextKeyRotate;

    [Range(0.02f, 1f)]
    public float m_keyRepeatRateRotate = 0.2f;

    bool m_gameOver = false;

    public GameObject m_gameOverPanel;

    SoundManager m_soundManager;

    void Start()
    {
        // Initialize references to board and spawner
        m_gameBoard = GameObject.FindObjectOfType<Board>();
        m_spawner = GameObject.FindObjectOfType<Spawner>();
        m_soundManager = GameObject.FindObjectOfType<SoundManager>();

        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING! There is no game board defined!");
        }

        if (!m_spawner)
        {
            Debug.LogWarning("WARNING! There is no spawner defined!");
        }

        if (!m_soundManager)
        {
            Debug.LogWarning("WARNING! There is no sound manager defined!");
        }

        // Place spawner and spawn first shape
        m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
        m_activeShape = m_spawner.SpawnShape();

        // Initialize key input timers
        m_timeToNextKeyLeftRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;

        if (!m_gameOverPanel) return;

        m_gameOverPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (!m_spawner || !m_gameBoard || !m_activeShape || m_gameOver || !m_soundManager) return;

        PlayerInput();
    }

    void PlaySound(AudioClip clip, float volMultiplier = 1)
    {
        if (!clip || !m_soundManager.m_fxEnabled) return;

        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(m_soundManager.m_fxVolume * volMultiplier, 0.05f, 1f));
    }

    void PlayerInput()
    {
        if (Input.GetButtonDown("MoveRight") || Input.GetButton("MoveRight") && (Time.time > m_timeToNextKeyLeftRight))
        {
            m_activeShape.MoveRight();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveLeft();
                PlaySound(m_soundManager.m_errorSound, 0.5f);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound, 0.5f);
            }
        }

        else if (Input.GetButtonDown("MoveLeft") || Input.GetButton("MoveLeft") && (Time.time > m_timeToNextKeyLeftRight))
        {
            m_activeShape.MoveLeft();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
                PlaySound(m_soundManager.m_errorSound, 0.5f);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound, 0.5f);
            }
        }

        else if (Input.GetButtonDown("Rotate") && (Time.time > m_timeToNextKeyRotate))
        {
            m_activeShape.RotateRight();
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;
            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.RotateLeft();
                PlaySound(m_soundManager.m_errorSound, 0.5f);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound, 0.5f);
            }
        }

        else if (Input.GetButton("MoveDown") && (Time.time > m_timeToNextKeyDown) || (Time.time > m_timeToDrop))
        {
            m_timeToDrop = Time.time + m_dropInterval;
            m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
            m_activeShape.MoveDown();

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                LandShape();
            }
        }


    }

    private void LandShape()
    {
        m_activeShape.MoveUp();

        // Check if the shape is over the limit after landing
        if (m_gameBoard.IsOverLimit(m_activeShape))
        {
            GameOver();
            return;
        }

        m_gameBoard.StoreShapeInGrid(m_activeShape);
        m_activeShape = m_spawner.SpawnShape();

        // Reset key input timers
        m_timeToNextKeyLeftRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;

        int completedRowCount = m_gameBoard.ClearAllCompletedRows();

        PlaySound(m_soundManager.m_dropSound, 0.75f);

        if (completedRowCount > 0) 
        {
            if (completedRowCount > 1)
            {
                AudioClip randomVocal = m_soundManager.GetRandomClip(m_soundManager.m_vocalClips);
                PlaySound(randomVocal);
            }

            PlaySound(m_soundManager.m_clearRowSound);
        }
    }

    public void Restart()
    {
        Debug.Log("Restarted");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        Debug.LogWarning(m_activeShape.name + " is over the limit");
        m_gameOver = true;

        if (!m_gameOverPanel) return;

        PlaySound(m_soundManager.m_gameOverVocal);
        PlaySound(m_soundManager.m_gameOverSound, 5f);

        m_gameOverPanel.SetActive(true);
    }
}
