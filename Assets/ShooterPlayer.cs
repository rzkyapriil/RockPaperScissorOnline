using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ShooterPlayer : MonoBehaviourPun
{
    [SerializeField] float speed = 5;
    [SerializeField] TMP_Text playerName;

    private void Start()
    {
        playerName.text = photonView.Owner.NickName;
    }

    void Update()
    {
        if(photonView.IsMine == false)
            return;

        Vector2 moveDir = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        transform.Translate(moveDir * Time.deltaTime * speed);
    }
}
