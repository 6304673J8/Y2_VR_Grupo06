using UnityEngine;

public class GadgetFlashlight : MonoBehaviour
{
    [SerializeField] private GameObject spotLight;
    [SerializeField] private GameObject lightTriggerBody;
    [SerializeField] private AudioSource audioSrc;
    private bool b_FlashLightIsActive;

    // Start is called before the first frame update
    private void Awake()
    {
        //TurnOff();
        b_FlashLightIsActive = false;
        audioSrc = GetComponent<AudioSource>();
    }

    public void ActivateFlashlight()
    {
        Debug.Log("Ready To TurnOn");
        //audioSrc.Play();
        if (b_FlashLightIsActive == false)
        {
            Debug.Log("TurnedOn");
            TurnOn();
            b_FlashLightIsActive = true;
        }
        else if(b_FlashLightIsActive == true)
        {
            Debug.Log("TurnedOff");
            TurnOff();
            b_FlashLightIsActive = false;
        }
    }

    private void TurnOn()
    {
        spotLight.SetActive(true);
        lightTriggerBody.SetActive(true);
    }

    private void TurnOff()
    {
        spotLight.SetActive(false);
        lightTriggerBody.SetActive(false);
    }       
}