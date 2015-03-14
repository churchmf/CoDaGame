using UnityEngine;
using System.Collections;

[ExecuteInEditMode]

//http://createathingunity.blogspot.co.uk/
/// <summary>
/// Network manager handles initialization of players connecting to a server
/// </summary>
public class NetworkManager : MonoBehaviour
{
		public Transform playerPrefab;
		public Transform cameraPrefab;

		void OnServerInitialized ()
		{ 
				if (Network.isServer) {
						MakePlayer (Network.player);
				}
		}
	
		void OnConnectedToServer ()
		{
				networkView.RPC ("MakePlayer", RPCMode.Server, Network.player);
		}

		[RPC]
		void MakePlayer (NetworkPlayer thisPlayer)
		{
				var spawnPosition = playerPrefab.position;
				// if the client is making a player, start them on the bottom
				if (thisPlayer != Network.player) {
						spawnPosition = new Vector3 (playerPrefab.position.x, -1 * playerPrefab.position.y, playerPrefab.position.z);
				}

				Transform newPlayer = Network.Instantiate (playerPrefab, spawnPosition, playerPrefab.rotation, 0) as Transform;
				
				if (thisPlayer != Network.player) {
						networkView.RPC ("EnableCamera", thisPlayer, newPlayer.networkView.viewID);
				} else {
						EnableCamera (newPlayer.networkView.viewID);
				}
		}
	
		[RPC]
		void EnableCamera (NetworkViewID viewID)
		{
				GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
				foreach (GameObject playerObject in players) {
						if (playerObject.networkView && playerObject.networkView.viewID == viewID) {
								playerObject.GetComponent<NetworkMovement> ().haveControl = true;
								Transform myCamera = Network.Instantiate (cameraPrefab, playerPrefab.position, playerPrefab.rotation, 0) as Transform;
								myCamera.camera.enabled = true;
								myCamera.camera.GetComponent<PlayerCameraMovement> ().PlayerObject = playerObject.transform;
								myCamera.camera.GetComponent<AudioListener> ().enabled = true;
								break;
						}
				}
		}
}
