﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuTrigger : MonoBehaviour {
	public string nextLevel;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider coll) {
		Image death = GameObject.FindGameObjectWithTag("Death").GetComponent<Image>();
		death.CrossFadeAlpha(1, .5f, true);
		StartCoroutine("load");
	}

	IEnumerator load() {
		yield return new WaitForSeconds(.5f);
		Application.LoadLevel(nextLevel);
	}



	// Update is called once per frame
	void Update () {
	
	}
}
