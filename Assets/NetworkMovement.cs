using UnityEngine;
using System.Collections;

//http://createathingunity.blogspot.co.uk/
/// <summary>
/// Network movement handles basic movement across the network.
/// </summary>
public class NetworkMovement : MonoBehaviour
{
		public int moveSpeed = 12;
		public bool haveControl = false;
		public float JumpThreshold = 1;
		public float JumpForce = 500;
		public Vector3 Gravity = new Vector3 (0, -10, 0);
		public int GravityDirection = 1;
		
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

		void ApplyGravity ()
		{
				if (Physics.Raycast (transform.position, Vector3.up)) {
						GravityDirection = -1;
				}

				rigidbody.AddForce (GravityDirection * Gravity * rigidbody.mass);
		}

		[RPC]
		void MovePlayer (float horiz, bool wantsToJump)
		{
				Vector3 newVelocity = (transform.forward * horiz * moveSpeed); // + (transform.forward * vert * moveSpeed);
				Vector3 myVelocity = rigidbody.velocity;
				myVelocity.x = newVelocity.x;
				myVelocity.z = newVelocity.z;

				Debug.Log (myVelocity);

				bool inAir = (transform.position.y > GravityDirection * JumpThreshold);
				if (wantsToJump && !inAir) {
						rigidbody.AddForce (0, JumpForce * GravityDirection * rigidbody.mass, 0);
				}

				if (inAir) {
						rigidbody.AddForce (myVelocity.z, 0, 0);
				} else {
						rigidbody.AddTorque (-myVelocity);
				}
				ApplyGravity ();
		}
}