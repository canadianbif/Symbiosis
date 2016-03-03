﻿using UnityEngine;
using System.Collections;

public class Enemy2Behavior : EnemyBehavior {

	public GameObject bullet;
	public int bulletVelocity;
	private int nextFire;

	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		timer++;
		float dist_1 = Vector3.Distance(myTransform.position, p1_Transform.position);
		float dist_2 = Vector3.Distance(myTransform.position, p2_Transform.position);

		Transform target;
		float targetDist;
		if (dist_1 < dist_2) {
			target = p1_Transform;
			targetDist = dist_1;
		} else {
			target = p2_Transform;
			targetDist = dist_2;
		}
		//rotate to look at the player
		myTransform.LookAt(target);

		Vector3 moveDirection = myTransform.forward;
		moveDirection.y = 0;
		//move towards the player
		if (targetDist > 5) {
			myTransform.position += moveDirection * moveSpeed * Time.deltaTime;
		} else if (targetDist < 3) {
			if (collisionNormal.z == -1 || collisionNormal.z == 1) {
				moveDirection.z = 0;//(-0.5f * collisionNormal.z);
			} else if (collisionNormal.x == -1 || collisionNormal.x == 1) {
				moveDirection.x = 0;//(-0.5f * collisionNormal.x);
			}
			myTransform.position -= moveDirection * (moveSpeed - 1) * Time.deltaTime;
		} else if (timer > nextFire) {
			Shoot(myTransform.forward);
		}

	}

	void Shoot(Vector3 shootDir) {
		GameObject clone = Instantiate (bullet, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation) as GameObject;
		clone.transform.rotation = Quaternion.LookRotation (shootDir);
		Physics.IgnoreCollision (clone.GetComponent<Collider> (), GetComponent<Collider> ());
		clone.GetComponent<Rigidbody> ().velocity = (clone.transform.forward * bulletVelocity);

		timer = 0;
		nextFire = Random.Range(50, 60);
	}

	public void addShootingOffset (int offset) {
		nextFire = offset;
	}
}