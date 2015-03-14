using UnityEngine;
using System.Collections;

public class PlayerCameraMovement : MonoBehaviour
{
		public Transform PlayerObject;
		public Vector3 CameraOffset = new Vector3 (0, 2, -10);

		void FixedUpdate ()
		{
				if (PlayerObject != null) {
						var gravityDir = PlayerObject.GetComponent<NetworkMovement> ().GravityDirection;
						Vector3 PlayerPOS = PlayerObject.position;
						transform.position = new Vector3 (PlayerPOS.x + CameraOffset.x, PlayerPOS.y + CameraOffset.y * gravityDir, PlayerPOS.z + CameraOffset.z);

						int cameraRotation = 0;
						if (gravityDir == -1) {
								cameraRotation = 180;
						}
						transform.rotation = Quaternion.Euler (0, 0, cameraRotation);
				}
		}
}
