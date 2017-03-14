using UnityEngine;
using System.Collections;

public class FireCannon : MonoBehaviour {

    private GameObject cannon = null;
    private AudioClip fireSfx = null;
    
    private Transform firePos = null;
    private AudioSource sfx = null;

    private void Awake()
    {
        cannon = (GameObject)Resources.Load("Cannon");
        fireSfx = (AudioClip)Resources.Load("CannonFire");

        sfx = GetComponent<AudioSource>();
        firePos = this.transform;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Fire();
        }
	}

    void Fire() {
        sfx.PlayOneShot(fireSfx, 1.0f);
        Instantiate(cannon, firePos.position, firePos.rotation);
    }
}
