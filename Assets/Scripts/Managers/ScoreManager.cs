using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    int m_score = 0;
    int m_lines;
    int m_level = 1;

    public int m_linesPerLevel = 5;

    const int m_minLines = 1;
    const int m_maxLines = 4;

    public Text m_linesText;
    public Text m_levelText;
    public Text m_scoreText;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ScoreLines(int n)
    {
        n = Mathf.Clamp(n, m_minLines, m_maxLines);

        switch (n)
        {
            case 1:
                m_score += 40 * m_level;
                break;
            case 2:
                m_score += 100 * m_level;
                break;
            case 3:
                m_score += 300 * m_level;
                break;
            case 4:
                m_score += 1200 * m_level;
                break;
        }

        UpdateUIText();
    }

    void Reset()
    {
        m_level = 1;
        m_lines = m_linesPerLevel * m_level;
    }

    void UpdateUIText()
    {
        if (!m_linesText || !m_levelText || !m_scoreText) return;

        m_linesText.text = m_lines.ToString();
        m_levelText.text = m_level.ToString();
        m_scoreText.text = string.Format("{0:00000}", m_score);
    }
}
