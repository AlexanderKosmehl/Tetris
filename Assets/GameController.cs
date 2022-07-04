using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        // Initialize references to board and spawner
        m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING! There is no game board defined!");
        }

        if (!m_spawner)
        {
            Debug.LogWarning("WARNING! There is no spawner defined!");
        }

        // Place spawner and spawn first shape
        m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
        m_activeShape = m_spawner.SpawnShape();

        // Initialize key input timers
        m_timeToNextKeyLeftRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;
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
            }
        }

        else if (Input.GetButtonDown("MoveLeft") || Input.GetButton("MoveLeft") && (Time.time > m_timeToNextKeyLeftRight))
        {
            m_activeShape.MoveLeft();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
            }
        }

        else if (Input.GetButtonDown("Rotate") && (Time.time > m_timeToNextKeyRotate))
        {
            m_activeShape.RotateRight();
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;
            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.RotateLeft();
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
        m_gameBoard.StoreShapeInGrid(m_activeShape);

        m_activeShape = m_spawner.SpawnShape();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_spawner || !m_gameBoard || !m_activeShape) return;

        PlayerInput();

        if (Time.time > m_timeToDrop)
        {

        }
    }
}
