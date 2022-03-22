using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialManager : MonoBehaviour
{
	public InputComprobationManager inputComprobationManager;
	//public TutorialInteractable tutorialInteractable;

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

	[Header("Tutorial 1")]
	public Color neutralColor;
	public Color yellowColor;
	//public MeshRenderer leftHandColliderMesh;
	//public MeshRenderer rightHandColliderMesh;
	public LayerMask handsLayer;

	[Header("Stage 2")]
	//public Autohand.Grabbable cube;
	public TutorialInteractable tutorialInteractable;
	public XRBaseInteractable explosiveCube;
	public GameObject cubePrefab;
	private List<GameObject> spawnedCubes = new List<GameObject>();
	public Vector3 cubeSpawnPos;
	public GameObject explodingTarget;
	//public GameObject buttonInstantiator;
	//public bool proceedButton { get; set; } = false;


	//[Header("Ready")]

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
		yield return Tutorial_0();

		// BASIC MOVEMENT
		//- Finger movement -
		yield return Tutorial_1_0();
		//- Hands movement
		//yield return Tutorial_1_1();
		// BASIC INTERACTION
		//- Grab and Throw Objects -
		yield return Tutorial_2_0();

	}

	IEnumerator Tutorial_0()
	{
		//audioSource.clip = audioClips[0];
		//audioSource.Play();
		//yield return new WaitUntil(() => !audioSource.isPlaying);
		yield return new WaitForSeconds(2f);
	}
	IEnumerator Tutorial_1_0()
	{
		//audioSource.clip = audioClips[1];
		//audioSource.Play();
		yield return new WaitForSeconds(0.5f);
		//Instantiate the UIs in the hands
		//LeftHand
		var leftUI = Instantiate(tutorialUIPrefab,
					leftHandAttachPoint.transform.position + new Vector3(-0.2f, 0.1f, 0f),
					Quaternion.identity).GetComponent<TutorialUI>();

		leftUI.StartUI(playerEye, leftHandAttachPoint.gameObject,
					new Vector3(-0.2f, 0.1f, 0f), TutorialUI.AttachToH.LEFT_H);

		leftUI.text.text = "Pulsa los gatillos para mover los dedos de las manos.";
		currentUIs.Add(leftUI);

		//RightHand
		var rightUI = Instantiate(tutorialUIPrefab,
			rightHandAttachPoint.transform.position + new Vector3(0.2f, 0.1f, 0f),
			Quaternion.identity).GetComponent<TutorialUI>();

		rightUI.StartUI(playerEye, rightHandAttachPoint.gameObject, new Vector3(0.2f, 0.1f, 0f), TutorialUI.AttachToH.RIGHT_H);
		rightUI.text.text = "Pulsa los gatillos para mover los dedos de las manos.";
		currentUIs.Add(rightUI);

		bool validLeftH = false;
		bool validRightH = false;
		while (!validLeftH || !validRightH)
		{
			// Grip Comprobation :         if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
			if (inputComprobationManager.b_lTriggerIsActive && !validLeftH)
			{
				//Instantiate(oneshotAudioPrefab, leftHandLink.transform.position, Quaternion.identity).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[0], true);
				validLeftH = true;
				currentUIs[0].ToggleThumbsUp(true);
			}
			if (inputComprobationManager.b_rTriggerIsActive && !validRightH)
			{
				//Instantiate(oneshotAudioPrefab, rightHandLink.transform.position, Quaternion.identity).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[0], true);
				validRightH = true;
				currentUIs[1].ToggleThumbsUp(true);
			}
			yield return new WaitForEndOfFrame();
		}

		//audioSource.Stop();
		//Instantiate(oneshotAudioPrefab, null).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[1], false);
		yield return new WaitForSeconds(.4f);
		foreach (var ui in currentUIs)
		{
			Destroy(ui.gameObject);
		}
		currentUIs.Clear();
	}

	/*IEnumerator Tutorial_1_1()
	{
		//audioSource.clip = audioClips[2];
		//audioSource.Play();
		yield return new WaitForSeconds(0.5f);
		//Instantiate the UIs in the hands
		//LeftHand
		var leftUI = Instantiate(tutorialUIPrefab,
					leftHandAttachPoint.transform.position + new Vector3(-0.2f, 0.1f, 0f),
					Quaternion.identity).GetComponent<TutorialUI>();

		leftUI.StartUI(playerEye, leftHandAttachPoint.gameObject,
					new Vector3(-0.2f, 0.1f, 0f), TutorialUI.AttachToH.LEFT_H);

		leftUI.text.text = "Pulsa los gatillos para mover los dedos de las manos.";
		currentUIs.Add(leftUI);

		//RightHand
		var rightUI = Instantiate(tutorialUIPrefab,
			rightHandAttachPoint.transform.position + new Vector3(0.2f, 0.1f, 0f),
			Quaternion.identity).GetComponent<TutorialUI>();

		rightUI.StartUI(playerEye, rightHandAttachPoint.gameObject, new Vector3(0.2f, 0.1f, 0f), TutorialUI.AttachToH.RIGHT_H);
		rightUI.text.text = "Pulsa los gatillos para mover los dedos de las manos.";
		currentUIs.Add(rightUI);

		bool validLeftH = false;
		bool validRightH = false;
		while (!validLeftH || !validRightH)
		{
			// Grip Comprobation :         if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
			if (inputComprobationManager.b_lTriggerIsActive && !validLeftH)
			{
				//Instantiate(oneshotAudioPrefab, leftHandLink.transform.position, Quaternion.identity).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[0], true);
				validLeftH = true;
				Debug.Log("I PRESS LEFT");
				currentUIs[0].ToggleThumbsUp(true);
			}
			if (inputComprobationManager.b_rTriggerIsActive && !validRightH)
			{
				//Instantiate(oneshotAudioPrefab, rightHandLink.transform.position, Quaternion.identity).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[0], true);
				validRightH = true;
				Debug.Log("I PRESS RIGHT");
				currentUIs[1].ToggleThumbsUp(true);
			}
			yield return new WaitForEndOfFrame();
		}

		//audioSource.Stop();
		//Instantiate(oneshotAudioPrefab, null).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[1], false);
		yield return new WaitForSeconds(.4f);
		foreach (var ui in currentUIs)
		{
			Destroy(ui.gameObject);
		}
		currentUIs.Clear();
	}*/

	IEnumerator Tutorial_2_0()
	{
		//audioSource.Stop();
		//audioSource.clip = audioClips[3];
		//audioSource.Play();
		bool grabbed = false;
		bool released = false;
		//bool thrown = false;
		//bool exploded = false;
		yield return new WaitForSeconds(.2f);
		explosiveCube.gameObject.SetActive(true);
		tutorialInteractable.m_Held = false;
		spawnedCubes.Add(explosiveCube.gameObject);

		var cubeUI = Instantiate(tutorialUIPrefab,
			explosiveCube.transform.position + new Vector3(-0.2f, 0.2f, 0f),
			Quaternion.identity).GetComponent<TutorialUI>();

		cubeUI.StartUI(playerEye, explosiveCube.gameObject,
			new Vector3(-0.2f, 0.2f, 0f), TutorialUI.AttachToH.LEFT_H);

		cubeUI.text.text = "Usa el gatillo del dedo índice para agarrar y soltar objetos.";

		while (!grabbed || !released)
		{
			// Grip Comprobation :         if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
			if (tutorialInteractable.m_Held == true && !grabbed)
			{
				//Instantiate(oneshotAudioPrefab, leftHandLink.transform.position, Quaternion.identity).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[0], true);
				grabbed = true;
				Debug.Log("I GRABBED");
			}
			if (tutorialInteractable.m_Thrown == true && !released)
			{
				//Instantiate(oneshotAudioPrefab, rightHandLink.transform.position, Quaternion.identity).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[0], true);
				released = true;
				Debug.Log("I LETTED GO");
			}

			yield return new WaitForEndOfFrame();
		}
		cubeUI.ToggleThumbsUp(true);
		//Instantiate(oneshotAudioPrefab, cube.transform.position,
		//Quaternion.identity).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[0], true);
		yield return new WaitForSeconds(1f);

		//audioSource.Stop();
		//audioSource.clip = audioClips[4];
		//audioSource.Play();
		yield return new WaitForSeconds(1f);

		cubeUI.text.text = "Lanza el cubo a la diana.";
		cubeUI.ToggleThumbsUp(false);
		Rigidbody rb = explosiveCube.GetComponent<Rigidbody>();
		rb.velocity = Vector3.zero;
		rb.position = cubeSpawnPos;
		cubeUI.transform.position = explosiveCube.transform.position + new Vector3(-0.2f, 0.2f, 0f);
		explodingTarget.SetActive(true);
		//Move the cube to the starting pos
		yield return new WaitUntil(() => tutorialInteractable.m_Thrown || !explodingTarget.activeInHierarchy);
		yield return new WaitForSeconds(2f);

		Destroy(cubeUI.gameObject);

	}
}
