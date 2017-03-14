using UnityEngine;
using System.Collections;

public class TankMove : MonoBehaviour {

    public float moveSpeed = 20.0f;
    public float rotSpeed = 50.0f;

    private Rigidbody rbody;
    private Transform tr;

    private float h, v;

    private PhotonView pv = null;
    private Transform camPivot;

    void Start () {
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        rbody.centerOfMass = new Vector3(0f- 0.5f, 0f);

        pv = GetComponent<PhotonView>();
        camPivot = this.transform;

        if (pv.isMine) {
            Camera.main.GetComponent<SmoothFollow>().target = camPivot;
        }
	}
	
	// Update is called once per frame
	void Update () {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        tr.Rotate(Vector3.up * h*rotSpeed * Time.deltaTime);
        tr.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);
	}
}
