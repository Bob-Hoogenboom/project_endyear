using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;
    [SerializeField] private GameObject interactFeedback;

    private void Start()
    {
        interactFeedback.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            interactFeedback.SetActive(false);
            return;
        }

        interactFeedback.SetActive(true);

        Debug.Log("active");
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            onInteract.Invoke();
        }
    }
}
