using System.Collections;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class ObjectiveTextTrigger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private string message = "";
    [SerializeField] private float displayTime = 3f;

    void Start()
    {
        if (objectiveText != null)
        {
            objectiveText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DisplayObjectiveText();
        }
    }

    private void DisplayObjectiveText()
    {
        if (objectiveText != null)
        {
            objectiveText.text = message;
            objectiveText.gameObject.SetActive(true);
            StartCoroutine(HideTextAfterTime());
        }
    }

    private IEnumerator HideTextAfterTime()
    {
        yield return new WaitForSeconds(displayTime);
        objectiveText.gameObject.SetActive(false);
    }
}
