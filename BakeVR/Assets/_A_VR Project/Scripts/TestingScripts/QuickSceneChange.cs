using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSceneChange : MonoBehaviour
{
    [SerializeField] private string loadScene;

    private void OnTriggerEnter(Collider other)
    {
        //if(other.CompareTag(""))
        SceneManager.LoadScene(loadScene);
    }
}
