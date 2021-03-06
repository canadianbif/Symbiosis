﻿using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

	public string nextRoomNum;
	public char outDoor;
	private GameObject nextRoom;
	private GameObject playerCamera;
	private float cameraOffset = -3.2f;

	private Vector3 nextRoomPos;
	private GameObject player1;
	private GameObject player2;
	private GameObject playersCamera;

	private RoomController roomController;

	// Use this for initialization
	void Start () {
		nextRoom = GameObject.Find ("Room" + nextRoomNum);
		nextRoomPos = new Vector3(nextRoom.transform.position.x, nextRoom.transform.position.y, nextRoom.transform.position.z);
		roomController = transform.parent.GetComponent<RoomController> ();
		player1 = GameObject.Find ("P1");
		player2 = GameObject.Find ("P2");
		playersCamera = GameObject.Find ("CameraParentP1");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (roomController.roomCleared == true) {
			if (other.tag == "Player") {
				if (roomController.getPlayersTogether()) {
					Vector3 baseSpawnPoint = new Vector3 (nextRoomPos.x, player1.transform.position.y, nextRoomPos.z);
					baseSpawnPoint = baseSpawnPoint + OutPlacer(outDoor);

					Vector3 outPosition_1 = baseSpawnPoint + Offsetter(outDoor, true);
					player1.transform.position = outPosition_1;
					Vector3 outPosition_2 = baseSpawnPoint + Offsetter(outDoor, false);
					player2.transform.position = outPosition_2;

					playersCamera.GetComponent<CameraController>().setMaxX(nextRoomPos.x);
					playersCamera.transform.position = new Vector3 (baseSpawnPoint.x, playersCamera.transform.position.y, nextRoomPos.z + cameraOffset); 
				} else {
					Vector3 newPlayerPosition = new Vector3 (nextRoomPos.x, other.transform.position.y, nextRoomPos.z);
					newPlayerPosition = newPlayerPosition + OutPlacer (outDoor);
					other.transform.position = newPlayerPosition;
					other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

					playerCamera = GameObject.Find ("CameraParent" + other.name);
					playerCamera.GetComponent<CameraController>().NewRoomCameraPos = new Vector3 (newPlayerPosition.x, playerCamera.transform.position.y, nextRoomPos.z + cameraOffset);
					playerCamera.GetComponent<CameraController>().setMaxX(nextRoomPos.x);
				}
			}
		}
	}

	Vector3 OutPlacer(char dirChar){
		Vector3 retval;
		if (dirChar == 'n'){
			retval = new Vector3 (0, 0, 3.7f);
		} else if (dirChar == 's'){
			retval = new Vector3 (0, 0, -3.7f);
		} else if (dirChar == 'e'){
			retval = new Vector3 (7.7f, 0,0);
		} else{
			retval = new Vector3 (-7.7f, 0,0);
		}
		return retval;
	}
	Vector3 Offsetter(char dirChar, bool isPlayer_1){
		Vector3 retval;
		float x = 0.4f;
		x = (isPlayer_1 ? -x : x);

		if (dirChar == 'n' || dirChar == 's'){
			retval = new Vector3 (x, 0, 0);
		} else {
			retval = new Vector3 (0, 0, 2 * x);
		}
		return retval;
	}
}
