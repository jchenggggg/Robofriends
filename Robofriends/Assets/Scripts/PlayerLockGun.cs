﻿using UnityEngine;
using System.Collections;

public class PlayerLockGun : MonoBehaviour {
	public float slowdown;
	private Robot lockedRobot;
	public float maxShootDistance;
	private LineRenderer connection;	

	// Use this for initialization
	void Start () {
		connection = transform.FindChild("Gun").GetComponentInChildren<LineRenderer>();
	}

	IEnumerator fireGun() {
		Vector3 clickPos;
		Vector3 direction;
		int layerMask = LayerMask.GetMask("Platform", "Robot");
		RaycastHit hit = new RaycastHit();

		while (Input.GetButton("Fire1")) {
			clickPos =  UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
			clickPos.z = 0;

			Vector3 lineEndpoint = new Vector3();
			direction = clickPos - transform.position;

			if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, layerMask)) {
				lineEndpoint = hit.point;
			}
			else {
				direction.Normalize();
				lineEndpoint = transform.position + (direction * maxShootDistance);
			}

			GetComponent<LineRenderer>().SetPosition(0, transform.position);
			GetComponent<LineRenderer>().SetPosition(1, lineEndpoint);
			yield return null;
		}

		GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
		GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);

		Time.timeScale = 1.0f;
		Time.fixedDeltaTime = Time.fixedDeltaTime * slowdown;
		clickPos =  UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);

		direction = clickPos - transform.position;
		direction.z = 0;
		direction.Normalize();

		if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, layerMask)) {
			Robot r;
			if (r = hit.collider.gameObject.GetComponent<Robot>()) {
				if (lockedRobot) {
					lockedRobot.ReleaseParent();
				}
				r.SetParent(gameObject);
				lockedRobot = r;
				connection.SetPosition(0, this.transform.position);
				connection.SetPosition(1, r.transform.position);
				StartCoroutine("drawLine");
			}
		}
	}
	

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			Time.timeScale = Time.timeScale / slowdown;
			Time.fixedDeltaTime = Time.fixedDeltaTime / slowdown;
			StartCoroutine("fireGun");
		}
		else if (Input.GetButtonDown("Fire2")) {
			if (lockedRobot) {
				lockedRobot.ReleaseParent();
				lockedRobot = null;

				//remove connection line
				StopCoroutine("drawLine");
				connection.SetPosition(0, Vector3.zero);
				connection.SetPosition(1, Vector3.zero);
			}
		}
	}

	IEnumerator drawLine() {
		while(lockedRobot) {
			connection.SetPosition(0, this.transform.position);
			connection.SetPosition(1, lockedRobot.transform.position);
			yield return null;
		}
	}
}
