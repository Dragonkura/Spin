using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GamePlaySetting", menuName = "ScriptableObjects/GamePlaySetting", order = 1)]

public class GamePlaySettingConfig : ScriptableObject
{
    [Range(0,50)]
    public float  minRadius;
    [Range(0, 5)]
    public float srinkSpeed;

    public float delayTimeToSrink;
}
