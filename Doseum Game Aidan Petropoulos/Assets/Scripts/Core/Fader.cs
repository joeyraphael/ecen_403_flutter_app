using System.Collections; // for IEnumerator stuff
using UnityEngine; // unity
using UnityEngine.UI; // for Image fade

public class Fader : MonoBehaviour
{
    Image image; // the UI image i fade in/out

    private void Awake()
    {
        image = GetComponent<Image>(); // grab the image on this object
    }

    public IEnumerator FadeIn(float time)
    {
        float t = 0f; // timer
        Color c = image.color; // grab color
        c.a = 0f; // start invisible
        image.color = c;

        // fade from 0 → 1
        while (t < time)
        {
            t += Time.deltaTime; // add time
            c.a = Mathf.Lerp(0f, 1f, t / time); // smooth fade
            image.color = c; // apply
            yield return null; // wait one frame
        }
    }

    public IEnumerator FadeOut(float time)
    {
        float t = 0f;
        Color c = image.color;
        c.a = 1f; // start fully visible
        image.color = c;

        // fade from 1 → 0
        while (t < time)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / time); // smooth fade down
            image.color = c;
            yield return null;
        }
    }
}
