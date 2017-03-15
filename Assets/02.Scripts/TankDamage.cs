using UnityEngine;
using System.Collections;

public class TankDamage : MonoBehaviour {

    private MeshRenderer[] renderers;
    private GameObject expEffect = null;

    private int initHp = 100;
    private int currHp = 0;

    void Awake () {
        renderers = GetComponentsInChildren<MeshRenderer>();
        currHp = initHp;
        expEffect = (GameObject)Resources.Load("Large Explosion");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currHp > 0 && other.CompareTag("CANNON")) {
            currHp -= 20;
            if (currHp <= 0) {
                StartCoroutine(this.ExplosionTank());
            }
        }
    }

    IEnumerator ExplosionTank() {
        Object effect = GameObject.Instantiate(expEffect, transform.position, Quaternion.identity);

        Destroy(effect, 3.0f);

        SetTankVisible(false);

        yield return new WaitForSeconds(3.0f);

        currHp = initHp;
        SetTankVisible(true);
    }

    void SetTankVisible(bool isVisible) {
        foreach (MeshRenderer _renderer in renderers) {
            _renderer.enabled = isVisible;
        }
    }
}
