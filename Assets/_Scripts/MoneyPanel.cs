using UnityEngine;

public class MoneyPanel : MonoBehaviour
{
    void OnEnable()
    {
        Invoke("MakeDisable", 5f);
    }

    public void MakeDisable()
    {
        gameObject.SetActive(false);
        if (GameManager.Instance.NoOfAnsweredQuestion <= 14)
        {
            GameManager.Instance.StartLoadingQuestion();
        }
    }

    public void StartAnimation(int index)
    {
        transform.GetChild(index).GetComponent<Animator>().SetTrigger("Levelup");
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}
