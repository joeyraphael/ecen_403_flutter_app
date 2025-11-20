using UnityEngine;
using System.Collections;

public class InputLatencyTest : MonoBehaviour
{
    private float pressTime;
    public GameObject feedbackObject; // Assign a simple UI element (like a colored image)

    void Update()
    {
        // Detect a left click or touch
        if (Input.GetMouseButtonDown(0))
        {
            pressTime = Time.realtimeSinceStartup;
            StartCoroutine(Feedback());
        }
    }

    IEnumerator Feedback()
    {
        // Visually respond by flashing an object or changing its color
        feedbackObject.SetActive(true);
        yield return null; // Wait 1 frame
        float delta = (Time.realtimeSinceStartup - pressTime) * 1000f;
        Debug.Log($"[VALIDATION] Input Delay: {delta:F1} ms");
        feedbackObject.SetActive(false);
    }
}
