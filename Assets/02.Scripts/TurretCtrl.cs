using UnityEngine;
using System.Collections;

public class TurretCtrl : MonoBehaviour {

    private Transform tr;

    private RaycastHit hit;

    public float rotSpeed = 5.0f;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8)) {
            Vector3 relative = tr.InverseTransformPoint(hit.point);

            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;

            tr.Rotate(0, angle * Time.deltaTime * rotSpeed, 0);
        }
	}
}
