using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Around_Intellector : MonoBehaviour
{

    public bool answer;

    [SerializeField] GameObject yesButton;
    [SerializeField] GameObject noButton;

    // Start is called before the first frame update
    void Start()
    {
        answer = true;
    }

    // Update is called once per frame
    void Awake()
    {
        GetAnswer(() =>
        {
            answer = true;
            Debug.Log("Да");
        }, () =>
        {
            answer = false;
            Debug.Log("Нет");
        });
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
