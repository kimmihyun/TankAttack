using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PhotonInit : MonoBehaviour {
    //App버전정보
    public string version = "v1.0";

    public InputField userId,roomName;

    public GameObject scrollContents, roomItem;

    private void Awake()
    {
        //포톤클라우드에 접속
        if (!PhotonNetwork.connected) PhotonNetwork.ConnectUsingSettings(version);
        userId.text = GetUserId();
        roomName.text = "ROOM_" + Random.Range(0, 999).ToString("000");
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
    public void OnClickCreateRoom() {
        string _roomName = roomName.text;

        if (string.IsNullOrEmpty(roomName.text)) {
            _roomName = "ROOM_" + Random.Range(0, 999).ToString("000");
        }

        PhotonNetwork.player.name = userId.text;
        PlayerPrefs.SetString("USER_ID", userId.text);

        RoomOptions roomOption = new RoomOptions();
        roomOption.IsOpen = true;
        roomOption.isVisible = true;
        roomOption.maxPlayers = 20;

        PhotonNetwork.CreateRoom(_roomName, roomOption, TypedLobby.Default);
    }

    void OnPhotonCreateRoomFailed(object[] codeAndMsg) {
        Debug.Log("Create Room Failed = " + codeAndMsg[1]);
    }

    void OnReceivedRoomListUpdate() {

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ROOM_ITEM")) {
            Destroy(obj);
        }

        int rowCount = 0;
        scrollContents.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        foreach (RoomInfo _room in PhotonNetwork.GetRoomList())
        {
            Debug.Log(_room.name);
            GameObject room = (GameObject)Instantiate(roomItem);

            room.transform.SetParent(scrollContents.transform, false);

            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = _room.name;
            roomData.connectPlayer = _room.playerCount;
            roomData.maxPlayers = _room.maxPlayers;

            roomData.DispRoomData();
            roomData.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { OnClickRoomItem(roomData.roomName); });
            
            scrollContents.GetComponent<GridLayoutGroup>().constraintCount = ++rowCount;
            scrollContents.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 20);
        }
    }

    void OnClickRoomItem(string roomName) {
        PhotonNetwork.player.name = userId.text;
        PlayerPrefs.SetString("USER_ID", userId.text);

        PhotonNetwork.JoinRoom(roomName);
    }

}
