using Fragsurf.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private UIPlayerHUD playerHUD;

    private int playerGemCount;
    private float ySpeed;
    private float xSpeed;
    public static GameManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }
    private void Update() {
        ySpeed = SurfCharacter.Instance.moveData.velocity.y;
        xSpeed = SurfCharacter.Instance.moveData.velocity.x + SurfCharacter.Instance.moveData.velocity.z;
        playerHUD.UptadeHUDCounters((int)ySpeed, (int)xSpeed, playerGemCount);
    }
    public void IncreaseGemCounter(int amount) { 
        playerGemCount += amount;
    }
}
