using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2;
    public Color fadeColor;
    private Renderer render;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
        if (fadeOnStart)
        {
            FadeIn();
        }
    }
    
    public void FadeIn() {
        Fade(1, 0);
    }
    public void FadeOut() {
        Fade(0, 1);
    }
    public void Fade(float alphaIn, float alphaOut)
    {
        // Calling related Couroutine
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        // Resets the Timer to 0
        float timerFader = 0;

        // Once Timer is bigger than the Fade Duration return
        while(timerFader <= fadeDuration)
        {
            Color newColor = fadeColor;
            // The third parameter dictates the smooth interpolation between In / Out
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timerFader / fadeDuration);
            render.material.SetColor("_BaseColor", newColor);

            // Every frame of the game the Timer will increase
            timerFader += Time.deltaTime;
            yield return null;
        }
        // Making sure that the final value is alphaOut
        Color newColorFinal = fadeColor;
        newColorFinal.a = alphaOut;

        render.material.SetColor("_BaseColor", newColorFinal);
    }
}
