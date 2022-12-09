using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManagerOffline : MonoBehaviour
{
    public CardPlayer P1;
    public CardPlayer P2;
    public GameState State = GameState.ChooseAttack;

    private CardPlayer damagedPlayer;
    private CardPlayer winner;
    public GameObject gameOverPanel;
    public TMP_Text winnerText;

    public enum GameState
    {
        ChooseAttack,
        Attacks,
        Damages,
        Draw,
        GameOver
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.ChooseAttack:
                if(P1.AttackValue != null && P2.AttackValue != null)
                {
                    P1.AnimateAttack();
                    P2.AnimateAttack();
                    P1.IsClickable(false);
                    P2.IsClickable(false);
                    State = GameState.Attacks;
                }
                break;
            case GameState.Attacks:
                if (P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    damagedPlayer = GetDamagePlayer();
                    if (damagedPlayer != null)
                    {
                        damagedPlayer.AnimateDamage();
                        State = GameState.Damages;
                    } 
                    else
                    {
                        P1.AnimateDraw();
                        P2.AnimateDraw();
                        State = GameState.Draw;
                    }
                }
                break;
            case GameState.Damages:
                if (P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    if(damagedPlayer == P1)
                    {
                        P1.ChangeHealth(-10);
                        P2.ChangeHealth(5);
                    }
                    else
                    {
                        P1.ChangeHealth(5);
                        P2.ChangeHealth(-10);
                    }

                    var winner = GetWinner();

                    if (winner == null)
                    {
                        ResetPlayers();
                        P1.IsClickable(true);
                        P2.IsClickable(true);
                        State = GameState.ChooseAttack;
                    }
                    else
                    {
                        FindObjectOfType<AudioManager>().PlayAudio("Win");
                        gameOverPanel.SetActive(true);
                        winnerText.text = winner == P1 ? "Player 1 Win" : "Player 2 Win";
                        ResetPlayers();
                        State = GameState.GameOver;
                    }
                }
                break;
            case GameState.Draw:
                if(P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    ResetPlayers();
                    P1.IsClickable(true);
                    P2.IsClickable(true);
                    State = GameState.ChooseAttack;
                }
                break;
        }
    }

    private void ResetPlayers()
    {
        damagedPlayer = null;
        P1.Reset();
        P2.Reset();
    }
    private CardPlayer GetDamagePlayer()
    {
        Attack? PlayerAtk1 = P1.AttackValue;
        Attack? PlayerAtk2 = P2.AttackValue;

        if(PlayerAtk1 == Attack.Batu && PlayerAtk2 == Attack.Kertas)
        {
            return P1;
        }
        else if (PlayerAtk1 == Attack.Batu && PlayerAtk2 == Attack.Gunting)
        {
            return P2;
        }
        else if (PlayerAtk1 == Attack.Kertas && PlayerAtk2 == Attack.Batu)
        {
            return P2;
        }
        else if (PlayerAtk1 == Attack.Kertas && PlayerAtk2 == Attack.Gunting)
        {
            return P1;
        }
        else if (PlayerAtk1 == Attack.Gunting && PlayerAtk2 == Attack.Batu)
        {
            return P1;
        }
        else if (PlayerAtk1 == Attack.Gunting && PlayerAtk2 == Attack.Kertas)
        {
            return P2;
        }

        return null;
    }
    private CardPlayer GetWinner()
    {
        if(P1.Health == 0)
        {
            return P2;
        }
        else if(P2.Health == 0)
        {
            return P1;
        }
        else
        {
            return null;
        }
    }
}
