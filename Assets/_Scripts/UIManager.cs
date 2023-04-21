using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject QuestionTimer;  //5 sec Timer
    public GameObject AnswerTimer;    // 30 Sec Timer

    public MoneyPanel MoneyPanel;  // Shows the Money Track Of prize you Won

    public GameObject EndPanel;      // Show the amount you won 
    public Text WinningPrize;       //Text of Amount in EndPanel

    public GameObject LifelinePanel; //Lifeline button Panel

    public GameObject AudiancePanel;  //AudiancePoll Panel
    public Transform Barspanel;       //Bars in AudiancePoll

    public Text ExpertAnswer;         // Expert Answer

    public GameObject LifelineButton; // Life line Button

    public void SetWinningPrize(int i)
    {
        if (i == 0)
        {
            WinningPrize.text = "0";
        }
        else
        {
            WinningPrize.text = MoneyPanel.transform.GetChild(i - 1).GetComponentInChildren<Text>().text;
        }
    }

    public void Lifeline()
    {
        if (!LifelinePanel.activeSelf)
        {
            LifelinePanel.SetActive(true);
            GameManager.Instance.gamestate = GameState.Wait;
            GameManager.Instance.timer.StopTimer();
            GameManager.Instance.audioManager.Pause("Clock");
        }
        else
        {
            LifelinePanel.SetActive(false);
            GameManager.Instance.audioManager.UnPause("Clock");
            GameManager.Instance.timer.ResumeTimer();
        }
   
    }

    public void FiftyFifty(Transform Question)
    {
        int j = 0;
        Text[] wrongquestion = new Text[3];
        for (int i = 0; i < 4; i++)
        {
            if (Question.GetChild(i).GetComponentInChildren<Text>().text != GameManager.Instance.apicontroller.Answer)
            {
                wrongquestion[j] = Question.GetChild(i).GetComponentInChildren<Text>();
                j++;
            }
        }
        List<int> ints=new List<int>();
        while (ints.Count < 2)
        {
            int ran = Random.Range(0, 3);
            if (!ints.Contains(ran))
            {
                ints.Add(ran);
                wrongquestion[ran].text = string.Empty;
            }
        }
        GameManager.Instance.gamestate = GameState.PlayGame;
    }

    public void ChangeQuestion()
    {
        GameManager.Instance.apicontroller.loadquestion();
        GameManager.Instance.gamestate = GameState.PlayGame;
    }

    public void AudiancePoll (Transform Question)
    {
        AudiancePanel.gameObject.SetActive(true);
        int ran=Random.Range(0, 4);
        if (ran <= 2)
        {
            int answerindex = 0;

            //Show right Answer
            for (int i = 0; i < 4; i++)
            {
                if (Question.GetChild(i).GetComponentInChildren<Text>().text == GameManager.Instance.apicontroller.Answer)
                {
                    answerindex = Question.GetChild(i).GetSiblingIndex();
                }
            }
            var randPercentage = GivePercentage();
            randPercentage.Sort();
            randPercentage.Reverse();
            Barspanel.GetChild(answerindex).GetComponent<Image>().fillAmount = randPercentage[0];
            randPercentage.Remove(0);
            for (int i = 0; i < 4; i++)
            {
                if (i != answerindex)
                {
                    Barspanel.GetChild(i).GetComponent<Image>().fillAmount = randPercentage[i];
                }

            }

        }
        else
        {
            //Show wrong Answer
            for (int i = 0; i < 4; i++)
            {
                var randPercentage = GivePercentage();
                Barspanel.GetChild(i).GetComponent<Image>().fillAmount = randPercentage[i];              
            }
        }
        GameManager.Instance.gamestate = GameState.PlayGame;
    }

    List<float> GivePercentage()
    {
        List<float> nums = new List<float>();

        // Generate four random numbers between 0 and 1
        for (int i = 0; i < 4; i++)
        {
            nums.Add(Random.Range(0f, 1f));
        }

        // Scale the numbers so they add up to 1
        float total = 0;
        for (int i = 0; i < nums.Count; i++)
        {
            total += nums[i];
        }
        for (int i = 0; i < nums.Count; i++)
        {
            nums[i] /= total;
        }

        return nums;
    }

    public void AskAnExpert(Transform Question)
    {
        ExpertAnswer.gameObject.SetActive(true);
        int answerindex = 0;
        for (int i = 0; i < 4; i++)
        {
            if (Question.GetChild(i).GetComponentInChildren<Text>().text == GameManager.Instance.apicontroller.Answer)
            {
                answerindex = Question.GetChild(i).GetSiblingIndex();
            }
        }
        if (answerindex == 0)
        {
            ExpertAnswer.text = "Expert Says it A Option";
            Invoke("MakeDisableExpertAnswer", 3f);
        }
        else if (answerindex == 1)
        {
            ExpertAnswer.text = "Expert Says it B Option";
            Invoke("MakeDisableExpertAnswer", 3f);
        }
        else if (answerindex == 2)
        {
            ExpertAnswer.text = "Expert Says it C Option";
            Invoke("MakeDisableExpertAnswer", 3f);
        }
        else if (answerindex == 3)
        {
            ExpertAnswer.text = "Expert Says it D Option";
            Invoke("MakeDisableExpertAnswer", 3f);
        }
        GameManager.Instance.gamestate = GameState.PlayGame;
    }

    void MakeDisableExpertAnswer()
    {
        ExpertAnswer.gameObject.SetActive(false);
        GameManager.Instance.gamestate = GameState.PlayGame;
    }

    public void AllLifeLineOver(Transform LifeLinePanel)
    {     
        if (LifeLinePanel.childCount<=0)
        {
            LifelineButton.gameObject.SetActive(false);
        }
        else
        {
            LifelineButton.gameObject.SetActive(true);
        }
       
    }

    public void Destroybutton(GameObject button)
    {
        Destroy(button);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        StopAllCoroutines();
    }
}
