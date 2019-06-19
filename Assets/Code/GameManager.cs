using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] float comboTime = 2;
    [SerializeField] int[] scoreRewards, scoreBonusses;

    int score, scoreBonus, lastKill = -1;
    float[] comboTimers;
    Text scoreText, highScoreText;
    RectTransform timerFront;

    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("GameController").Length > 1)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);

            comboTimers = new float[scoreBonusses.Length];
        }
    }

    void Update()
    {
        if (scoreText == null && GameObject.Find("score") != null)
        {
            scoreText = GameObject.Find("score").GetComponent<Text>();
            scoreText.text = "Score: " + string.Format("{0:0,#}", score).Replace(",", " ");
        }
        if (timerFront == null && GameObject.Find("timerFront") != null)
        {
            timerFront = GameObject.Find("timerFront").GetComponent<RectTransform>();
            if (lastKill > -1)
                comboTimers[lastKill] = 0;
        }
        if (highScoreText == null && GameObject.Find("highScoreText") != null)
        {
            highScoreText = GameObject.Find("highScoreText").GetComponent<Text>();
            highScoreText.text = "High Score: " + string.Format("{0:0,#}", PlayerPrefs.GetInt("highScore", 0)).Replace(",", " ");

            scoreText.text = string.Format("{0:0,#}", score).Replace(",", " ");

            if (score > PlayerPrefs.GetInt("highScore", 0))
            {
                PlayerPrefs.SetInt("highScore", score);

                if (GameObject.Find("LoseMessage") != null)
                    GameObject.Find("LoseMessage").GetComponent<Text>().text = "Hey! You got\n\npoints, you beat that sucker!";
                else if (GameObject.Find("WinMessage") != null)
                    GameObject.Find("WinMessage").GetComponent<Text>().text = "You won with\n\npoints and beat the high score!";
            }

            score = 0;
        }

        if (lastKill != -1 && comboTimers[lastKill] > 0)
        {
            comboTimers[lastKill] -= Time.deltaTime;
            if (timerFront != null)
                timerFront.sizeDelta = new Vector2(260 * comboTimers[lastKill] / comboTime, 20);

            if (comboTimers[lastKill] <= 0)
            {
                scoreBonus = 0;
                lastKill = 1;
            }
        }
    }

    public void ChangeScore(int type)
    {
        if (type < 2)
        {
            if (lastKill == type && comboTimers[type] > 0)
                scoreBonus += scoreBonusses[type];
            else
                scoreBonus = 0;

            score += scoreRewards[type] + scoreBonus;
            comboTimers[type] = comboTime;
            lastKill = type;
        }
        else if (type >= 2)
            score += scoreRewards[type];

        if (scoreText != null)
            scoreText.text = "Score: " + string.Format("{0:0,#}", score).Replace(",", " ");
    }
}
