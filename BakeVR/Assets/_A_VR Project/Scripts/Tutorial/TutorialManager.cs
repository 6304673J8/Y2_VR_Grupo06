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
	public TutorialCubesManager tutorialCubesManager;
	public TutorialCubesManager tutorialButtonManager;
	public GameObject tutorialTable;
	public GameObject tutorialButtonTable;
	public GameObject UIAttachPoint;
	public GameObject UIButtonAttachPoint;
	//	public Color neutralColor;
	//	public Color yellowColor;
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
	public GameObject buttonsHolder;
	public GameObject nextTutorialButton;
	public bool proceedButton { get; set; } = false;
	private TutorialUI actionObjectUI;

	[Header("Stage 3")]
	public GameObject currentActionObject;
	public GameObject actionObjectPedestal;
	public Vector3 actionObjectSpawnPos;
	public GameObject actionObjectPrefab;

	public bool actionedObject { get; set; } = false;
	private bool respawningActionObject = false;

	[Header("Stage 4")]
	public GameObject leverGameObject;
	public GameObject tutorialSocketBase;

	public GameObject socketUIAttachPoint;
	public GameObject leverAttachPoint;

	private TutorialUI socketTutorialInitialUI;
	private TutorialUI leverTutorialUI;

	public bool actionedSocket { get; set; } = false;
	public bool returnedSocket { get; set; } = false;
	public bool actionedLever { get; set; } = false;

	[Header("Stage 5")]
	public GameObject teleportGameObject;

	public bool actionedTeleport { get; set; } = false;


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

		// BASIC INPUT
		//- Finger movement -
		yield return Tutorial_1_0();
		//- Hands movement
		yield return Tutorial_1_1();
		//- Hands Interaction
		yield return Tutorial_1_2();
		
		// BASIC INTERACTION
		//- Grab and Throw Objects -
		yield return Tutorial_2_0();
		yield return Tutorial_2_1();

		// COMPLEX INTERACTION
		//- Trigger While Holding Objects -
		yield return Tutorial_3_0();
		
		// SOCKETS INTERACTION
		//- Socket Basics -
		yield return Tutorial_4_0();
		//- Socket Manager / Lever -
		yield return Tutorial_4_1();

		// TELEPORTATION INTERACTION
		//- Basic Teleport -
		yield return Tutorial_5_0();
	}

	IEnumerator Tutorial_0()
	{
		//audioSource.clip = audioClips[0];
		//audioSource.Play();
		//yield return new WaitUntil(() => !audioSource.isPlaying);
		yield return new WaitForSeconds(1f);
	}

	// BASIC INPUT FUNCTION - TRIGGER
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

		leftUI.text.text = "Prueba a pulsar el gatillo del dedo índice.";
		currentUIs.Add(leftUI);

		//RightHand
		var rightUI = Instantiate(tutorialUIPrefab,
			rightHandAttachPoint.transform.position + new Vector3(0.2f, 0.1f, 0f),
			Quaternion.identity).GetComponent<TutorialUI>();

		rightUI.StartUI(playerEye, rightHandAttachPoint.gameObject, new Vector3(0.2f, 0.1f, 0f), TutorialUI.AttachToH.RIGHT_H);
		rightUI.text.text = "Prueba a pulsar el gatillo del dedo índice.";
		currentUIs.Add(rightUI);

		//While The Required Button Is Not Pressed, UIs Won't Dissappear
		bool validLeftH = false;
		bool validRightH = false;
		yield return new WaitForSeconds(.4f);

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

	// BASIC INTERACTION - PHYSICS
	IEnumerator Tutorial_1_1()
	{
		//audioSource.clip = audioClips[2];
		//audioSource.Play();
		bool cleanedTable = false;
		yield return new WaitForSeconds(0.5f);
		
		//Instantiate The UI In The Proper Place
		tutorialTable.gameObject.SetActive(true);
		tutorialCubesManager.cleanedTable = false;

		var cubesUI = Instantiate(tutorialUIPrefab,
			UIAttachPoint.transform.position + new Vector3(-0.2f, 0.2f, 0f),
			Quaternion.identity).GetComponent<TutorialUI>();

		cubesUI.StartUI(playerEye, UIAttachPoint.gameObject,
			new Vector3(-0.2f, 0.2f, 0f), TutorialUI.AttachToH.LEFT_H);

		cubesUI.text.text = "Usa tus manos para tirar los cubos de la mesa.";

		while (!cleanedTable)
		{
			// Grip Comprobation :         if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
			if (tutorialCubesManager.cleanedTable == true && !cleanedTable)
			{
				//Instantiate(oneshotAudioPrefab, leftHandLink.transform.position, Quaternion.identity).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[0], true);
				cleanedTable = true;
				Debug.Log("CLEANED TABLE");
			}
			yield return new WaitForEndOfFrame();
		}
		cubesUI.ToggleThumbsUp(true);
		yield return new WaitForSeconds(.4f);

		//audioSource.Stop();
		//Instantiate(oneshotAudioPrefab, null).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[1], false);
		yield return new WaitForSeconds(.4f);
		Destroy(cubesUI.gameObject);
		tutorialTable.gameObject.SetActive(false);
	}

	// BASIC INTERACTION - PHYSICS + BUTTON
	IEnumerator Tutorial_1_2()
	{
		//audioSource.clip = audioClips[2];
		//audioSource.Play();
		bool cleanedButtonTable = false;
		yield return new WaitForSeconds(0.5f);

		//Instantiate The UI In The Proper Place
		tutorialButtonTable.gameObject.SetActive(true);
		tutorialButtonManager.cleanedTable = false;

		var buttonUI = Instantiate(tutorialUIPrefab,
			UIButtonAttachPoint.transform.position + new Vector3(-0.2f, 0.2f, 0f),
			Quaternion.identity).GetComponent<TutorialUI>();

		buttonUI.StartUI(playerEye, UIButtonAttachPoint.gameObject,
			new Vector3(-0.2f, 0.2f, 0f), TutorialUI.AttachToH.LEFT_H);

		buttonUI.text.text = "Presiona el botón y facilita tú trabajo.";

		while (!cleanedButtonTable)
		{
			// Grip Comprobation :         if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
			if (tutorialButtonManager.cleanedTable == true && !cleanedButtonTable)
			{
				//Instantiate(oneshotAudioPrefab, leftHandLink.transform.position, Quaternion.identity).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[0], true);
				cleanedButtonTable = true;
				Debug.Log("CLEANED TABLE");
			}
			yield return new WaitForEndOfFrame();
		}
		buttonUI.ToggleThumbsUp(true);
		yield return new WaitForSeconds(.4f);

		//audioSource.Stop();
		//Instantiate(oneshotAudioPrefab, null).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[1], false);
		yield return new WaitForSeconds(.4f);
		Destroy(buttonUI.gameObject);
		tutorialButtonTable.gameObject.SetActive(false);
	}

	// BASIC INTERACTION FUNCTION - GRIP + THROW
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

		cubeUI.text.text = "Usa el gatillo del dedo medio para agarrar y soltar objetos.";

		//tutorialInteractable Is The Explosive Cube GameObject
		//While The Required GameObject Is Not -Grabbed- And -Released- UIs Won't Dissappear
		while (!grabbed || !released)
		{
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
				Debug.Log("I LET GO");
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

		cubeUI.text.text = "Lanza el cubo al objetivo.";
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

	IEnumerator Tutorial_2_1()
	{
		//audioSource.Stop();
		//audioSource.clip = audioClips[5];
		//audioSource.Play();
		//bool thrown = false;
		//bool exploded = false;
		yield return new WaitForSeconds(.2f);
		buttonsHolder.gameObject.SetActive(true);

		var nextTutorialUI = Instantiate(tutorialUIPrefab,
			nextTutorialButton.transform.position + new Vector3(-0.2f, 0.2f, 0f),
			Quaternion.identity).GetComponent<TutorialUI>();

		nextTutorialUI.StartUI(playerEye, nextTutorialButton.gameObject,
			new Vector3(-0.2f, 0.2f, 0f), TutorialUI.AttachToH.LEFT_H);

		nextTutorialUI.text.text = "Presiona el botón para avanzar al siguiente tutorial.";

		yield return new WaitUntil(() => proceedButton);
		//Ding 

		//Instantiate(oneshotAudioPrefab, null).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[1], false);
		nextTutorialUI.ToggleThumbsUp(true);
		Destroy(explodingTarget.gameObject);

		foreach (GameObject obj in spawnedCubes)
        {
			yield return new WaitForSeconds(0.05f);
			Destroy(obj);
		}
		spawnedCubes.Clear();
		yield return new WaitForSeconds(.5f);
		Destroy(nextTutorialUI.gameObject);
		buttonsHolder.gameObject.SetActive(false);
	}

	IEnumerator Tutorial_3_0()
    {
		//audioSource.Stop();
		//audioSource.clip = audioClips[6];
		//audioSource.Play();

		yield return new WaitForSeconds(.5f);

		//Special Action Object

		actionObjectUI = Instantiate(tutorialUIPrefab,
			currentActionObject.transform.position + new Vector3(-0.2f, 0.2f, 0f),
			Quaternion.identity).GetComponent<TutorialUI>();

		actionObjectUI.StartUI(playerEye, currentActionObject.gameObject,
			new Vector3(-0.2f, 0.2f, 0f), TutorialUI.AttachToH.LEFT_H);

		actionObjectUI.text.text = "Prueba a coger el objeto y presiona el gatillo del dedo índice para accionarlo.";
		actionObjectPedestal.gameObject.SetActive(true);

		while (!actionedObject)
		{
			yield return new WaitForEndOfFrame();
		}
		actionObjectUI.ToggleThumbsUp(true);
		//Instantiate(oneshotAudioPrefab, null).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[1], false);
		yield return new WaitForSeconds(1f);
		Destroy(actionObjectUI.gameObject);
		actionObjectPedestal.SetActive(false);
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
		
		socketTutorialInitialUI.text.text = "Prueba a quitar y colocar alguno de los siguientes objetos.";

		tutorialSocketBase.gameObject.SetActive(true);
		while (!actionedSocket && !returnedSocket)
		{
			yield return new WaitForEndOfFrame();
		}
		socketTutorialInitialUI.ToggleThumbsUp(true);
		//Instantiate(oneshotAudioPrefab, null).GetComponent<OneshotAudio>().LaunchAudio(feedbackClips[1], false);
		yield return new WaitForSeconds(1f);
		Destroy(socketTutorialInitialUI.gameObject);
	}

	IEnumerator Tutorial_4_1()
	{
		//audioSource.Stop();
		//audioSource.clip = audioClips[7];
		//audioSource.Play();
		yield return new WaitForSeconds(.5f);

		leverTutorialUI = Instantiate(tutorialUIPrefab,
			leverAttachPoint.transform.position + new Vector3(-0.2f, 0.2f, 0f),
			Quaternion.identity).GetComponent<TutorialUI>();

		leverTutorialUI.StartUI(playerEye, leverAttachPoint.gameObject,
			new Vector3(-0.2f, 0.2f, 0f), TutorialUI.AttachToH.LEFT_H);

		leverTutorialUI.text.text = "Repara la palanca acercando el objeto faltante y accionala.";
		leverGameObject.gameObject.SetActive(true);
		actionedLever = false;
		while (!actionedLever)
		{
			yield return new WaitForEndOfFrame();
		}

		leverTutorialUI.ToggleThumbsUp(true);

		yield return new WaitForSeconds(1f);
		Destroy(leverTutorialUI.gameObject);
		tutorialSocketBase.gameObject.SetActive(false);
	}

	IEnumerator Tutorial_5_0()
    {
		//audioSource.Stop();
		//audioSource.clip = audioClips[7];
		//audioSource.Play();
		yield return new WaitForSeconds(.5f);

		var leftTeleportUI = Instantiate(tutorialUIPrefab,
					leftHandAttachPoint.transform.position + new Vector3(-0.2f, 0.1f, 0f),
					Quaternion.identity).GetComponent<TutorialUI>();

		leftTeleportUI.StartUI(playerEye, leftHandAttachPoint.gameObject,
					new Vector3(-0.2f, 0.1f, 0f), TutorialUI.AttachToH.LEFT_H);

		leftTeleportUI.text.text = "Mueve el joystick hacia delante.";

		teleportGameObject.gameObject.SetActive(true);

		while (!actionedTeleport)
		{
			yield return new WaitForEndOfFrame();
		}

		leftTeleportUI.ToggleThumbsUp(true);

		yield return new WaitForSeconds(1f);
		Destroy(leftTeleportUI.gameObject);
		tutorialSocketBase.gameObject.SetActive(false);
	}

	public void SpawnCubeButton()
	{
		if (proceedButton)
			return;
		GameObject newCube = Instantiate(cubePrefab, cubeSpawnPos, Quaternion.Euler(Random.Range(0f, 90f), Random.Range(0f, 90f), Random.Range(0f, 90f)));
		if (spawnedCubes.Count > 15)
		{
			Destroy(spawnedCubes[0].gameObject);
			spawnedCubes.RemoveAt(0);
		}
		spawnedCubes.Add(newCube.gameObject);
	}

	public void SmashActionObject()
	{
		if (respawningActionObject)
			return;
		respawningActionObject = true;
		Invoke("RespawnActionObject", 1.5f);
	}

	void RespawnActionObject()
	{
		respawningActionObject = false;
		var obj = Instantiate(actionObjectPrefab, actionObjectSpawnPos, Quaternion.identity);
		//To Comprobate - Pressure Sensor -
		/*obj.GetComponent<XRInteractableEvent>().onActivate.AddListener(SmashActionObject);
		obj.GetComponent<XRBaseInteractable>().onActivate.AddListener(() => actionedObject = true);*/
		//obj.GetComponent<FloorGuardian>().OnTriggerFloorGuard.AddListener(SmashActionObject);
		obj.GetComponent<XRGrabInteractable>().onActivate.AddListener((PressureSensor) => actionedObject = true);
		obj.GetComponent<FloorGuardian>().OnTriggerFloorGuard.AddListener(SmashActionObject);
		currentActionObject = obj;
		actionObjectUI.anchor = currentActionObject;
	}
}