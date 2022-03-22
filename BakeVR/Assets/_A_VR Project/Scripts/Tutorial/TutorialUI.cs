using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    public enum AttachToH { LEFT_H, RIGHT_H, NONE }
    public AttachToH attachToH;

    public GameObject playerEye;
    public LineRenderer render;

    public GameObject anchor;
    public Vector3 anchorOffset;
    public GameObject leftH;
    public GameObject rightH;

    public TextMeshProUGUI text;

    //Once the proper tutorial action is done
    public Image thumbsUp;
    //public GameObject ParticleTutorial;

    float posLerpSpeed = 0f;

    public void StartUI(GameObject eye, GameObject anchor, Vector3 offset, AttachToH HandType)
    {
        playerEye = eye;
        this.anchor = anchor;
        anchorOffset = offset;
        attachToH = HandType;
        this.transform.position = eye.transform.position + eye.transform.forward;
        StartCoroutine(AttachLerp());
    }

    void Update()
    {
        if (anchor != null)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, 
                                    anchor.transform.position + anchorOffset, 
                                    Time.deltaTime * posLerpSpeed);

            int i = 0;
            if (attachToH != AttachToH.NONE)
                render.SetPosition(i++, anchor.transform.position);
            if (attachToH == AttachToH.RIGHT_H)
            {
                render.SetPosition(i++, leftH.transform.position);
                render.SetPosition(i++, rightH.transform.position);
            }
            else
            {
                render.SetPosition(i++, rightH.transform.position);
                render.SetPosition(i++, leftH.transform.position);
            }
        }
        //Rotation updating
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, 
                        Quaternion.LookRotation(this.transform.position - playerEye.transform.position),
                                                Time.deltaTime * 10f);
    }

    public void ToggleThumbsUp(bool display)
    {
        text.gameObject.SetActive(!display);
        thumbsUp.gameObject.SetActive(display);
    }

    IEnumerator AttachLerp()
    {
        text.color = Color.clear;
        while (posLerpSpeed < 10f)
        {
            posLerpSpeed += Time.deltaTime * 4f;
            yield return new WaitForEndOfFrame();
            text.color = new Color(1f, 1f, 1f, posLerpSpeed / 10f);
        }
        posLerpSpeed = 10f;
        text.color = Color.white;
    }
}
