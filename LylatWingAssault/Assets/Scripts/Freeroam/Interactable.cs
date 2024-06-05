using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;
    [SerializeField] private GameObject interactFeedback;
    private bool _interactActive = false;

    private void Start()
    {
        interactFeedback.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _interactActive)
        {
            Debug.Log("Clicked");
            onInteract.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        interactFeedback.SetActive(true);
        _interactActive = true;
    }



    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        interactFeedback.SetActive(false);
        _interactActive = false;
    }
}
