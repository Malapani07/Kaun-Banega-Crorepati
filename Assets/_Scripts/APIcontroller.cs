using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class APIcontroller : MonoBehaviour
{
    public Text Question;
    public Button[] Options;
    public List<string> incorrectList;
    public List<int> ints;
    public string Answer;
    private readonly string[] API_Url = 
     {
        "https://opentdb.com/api.php?amount=1&category=9&difficulty=easy&type=multiple", //easy GK
        "https://opentdb.com/api.php?amount=1&category=10&difficulty=easy&type=multiple", //book
        "https://opentdb.com/api.php?amount=1&category=17&difficulty=easy&type=multiple",//sci nature
        "https://opentdb.com/api.php?amount=1&category=18&difficulty=easy&type=multiple",//computer
        "https://opentdb.com/api.php?amount=1&category=19&difficulty=easy&type=multiple",//maths
        "https://opentdb.com/api.php?amount=1&category=22&difficulty=easy&type=multiple",//geography
        "https://opentdb.com/api.php?amount=1&category=27&difficulty=easy&type=multiple",//animals
        "https://opentdb.com/api.php?amount=1&category=11&difficulty=easy&type=multiple",//flim


        "https://opentdb.com/api.php?amount=1&category=9&difficulty=medium&type=multiple", //medium GK
        "https://opentdb.com/api.php?amount=1&category=10&difficulty=medium&type=multiple", //book
        "https://opentdb.com/api.php?amount=1&category=17&difficulty=medium&type=multiple",//sci nature
        "https://opentdb.com/api.php?amount=1&category=18&difficulty=medium&type=multiple",//computer
        "https://opentdb.com/api.php?amount=1&category=19&difficulty=medium&type=multiple",//maths
        "https://opentdb.com/api.php?amount=1&category=22&difficulty=medium&type=multiple",//geography
        "https://opentdb.com/api.php?amount=1&category=27&difficulty=medium&type=multiple",//animals
        "https://opentdb.com/api.php?amount=1&category=23&difficulty=medium&type=multiple",//history
        "https://opentdb.com/api.php?amount=1&category=15&difficulty=medium&type=multiple",//vedio Game
        "https://opentdb.com/api.php?amount=1&category=20&difficulty=medium&type=multiple", //mythology

        "https://opentdb.com/api.php?amount=1&category=9&difficulty=hard&type=multiple", //Hard GK
        "https://opentdb.com/api.php?amount=1&category=10&difficulty=hard&type=multiple", //book
        "https://opentdb.com/api.php?amount=1&category=17&difficulty=hard&type=multiple",//sci nature
        "https://opentdb.com/api.php?amount=1&category=18&difficulty=hard&type=multiple",//computer
        "https://opentdb.com/api.php?amount=1&category=19&difficulty=hard&type=multiple",//maths
        "https://opentdb.com/api.php?amount=1&category=22&difficulty=hard&type=multiple",//geography
        "https://opentdb.com/api.php?amount=1&category=27&difficulty=hard&type=multiple",//animals
        "https://opentdb.com/api.php?amount=1&category=23&difficulty=hard&type=multiple",//history
        "https://opentdb.com/api.php?amount=1&category=15&difficulty=hard&type=multiple",//vedio Game
        "https://opentdb.com/api.php?amount=1&category=20&difficulty=hard&type=multiple" //mythology
    };

  
    public void loadquestion()
    {
        for (int i = 0; i < Options.Length; i++)
        {
            Options[i].GetComponent<Image>().color = Color.white;
            Options[i].GetComponentInChildren<Text>().text = string.Empty;
        }
        incorrectList.Clear();
        StartCoroutine(LoadQuestion());
    }

    IEnumerator LoadQuestion()
    {

        NetworkReachability reachability = Application.internetReachability;
        while (reachability != NetworkReachability.ReachableViaCarrierDataNetwork && reachability != NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Question.text = "Error:- Check If You are not connected to internet.....";
            yield return new WaitForSeconds(1f); // Wait for one second before checking again
            reachability = Application.internetReachability; // Check internet connectivity again
        }

        UnityWebRequest Qrequest = UnityWebRequest.Get(API_Url[GetQuestionindex()]);
        Question.text = "Loading....";

        yield return Qrequest.SendWebRequest();


        if (Qrequest.result != UnityWebRequest.Result.Success)
        {
            Question.text = "Error:- Check If You are not connected to internet. Close and Start the Game Again.";
        }
        else
        {
            GameManager.Instance.gamestate = GameState.PlayGame;
            GameManager.Instance.UiManager.AllLifeLineOver(GameManager.Instance.UiManager.LifelinePanel.transform);
            if (GameManager.Instance.NoOfAnsweredQuestion < 7)
            {
                GameManager.Instance.UiManager.AnswerTimer.SetActive(true); //Start the AnswerTimer.         
                GameManager.Instance.audioManager.Play("Clock");            //Play the Clock
            }          
            QuestionDataWrapper questionDataWrapper = JsonUtility.FromJson<QuestionDataWrapper>(Qrequest.downloadHandler.text);
            QuestionData questionData = questionDataWrapper.results[0];
            Question.text = System.Net.WebUtility.HtmlDecode(questionData.question);

            for (int i = 0; i < questionData.incorrect_answers.Length; i++)
            {
               incorrectList.Add(questionData.incorrect_answers[i]);
            }
            Answer = System.Net.WebUtility.HtmlDecode(questionData.correct_answer);
            SetOption(incorrectList,Options,questionData.correct_answer);
        }
    }

    int counter = 0;
    void SetOption(List<string> incorrectOption, Button[] option, string Answer)
    {
        while (ints.Count < 3)
        {            
            var ran = Random.Range(0,4);
            if (!ints.Contains(ran))
            {
                ints.Add(ran);
                option[ran].GetComponentInChildren<Text>().text = System.Net.WebUtility.HtmlDecode(incorrectOption[counter]);
                counter++;
            }
        }
        counter = 0;
        ints.Clear();
        for (int i = 0; i < option.Length; i++)
        {
            if (option[i].GetComponentInChildren<Text>().text == string.Empty)
            {
                option[i].GetComponentInChildren<Text>().text = System.Net.WebUtility.HtmlDecode(Answer);
            }
        }
    }

    int GetQuestionindex()
    {
        if (GameManager.Instance.NoOfAnsweredQuestion < 8)
        {
            return Random.Range(0, 8);
        }
        else if (GameManager.Instance.NoOfAnsweredQuestion < 18)
        {
            return Random.Range(8, 18);
        }
        else if (GameManager.Instance.NoOfAnsweredQuestion < 27)
        {
            return Random.Range(18, 27);
        }
      
        return Random.Range(0, 27);    
    }

}
