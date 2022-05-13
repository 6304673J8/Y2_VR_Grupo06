using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class OnBoardingManager : MonoBehaviour
{
	public InputComprobationManager inputComprobationManager;
	//public TutorialInteractable tutorialInteractable;
	public TutorialInteractable tutorialInteractable;


	[Header("Hands")]
	public Transform leftHandAttachPoint;
	public Transform rightHandAttachPoint;

	/*[Header("Audio")]
	public AudioSource audioSource;
	public AudioClip[] audioClips;
	public GameObject oneshotAudioPrefab;
	public AudioClip[] feedbackClips;*/

	[Header("World UI")]
	public GameObject playerEye;
	public GameObject tutorialUIPrefab;
	private List<TutorialUI> currentUIs = new List<TutorialUI>();

	[Header("Stage 1")]
	private TutorialUI actionObjectUI;
	public XRBaseInteractable explosiveCube;
	//public GameObject cubePrefab;
	//private List<GameObject> spawnedCubes = new List<GameObject>();
	//public Vector3 cubeSpawnPos;
	public GameObject explodingTarget;
	//public GameObject buttonsHolder;
	//public GameObject nextTutorialButton;
	//public bool proceedButton { get; set; } = false;

	[Header("Stage 2")]
	public GameObject keyGameObject;
	public GameObject tutorialSocketBase;

	public GameObject socketUIAttachPoint;
	public GameObject keyAttachPoint;

	private TutorialUI socketTutorialInitialUI;
	private TutorialUI keyTutorialUI;

	public bool actionedSocket { get; set; } = false;
	public bool returnedSocket { get; set; } = false;
	public bool actionedLever { get; set; } = false;

	private void Awake()
	{
		inputComprobationManager = GetComponent<InputComprobationManager>();
	}

	void Start()
	{
		//thumbStick.action.Enable(); 
		//	triggerActivate.action.Enable();
		//teleportModeActivate.action.performed += OnTeleportActivate;

		StartCoroutine("GameIntroduction");
	}

	IEnumerator GameIntroduction()
	{
		yield return new WaitForSeconds(1f);

		// INTRO
		//yield return Tutorial_0();

		// BASIC INPUT
		//- Finger movement -
		//yield return Tutorial_1_0();
		//- Hands movement
		//yield return Tutorial_1_1();
		//- Hands Interaction
		//yield return Tutorial_1_2();

		// BASIC INTERACTION
		//- Grab and Throw Objects -
		yield return Tutorial_2_0();
		//yield return Tutorial_2_1();

		// COMPLEX INTERACTION
		//- Trigger While Holding Objects -
		//yield return Tutorial_3_0();

		// SOCKETS INTERACTION
		//- Socket Basics -
		yield return Tutorial_4_0();
		//- Socket Manager / Lever -
		//yield return Tutorial_4_1();

		// TELEPORTATION INTERACTION
		//- Basic Teleport -
		//yield return Tutorial_5_0();
	}

	IEnumerator Tutorial_2_0()
	{
		//audioSource.Stop();
		//audioSource.clip = audioClips[4];
		//audioSource.Play();
		bool grabbed = false;
		bool released = false;
		//bool thrown = false;
		//bool exploded = false;
		yield return new WaitForSeconds(.2f);
		keyGameObject.gameObject.SetActive(true);
		tutorialInteractable.m_Held = false;
		
		var cubeUI = Instantiate(tutorialUIPrefab,
			explosiveCube.transform.position + new Vector3(-0.2f, 0.2f, 0f),
			Quaternion.identity).GetComponent<TutorialUI>();

		cubeUI.StartUI(playerEye, explosiveCube.gameObject,
			new Vector3(-0.2f, 0.2f, 0f), TutorialUI.AttachToH.LEFT_H);

		cubeUI.text.text = "Usa el gatillo del dedo medio para agarrar y soltar objetos.";

		//tutorialInteractable Is The Explosive Cube GameObject
		//While The Required GameObject Is Not -Grabbed- And -Released- UIs Won't Dissappear
		while (!grabbed || !released)
		{
			if (tutorialInteractable.m_Held == true && !grabbed)
			{
				//Instantiate(oneshotAudioPrefab, leftHandAttachPoint.transform.position, Quaternion.identity).GetComponent<OneShotAudio>().InstantiateAudio(feedbackClips[0], true);
				grabbed = true;
				Debug.Log("I GRABBED");
			}
			if (tutorialInteractable.m_Thrown == true && !released)
			{
				//Instantiate(oneshotAudioPrefab, rightHandAttachPoint.transform.position, Quaternion.identity).GetComponent<OneShotAudio>().InstantiateAudio(feedbackClips[0], true);
				released = true;
				Debug.Log("I LET GO");
			}
			yield return new WaitForEndOfFrame();
		}
		cubeUI.ToggleThumbsUp(true);
		//Instantiate(oneshotAudioPrefab, cube.transform.position,
		//Quaternion.identity).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[0], true);
		yield return new WaitForSeconds(1f);

		/*audioSource.Stop();
		audioSource.clip = audioClips[4];
		audioSource.Play();*/
		//yield return new WaitForSeconds(1f);

		//cubeUI.text.text = "Lanza el cubo al objetivo.";
		//cubeUI.ToggleThumbsUp(false);
		//Rigidbody rb = explosiveCube.GetComponent<Rigidbody>();
		//rb.velocity = Vector3.zero;
		//rb.position = cubeSpawnPos;
		//cubeUI.transform.position = explosiveCube.transform.position + new Vector3(-0.2f, 0.2f, 0f);
		//explodingTarget.SetActive(true);
		//Move the cube to the starting pos
		//yield return new WaitUntil(() => tutorialInteractable.m_Thrown || !explodingTarget.activeInHierarchy);
		//yield return new WaitForSeconds(2f);

		Destroy(cubeUI.gameObject);
	}

	IEnumerator Tutorial_4_0()
	{
		//audioSource.Stop();
		//audioSource.clip = audioClips[6];
		//audioSource.Play();

		yield return new WaitForSeconds(.5f);

		//Socket Learning Tutorial
		socketTutorialInitialUI = Instantiate(tutorialUIPrefab,
			socketUIAttachPoint.transform.position + new Vector3(-0.2f, 0.2f, 0f),
			Quaternion.identity).GetComponent<TutorialUI>();

		socketTutorialInitialUI.StartUI(playerEye, socketUIAttachPoint.gameObject,
			new Vector3(-0.2f, 0.2f, 0f), TutorialUI.AttachToH.LEFT_H);

		socketTutorialInitialUI.text.text = "Acerca el identificador de la mesa aquí.";

		tutorialSocketBase.gameObject.SetActive(true);
		while (!actionedSocket && !returnedSocket)
		{
			yield return new WaitForEndOfFrame();
		}
		socketTutorialInitialUI.ToggleThumbsUp(true);
		//Instantiate(oneshotAudioPrefab, null).GetComponent<OneShotAudio>().InstantiateAudio(feedbackClips[1], false);
		yield return new WaitForSeconds(1f);
		Destroy(socketTutorialInitialUI.gameObject);
	}

	IEnumerator Tutorial_4_1()
	{
		/*audioSource.Stop();
		audioSource.clip = audioClips[8];
		audioSource.Play();*/
		yield return new WaitForSeconds(.5f);

		keyTutorialUI = Instantiate(tutorialUIPrefab,
			keyAttachPoint.transform.position + new Vector3(-0.2f, 0.2f, 0f),
			Quaternion.identity).GetComponent<TutorialUI>();

		keyTutorialUI.StartUI(playerEye, keyAttachPoint.gameObject,
			new Vector3(-0.2f, 0.2f, 0f), TutorialUI.AttachToH.LEFT_H);

		keyTutorialUI.text.text = "Repara la palanca acercando el objeto faltante y accionala.";
		keyGameObject.gameObject.SetActive(true);
		actionedLever = false;
		while (!actionedLever)
		{
			yield return new WaitForEndOfFrame();
		}

		keyTutorialUI.ToggleThumbsUp(true);

		yield return new WaitForSeconds(1f);
		Destroy(keyTutorialUI.gameObject);
		tutorialSocketBase.gameObject.SetActive(false);
	}

	
}