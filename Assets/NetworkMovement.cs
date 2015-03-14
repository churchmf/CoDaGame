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
		public float JumpForce = 5;

		void FixedUpdate ()
		{
				if (haveControl) {
						float horiz = Input.GetAxis ("Horizontal");
						bool wantsToJump = Input.GetButtonDown ("Jump");

						MovePlayer (horiz, wantsToJump);
						if (Network.isClient) {
								networkView.RPC ("MovePlayer", RPCMode.Server, horiz, wantsToJump);
						}
				}
		}

		[RPC]
		void MovePlayer (float horiz, bool wantsToJump)
		{
				Vector3 newVelocity = (transform.forward * -horiz * moveSpeed); // + (transform.forward * vert * moveSpeed);
				Vector3 myVelocity = rigidbody.velocity;
				myVelocity.x = newVelocity.x;
				myVelocity.z = newVelocity.z;

				bool inAir = (transform.position.y > JumpThreshold);
				if (wantsToJump && !inAir) {
						rigidbody.AddForce (0, JumpForce, 0);
				}

				rigidbody.AddTorque (myVelocity);

				// Lock Z axis
				//transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
		}
}