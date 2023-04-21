[System.Serializable]
public class QuestionData
{
    public string category;
    public string type;
    public string difficulty;
    public string question;
    public string[] incorrect_answers;
    public string correct_answer;
}

public class QuestionDataWrapper
{
    public QuestionData[] results;
}