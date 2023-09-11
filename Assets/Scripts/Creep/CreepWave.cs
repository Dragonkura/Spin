using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepWave : MonoBehaviour
{
    public CreepWaveConfigEditor creepWaveConfig;
    public SphereCollider triggerArea;
    public float distance;
    public WaveTier waveTier;
    public List<CreepConfig> creepConfigs;
    [SerializeField] Transform spawnPoint;
    [SerializeField] List<Creep> creeps = new List<Creep>();
    [System.Serializable]
    public class CreepConfig
    {
        public Creep prefab;
        public CreepType type;
    }
    public Creep GetCreep(CreepType type)
    {
        return creepConfigs.Find(x => x.type == type).prefab;
    }
    private void Start()
    {
        triggerArea.radius = creepWaveConfig.triggerColliderRadius;
        SpawnCreepWave();
    }
    public enum WaveTier
    {
        Tier1,
        Tier2,
        Tier3,
    }
    public void SpawnCreepWave()
    {
        var config = creepWaveConfig.waveConfigs.Find(x => x.waveTier == waveTier).creeps;
        var creepAmount = config.Count;
        for (int i = 0; i < config.Count; i++)
        {
            //Set Property
            var  creep = Instantiate(GetCreep(config[i]), spawnPoint);
            var creepConfig = creepWaveConfig.creepTierConfigs.Find(x => x.creepType == config[i]);
            creep.weaponSpawner.StartWeaponAmount = creepConfig.startingWaeponAmount;
            creep.maxWeaponAmount = creepConfig.startingWaeponAmount;
            creep.moveSpeed = creepConfig.creepMoveSpeed;
            //Set Pos
            creep.transform.SetLocalPosFromAngelAndDistance((360f / creepAmount) * (i + 1) * Mathf.Deg2Rad, distance);
            creep.startPos = creep.transform.position;
            creeps.Add(creep);
        }
    }
    [SerializeField] bool isArgoing;
    private void OnTriggerStay(Collider other)
    {
        if (isArgoing) return;
        if (other.gameObject.tag == TagNameConst.PLAYER)
        {
            foreach (var cp in creeps)
            {
                cp.target = other.gameObject.transform;
                cp.isArgo = true;
            }
            isArgoing = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == TagNameConst.PLAYER)
        {
            foreach (var cp in creeps)
            {
                cp.isArgo = false;
            }
            isArgoing = false;
        }
    }
  
    
}
