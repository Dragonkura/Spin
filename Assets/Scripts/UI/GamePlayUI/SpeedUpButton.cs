using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUpButton : MonoBehaviour
{
    private Image countDownImage;
    private Button button;
    private float countDownTime;
    private bool ableToCountDown;
    private float timeCounter;

    private void Awake()
    {
        countDownImage = transform.Find("CountDown").GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonSpeedUpClick);
        countDownTime = 5f;
    }
    public void OnButtonSpeedUpClick()
    {
        ableToCountDown = true;
        button.interactable = false;
    }
    private void Update()
    {
        if (!ableToCountDown) return;
        if (timeCounter <= countDownTime)
        {
            timeCounter += Time.deltaTime;
            countDownImage.fillAmount = 1 - (timeCounter / countDownTime);
        }
        else
        {
            ableToCountDown = false;
            button.interactable = true;
            timeCounter = 0;

        }
    }
}
