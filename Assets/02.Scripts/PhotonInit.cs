using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PhotonInit : MonoBehaviour {
    //App버전정보
    public string version = "v1.0";

    public InputField userId;

    private void Awake()
    {
        //포톤클라우드에 접속
        PhotonNetwork.ConnectUsingSettings(version);
    }

    private void OnGUI()
    {
        //화면 좌측 상단에 접속 과정에 대한 로그를 출력
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    void OnJoinedLobby() {
        Debug.Log("Entered Lobby !");
        //PhotonNetwork.JoinRandomRoom();
        userId.text = GetUserId();
    }

    string GetUserId() {
        string userId = PlayerPrefs.GetString("User_ID");
        if (string.IsNullOrEmpty(userId)) {
            userId = "USER_" + Random.Range(0, 999).ToString("000");
        }
        return userId;
    }

    void OnPhotonRandomJoinFailed() {
        Debug.Log("NO Rooms !");
        PhotonNetwork.CreateRoom("MyRoom");
    }

    void OnJoinedRoom() {
        Debug.Log("Enter Room");
        StartCoroutine(this.LoadBattleField());
    }

    IEnumerator LoadBattleField() {
        PhotonNetwork.isMessageQueueRunning = false;

        AsyncOperation ao = Application.LoadLevelAsync("scBattleField");
        yield return ao;
    }

    public void OnClickJoinRandomRoom() {
        PhotonNetwork.player.name = userId.text;
        PlayerPrefs.SetString("USER_ID", userId.text);

        PhotonNetwork.JoinRandomRoom();
    }
}
