using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Around_Intellector : MonoBehaviour
{
    public bool? answer;

    [SerializeField] GameObject yesButton;
    [SerializeField] GameObject noButton;

    void Start()
    {
        answer = null;
    }

    void Awake()
    {
        GetAnswer(
            yesAction: () => { answer = true; },
            noAction: () => { answer = false; }
        );
    }

    public void GetAnswer(UnityAction yesAction, UnityAction noAction)
    {
        yesButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            yesAction();
        });
        noButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            noAction();
        });
    }
}
