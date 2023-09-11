using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CreepWave;

[CreateAssetMenu(fileName = "CreepWaveConfig", menuName = "ScriptableObjects/CreepWaveConfig", order = 1)]

public class CreepWaveConfigEditor : ScriptableObject
{
    public List<WaveConfig> waveConfigs;
    public List<CreepTierConfig> creepTierConfigs;
    public float triggerColliderRadius;
}
[System.Serializable]
public class WaveConfig
{
    public WaveTier waveTier;
    public List<CreepType> creeps;
}
[System.Serializable]
public class CreepTierConfig
{
    public CreepType creepType;
    public int startingWaeponAmount;
    public float creepMoveSpeed;
}

