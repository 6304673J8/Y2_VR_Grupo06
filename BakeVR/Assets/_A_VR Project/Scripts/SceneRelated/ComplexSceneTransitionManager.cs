using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComplexSceneTransitionManager : MonoBehaviour
{
    public FadeScreen fadeScreen;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void GoToSceneAsync(int sceneIndex)
    {
        // Calling related Couroutine
        StartCoroutine(GoToSceneAsyncRoutine(sceneIndex));
    }

    IEnumerator GoToSceneAsyncRoutine(int sceneIndex)
    {
        fadeScreen.FadeOut();

        //Launch the new scene Asynchronously
        //Operation value will let us make a progress bar
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        //Resets the Timer to 0

        float timer = 0;

        // Until Timer is bigger than the Fade Duration and Load Scene Operation
        while (timer <= fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        // Finally The Scene Activation is set to True
        operation.allowSceneActivation = true;
    }
}
