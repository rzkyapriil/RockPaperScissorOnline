using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_Text feedbackText;

    public void ClickConnect()
    {
        feedbackText.text = "";

        if(usernameInput.text.Length < 3)
        {
            feedbackText.text = "Username min 3 character";
            return;
        }

        //simpan username
        PhotonNetwork.NickName = usernameInput.text;
        PhotonNetwork.AutomaticallySyncScene = true;

        //connect to server
        PhotonNetwork.ConnectUsingSettings();
        feedbackText.color = Color.black;
        feedbackText.text = "Connecting. . .";
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        feedbackText.text = "Connecting to Master";
        SceneManager.LoadScene("Lobby");
    }
}
