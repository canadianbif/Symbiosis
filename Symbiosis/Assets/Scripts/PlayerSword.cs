﻿using UnityEngine;
using System.Collections;

public class PlayerSword : MonoBehaviour {

	Animator swordAnimator;
	public iAugment augment;
	public float swordDamage;

	public bool isSwinging;
	private GameObject swordTrail;
	private Material trailColor;

	private Vector3 setTransform;
	private AudioPlacement AP;
	public void setAugment(iAugment aug){
		augment = aug;
	}

	// Use this for initialization
	void Start () {
		setTransform = transform.localPosition;
		AP = GameObject.Find ("AudioListener").GetComponent<AudioPlacement> ();
		swordAnimator = transform.GetComponent<Animator> ();
		foreach (Transform child in transform) {
			if (child.name == "Trail") {
				swordTrail = child.gameObject;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = setTransform;
		Color newColor = new Color ();

		if (augment != null) {
			if (augment.Element == "fire") {
				ColorUtility.TryParseHtmlString ("#C70A09", out newColor);
			} else if (augment.Element == "ice") {
				ColorUtility.TryParseHtmlString ("#033EC7", out newColor);
			} else if (augment.Element == "earth") {
				ColorUtility.TryParseHtmlString ("#0E5910", out newColor);
			}
		}else {
			ColorUtility.TryParseHtmlString ("#CCCCCC", out newColor);
		}

		Renderer renderer = transform.GetComponent<Renderer> ();
		foreach (Material matt in renderer.materials) {
			if (matt.name == "RedGlow (Instance)") {
				matt.SetColor("_Color", newColor);
			}
		}
	}

	public void StopSwinging() {
		isSwinging = false;
		swordTrail.SetActive(false);
		swordAnimator.SetTrigger ("Stop");
	}

	public void Swing() {
		isSwinging = true;

		AP.PlayClip ("SFX/sword", 0.1f);
		if (augment != null) {
			if (augment.Element == "fire") {
				trailColor = Resources.Load<Material>("RedTrail");
			} else if (augment.Element == "ice") {
				trailColor = Resources.Load<Material>("BlueTrail");
			} else if (augment.Element == "earth") {
				trailColor = Resources.Load<Material>("GreenTrail");
			}
		} else {
			trailColor = Resources.Load<Material>("WhiteTrail");
		}

		swordTrail.GetComponent<TrailRenderer>().material = trailColor;
		swordAnimator.SetTrigger ("Attack");
		StartCoroutine ("WaitAndStop");
	}

	IEnumerator WaitAndStop() {
		yield return new WaitForSeconds (0.05f);
		swordTrail.SetActive(true);
		yield return new WaitForSeconds (0.22f);

		StopSwinging();
	}

	void OnTriggerEnter(Collider c) {
		GameObject other = c.gameObject;

		if (other.tag == "Enemy") {
			string damageType = "none";
			float force = 5;
			if (augment != null) {
				augment.onHitEffect (other);
				damageType = augment.Element;
				if (augment.Element == "earth") {
					force = 20;
				}
			}

			//TODO: Add enemy Knockback
			Vector3 enemyPos = other.transform.position;
			Vector3 playerPos = transform.parent.position;
			other.GetComponent<Rigidbody>().AddForce((enemyPos - playerPos) * force, ForceMode.VelocityChange);

			EnemyStats enemyHP = other.GetComponent<EnemyStats> ();
			if (isSwinging) {
				enemyHP.TakeDamage (swordDamage, damageType);
			}
		}
	}
}

