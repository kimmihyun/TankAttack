using UnityEngine;
using System.Collections;

public class GameMgr : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        createTank();

        PhotonNetwork.isMessageQueueRunning = true;
	}


    void createTank()
    {
        float pos = Random.Range(-100, 100);

        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20.0f, pos), Quaternion.identity, 0);
    }

}
