using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public void CheckAnswer()
    {
        if (GameManager.Instance.gamestate==GameState.PlayGame)
        {
            GameManager.Instance.gamestate = GameState.Wait;
            ColouringButton(GameManager.Instance.apicontroller.transform);
            if (GetComponentInChildren<Text>().text == GameManager.Instance.apicontroller.Answer)
            {
                GameManager.Instance.UiManager.AnswerTimer.SetActive(false);  // SetAnswerTimer To false.
                GameManager.Instance.UiManager.LifelineButton.SetActive(false);

                GameManager.Instance.audioManager.Play("CorrectAnswer");     //  handles music
                GameManager.Instance.audioManager.Play("Clapping");
                GameManager.Instance.audioManager.Stop("Clock");

                StartCoroutine(ShowMoneyPanel());             
            }
            else
            {
                GameManager.Instance.UiManager.AnswerTimer.SetActive(false); // SetAnswerTimer To false.
                GameManager.Instance.UiManager.LifelineButton.SetActive(false);
                GameManager.Instance.audioManager.Play("WrongAnswer");   // handles music
                GameManager.Instance.audioManager.Stop("Clock");

                StartCoroutine(ShowEndPanel());
            }
        }
    }

    void ColouringButton(Transform questionpanel)
    {
        for (int i = 0; i < 4; i++)
        {
            if (questionpanel.GetChild(i).GetComponentInChildren<Text>().text == GameManager.Instance.apicontroller.Answer)
            {
                questionpanel.GetChild(i).GetComponentInChildren<Image>().color = Color.green;
            }
            else
            {
                questionpanel.GetChild(i).GetComponentInChildren<Image>().color = Color.red;
            }
        }
    }

    IEnumerator ShowMoneyPanel()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.UiManager.MoneyPanel.gameObject.SetActive(true);  // MoneyPanel to On

        var i = GameManager.Instance.NoOfAnsweredQuestion++;            // Start the animation of Money Panel
        GameManager.Instance.UiManager.MoneyPanel.StartAnimation(i);
        if (i >= 14)
        {
            yield return new WaitWhile(()=>GameManager.Instance.UiManager.MoneyPanel.gameObject.activeSelf);
            StartCoroutine(ShowEndPanel());
        }
    }

    IEnumerator ShowEndPanel()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.UiManager.EndPanel.SetActive(true);    // Shows endgame Panel 
        GameManager.Instance.UiManager.SetWinningPrize(GameManager.Instance.NoOfAnsweredQuestion);
    }

}
