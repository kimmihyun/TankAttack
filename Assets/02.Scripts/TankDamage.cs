﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TankDamage : MonoBehaviour {

    private MeshRenderer[] renderers;
    private GameObject expEffect = null;

    private int initHp = 100;
    private int currHp = 0;

    public Canvas hudCanvas;
    public Image hpBar;

    private PhotonView pv = null;
    public int playerId = -1;
    public int killCount = 0;
    public Text txtKillCount;

    void Awake () {
        renderers = GetComponentsInChildren<MeshRenderer>();
        currHp = initHp;
        expEffect = (GameObject)Resources.Load("Large Explosion");

        hpBar.color = Color.green;

        pv = GetComponent<PhotonView>();
        playerId = pv.ownerId;
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
                SaveKillCount(other.GetComponent<Cannon>().playerId);
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

    void SaveKillCount(int firePlayerId) {
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("TANK");

        foreach (GameObject tank in tanks) {
            var tankDamage = tank.GetComponent<TankDamage>();
            if (tankDamage != null && tankDamage.playerId == firePlayerId) {
                tankDamage.IncKillCount();
                break;
            }
        }
    }

    void IncKillCount() {
        ++killCount;
        txtKillCount.text = killCount.ToString();

        if (pv.isMine) {
            PhotonNetwork.player.AddScore(1);
        }
    }
}
