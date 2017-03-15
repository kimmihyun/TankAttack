using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayUserId : MonoBehaviour {

    public Text userId;
    private PhotonView pv = null;

	// Use this for initialization
	void Start () {
        pv = GetComponent<PhotonView>();
        userId.text = pv.owner.name;
	}
	}
