using UnityEngine;
using System.Collections;

public class FireCannon : MonoBehaviour {

    private GameObject cannon = null;
    private AudioClip fireSfx = null;
    
    private Transform firePos = null;
    private AudioSource sfx = null;

    private PhotonView pv = null;

    private void Awake()
    {
        firePos = this.transform;

        cannon = (GameObject)Resources.Load("Cannon");
        fireSfx = (AudioClip)Resources.Load("CannonFire");

        sfx = GetComponent<AudioSource>();
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update () {

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1)) return;

        if (pv.isMine&&Input.GetMouseButtonDown(0)) {
            Fire();
            pv.RPC("Fire", PhotonTargets.Others, null);
        }
	}

    [PunRPC]
    void Fire() {
        sfx.PlayOneShot(fireSfx, 1.0f);

        GameObject _cannon = (GameObject)Instantiate(cannon, firePos.position, firePos.rotation);
        _cannon.GetComponent<Cannon>().playerId = pv.ownerId;
    }
}
