using UnityEngine;
using System.Collections;

//http://createathingunity.blogspot.co.uk/
/// <summary>
/// Network movement handles basic movement across the network.
/// </summary>
public class NetworkMovement : MonoBehaviour
{
		public int moveSpeed = 8;
		public bool haveControl = false;
		public float JumpThreshold = 0.1f;

		void FixedUpdate ()
		{
				if (haveControl) {
						float vert = Input.GetAxis ("Vertical");
						float horiz = Input.GetAxis ("Horizontal");

						bool wantsToJump = Input.GetButtonDown ("Jump");
						bool inAir = (transform.position.y > JumpThreshold);

						MovePlayer (vert, horiz);
						if (Network.isClient) {
								networkView.RPC ("MovePlayer", RPCMode.Server, vert, horiz);
						}
				}
		}

		[RPC]
		void MovePlayer (float vert, float horiz)
		{
				Vector3 newVelocity = (transform.right * horiz * moveSpeed) + (transform.forward * vert * moveSpeed);
				Vector3 myVelocity = rigidbody.velocity;
				myVelocity.x = newVelocity.x;
				myVelocity.z = newVelocity.z;

				rigidbody.AddTorque (myVelocity);

				// Lock Z axis
				transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
		}
}