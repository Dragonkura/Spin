using System.Collections.Generic;
using UnityEngine;

public class WeaponOwner : MonoBehaviour
{
    public WeaponOwnerSettingEditor config;
    public float moveSpeed;
    public WeaponHandler weaponSpawner;
    public int maxWeaponAmount;
    public Rigidbody rb;
    public List<RotatingWeapon> ownWeapons;
    public HeathBar heathBar;
    protected bool isImmortal;
    protected bool CheckIsImmortal(WeaponElement a)
    {
        return isImmortal;
    }
    public virtual void Start()
    {
        if (config != null)
        {
            moveSpeed = config.moveSpeed;
            weaponSpawner.StartWeaponAmount = config.startWeaponAmount;
            maxWeaponAmount = config.maxWeaponAmount;
        }
        if (maxWeaponAmount <= 0) maxWeaponAmount = int.MaxValue;
        SetIdGroup(this.GetInstanceID());
        weaponSpawner.OnTriggerWithCollectableItem -= OnTriggerWithCollectableItem;
        weaponSpawner.OnTriggerWithCollectableItem += OnTriggerWithCollectableItem;
        weaponSpawner.OnTriggerWithOtherWeapon -= OnTriggerWithOtherWeapon;
        weaponSpawner.OnTriggerWithOtherWeapon += OnTriggerWithOtherWeapon;
        weaponSpawner.CheckIsImmortal -= CheckIsImmortal;
        weaponSpawner.CheckIsImmortal += CheckIsImmortal;
        weaponSpawner.Init();
        UpdateHealthBar(weaponSpawner.weaponsOwning.Count);
        isImmortal = false;
    }
    private void SetIdGroup(int id)
    {
        weaponSpawner.idGroup = id;
    }
    protected void UpdateHealthBar(int activeWeaponAmount)
    {
        if (isImmortal) return;
        if (heathBar != null) heathBar.UpdateHeathBarSlider(1f * activeWeaponAmount / maxWeaponAmount);
    }
    public virtual void OnTriggerWithCollectableItem()
    {
        if (IsAbleToCollect())
        {
            AddWeaponToOwner(1);
            UpdateHealthBar(weaponSpawner.weaponsOwning.Count);
        }
    }
    public virtual void OnTriggerWithOtherWeapon()
    {
        CheckState();
        UpdateHealthBar(weaponSpawner.weaponsOwning.Count);
    }
    protected void SetLayerForWeapon(PlayerType type)
    {
        switch (type)
        {
            case PlayerType.Creep:
                weaponSpawner.weaponLayerIndex = LayerIndexConst.CREEP_WEAPON;
                break;
            case PlayerType.Player:
                weaponSpawner.weaponLayerIndex = LayerIndexConst.PLAYER_WEAPON;
                break;
            default:
                break;
        }
    }
    public virtual void Move()
    {
    }
    public virtual void CheckState()
    {
        if (weaponSpawner.weaponsOwning.Count == 0) Die();
    }
    public virtual bool IsAbleToCollect()
    {
        if (weaponSpawner.weaponsOwning.Count < maxWeaponAmount) return true;
        else return false;
    }
    public virtual void Die()
    {
        gameObject.SetActive(false);
    }
    public virtual void AddWeaponToOwner(int amount)
    {
        if (weaponSpawner != null) weaponSpawner.SpawnWeapon(amount);
    }
    public enum PlayerType
    {
        Creep,
        Player
    }
}
