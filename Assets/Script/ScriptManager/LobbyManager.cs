using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField newRoomInputField;
    [SerializeField] TMP_Text feedbackText;
    [SerializeField] Button StartGameButton;
    [SerializeField] GameObject roomPanel;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] GameObject roomListObject;
    [SerializeField] GameObject playerListObject;
    [SerializeField] RoomItem roomItemPrefab;
    [SerializeField] PlayerItem playerItemPrefab;


    List<RoomItem> roomItemList = new List<RoomItem>();
    List<PlayerItem> playerItemList = new List<PlayerItem>();

    private void Start()
    {
        PhotonNetwork.JoinLobby();
        roomPanel.SetActive(false);
    }

    public void ClickCreateRoom()
    {
        feedbackText.text = "";
        if(newRoomInputField.text.Length < 3)
        {
            feedbackText.color = Color.red; 
            feedbackText.text = "Room name min 3 characters";
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 5;
        PhotonNetwork.CreateRoom(newRoomInputField.text, roomOptions);
    }

    public void ClickStartGame(string levelName)
    {
        PhotonNetwork.LoadLevel(levelName);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room: " + PhotonNetwork.CurrentRoom.Name);
        feedbackText.color = Color.black;
        feedbackText.text = "Create room: " + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        feedbackText.color = Color.black;
        feedbackText.text = "Joined Room: " + PhotonNetwork.CurrentRoom.Name;
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        roomPanel.SetActive(true);

        //update player list
        UpdatePlayerList();

        //mengatur game start
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        //update player list
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //update player list
        UpdatePlayerList();
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        //mengatur start game
    }

    private void SetStartGameButton()
    {
        //tampilan hanya di game master
        StartGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);

        //bisa di klik atau tidak
        StartGameButton.interactable = PhotonNetwork.CurrentRoom.PlayerCount >= 2;
    }

    private void UpdatePlayerList()
    {
        foreach(var item in playerItemList)
        {
            Destroy(item.gameObject);
        }

        playerItemList.Clear();

        foreach(var(id, player) in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerListObject.transform);
            newPlayerItem.Set(player);
            playerItemList.Add(newPlayerItem);

            if (player == PhotonNetwork.LocalPlayer)
                newPlayerItem.transform.SetAsFirstSibling();
        }

        //start game hanya bisa di click ketika
        SetStartGameButton();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + "," + message);
        feedbackText.text = returnCode.ToString() + ": " + message;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(var item in this.roomItemList)
        {
            Destroy(item.gameObject);
        }

        this.roomItemList.Clear();

        foreach(var roomInfo in roomList)
        {
            RoomItem newRoomItem = Instantiate(roomItemPrefab, roomListObject.transform);
            newRoomItem.Set(this, roomInfo.Name);
            this.roomItemList.Add(newRoomItem);
        }
    }
}
