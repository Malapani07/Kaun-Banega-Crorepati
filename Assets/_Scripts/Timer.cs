using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public bool QuestionTimer; // if its Starting timer or Answer timer 

    public Text time;
    public float MaxSec;
    public float timeRemaining;
    public bool isTimerRunning;

    private IEnumerator RunTimer()
    {
        while (timeRemaining > 0)
        {
            if (isTimerRunning)
            {
                yield return new WaitForSeconds(1f);
                timeRemaining--;
                time.text = ((int)timeRemaining).ToString();
            }
            else
            {
                yield return null;
            }        
        }

        if (timeRemaining <= 0)
        {
            if (!QuestionTimer)
            {
                this.gameObject.SetActive(false);
                GameManager.Instance.audioManager.Play("Buzzer");
                GameManager.Instance.UiManager.EndPanel.SetActive(true);
                GameManager.Instance.UiManager.SetWinningPrize(GameManager.Instance.NoOfAnsweredQuestion);
            }
            else if (QuestionTimer)
            {
                GameManager.Instance.StartLoadingQuestion();
                this.gameObject.SetActive(false);
            }
        }
    }

    public void StartTimers()
    {
        if (!isTimerRunning)
        {
            isTimerRunning = true;
            StartCoroutine(RunTimer());
        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }
    public void ResumeTimer()
    {
        isTimerRunning = true;
    }

    private void OnEnable()
    {
        timeRemaining = MaxSec;
        time.text = ((int)MaxSec).ToString();
        StartTimers();
    }

    private void OnDisable()
    {
        isTimerRunning = false;
        StopCoroutine(RunTimer());
    }
}
