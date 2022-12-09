using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShooterGameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    private void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, Vector2.zero, Quaternion.identity);
    }
}
