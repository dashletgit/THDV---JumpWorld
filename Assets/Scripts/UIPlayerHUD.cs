using Fragsurf.Movement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerHUD : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI gemCounter;
    [SerializeField] private TextMeshProUGUI speedCounter;
    //[SerializeField] private TextMeshProUGUI controls;
    public void UptadeHUDCounters(int ySpeed, int xSpeed, int gems) {
        gemCounter.text = "Gems: " + gems;
        speedCounter.text = "XSpeed: " + Mathf.Abs(xSpeed) + " " + "YSpeed: " + Mathf.Abs(ySpeed);
    }

}
