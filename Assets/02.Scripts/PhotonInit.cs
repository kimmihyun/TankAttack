using UnityEngine;
using System.Collections;

public class PhotonInit : MonoBehaviour {

    //App버전정보
    public string version = "v1.0";

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
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed() {
        Debug.Log("NO Rooms !");
        PhotonNetwork.CreateRoom("MyRoom");
    }

    void OnJoinedRoom() {
        Debug.Log("Enter Room");
        CreateTank();
    }

    void CreateTank() {
        float pos = Random.Range(-100.0f, 100.0f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20.0f, pos), Quaternion.identity, 0);
    }
}
