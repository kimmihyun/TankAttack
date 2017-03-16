using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour {

    public Text textConnect;

    public Text txtLogMsg;
    private PhotonView pv;

	// Use this for initialization
	void Awake () {
        pv = GetComponent<PhotonView>();

        createTank();

        PhotonNetwork.isMessageQueueRunning = true;

        GetConnectPlayerCount();
    }

    private IEnumerator Start()
    {
        string msg = "\n<color=#00ff00>{" + PhotonNetwork.player.name + "} Connect</color>";
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);

        yield return new WaitForSeconds(1.0f);

        SetConnectPlayerScore();
    }

    void SetConnectPlayerScore() {
        PhotonPlayer[] players = PhotonNetwork.playerList;
        foreach (PhotonPlayer _player in players) {
            Debug.Log("[" + _player.ID + "]" + _player.name + " " + _player.GetScore() + " kill");
        }

        GameObject[] tanks = GameObject.FindGameObjectsWithTag("TANK");
        foreach (GameObject tank in tanks) {
            int currKillCount = tank.GetComponent<PhotonView>().owner.GetScore();
            tank.GetComponent<TankDamage>().txtKillCount.text = currKillCount.ToString();
        }

    }

    /// <summary>
    /// 게임이시작될때 자신의 탱크를 생성
    /// </summary>
    void createTank()
    {
        float pos = Random.Range(-100, 100);

        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20.0f, pos), Quaternion.identity, 0);
    }

    /// <summary>
    /// 접속된 플레이어의 수를 text에 넣어주는 함수
    /// </summary>
    void GetConnectPlayerCount() {
        Room currRoom = PhotonNetwork.room;

        textConnect.text = currRoom.playerCount.ToString() + "/" + currRoom.maxPlayers.ToString();
    }

    /// <summary>
    /// 네트워크 플레이어가 접속했을 때 호출되는 함수
    /// </summary>
    /// <param name="newPlayer"></param>
    void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
        GetConnectPlayerCount();
    }

    /// <summary>
    /// 네트워크 플레이어가 룸을 나가거나 접속이 끊겼을 때 호출되는 함수
    /// </summary>
    /// <param name="outPlayer"></param>
    void OnPhotonPlayerDisconnected(PhotonPlayer outPlayer) {
        GetConnectPlayerCount();
    }

    [PunRPC]
    void LogMsg(string msg) {
        txtLogMsg.text = txtLogMsg.text + msg;
    }

    public void OnClickExitRoom() {
        string msg = "\n<color=#ff0000>[" + PhotonNetwork.player.name + "]</color>";
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);

        PhotonNetwork.LeaveRoom();
    }

    void OnLeftRoom() {
        Application.LoadLevel("scLobby");
    }

}
