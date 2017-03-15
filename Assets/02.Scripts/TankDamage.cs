using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TankDamage : MonoBehaviour {

    private MeshRenderer[] renderers;
    private GameObject expEffect = null;

    private int initHp = 100;
    private int currHp = 0;

    public Canvas hudCanvas;
    public Image hpBar;


    void Awake () {
        renderers = GetComponentsInChildren<MeshRenderer>();
        currHp = initHp;
        expEffect = (GameObject)Resources.Load("Large Explosion");

        hpBar.color = Color.green;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currHp > 0 && other.CompareTag("CANNON")) {
            currHp -= 20;
            hpBar.fillAmount = (float)currHp / (float)initHp;

            if (hpBar.fillAmount <= 0.4f) {
                hpBar.color = Color.red;
            } else if (hpBar.fillAmount <=0.65) {
                hpBar.color = Color.yellow;
            }

            if (currHp <= 0) {
                StartCoroutine(this.ExplosionTank());
            }
        }
    }

    IEnumerator ExplosionTank() {
        Object effect = GameObject.Instantiate(expEffect, transform.position, Quaternion.identity);

        Destroy(effect, 3.0f);

        hudCanvas.enabled = false;

        SetTankVisible(false);

        yield return new WaitForSeconds(3.0f);

        hpBar.fillAmount = 1.0f;
        hpBar.color = Color.green;
        hudCanvas.enabled = true;

        currHp = initHp;
        SetTankVisible(true);
    }

    void SetTankVisible(bool isVisible) {
        foreach (MeshRenderer _renderer in renderers) {
            _renderer.enabled = isVisible;
        }
    }
}
