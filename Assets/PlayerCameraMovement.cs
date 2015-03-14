using UnityEngine;
using System.Collections;

public class PlayerCameraMovement : MonoBehaviour
{
		public Transform PlayerObject;
		public Vector3 CameraOffset = new Vector3 (0, 2, -10);

		void FixedUpdate ()
		{
				Vector3 PlayerPOS = PlayerObject.position;
				transform.position = new Vector3 (PlayerPOS.x + CameraOffset.x, PlayerPOS.y + CameraOffset.y, PlayerPOS.z + CameraOffset.z);
		}
}
