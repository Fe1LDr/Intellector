using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Progressor_end : MonoBehaviour
{
    public PieceType? answer;

    [SerializeField] GameObject DominatorButton;
    [SerializeField] GameObject AgressorButton;
    [SerializeField] GameObject LiberatorButton;
    [SerializeField] GameObject DefensorButton;

    void Start()
    {
        answer = null;
    }

    void Awake()
    {
        GetAnswer(
            () => { answer = PieceType.dominator; },
            () => { answer = PieceType.agressor; },
            () => { answer = PieceType.liberator; },
            () => { answer = PieceType.defensor; }
            );
    }

    public void GetAnswer(UnityAction DominatorAction, UnityAction AgressorAction, UnityAction LiberatorAction, UnityAction DefensorAction)
    {
        DominatorButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            DominatorAction();
        });
        AgressorButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            AgressorAction();
        });
        LiberatorButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            LiberatorAction();
        });
        DefensorButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            DefensorAction();
        });
    }
}
