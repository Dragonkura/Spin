using System;
using System.Collections.Generic;
using UnityEngine;
using Watermelon;

public class WeaponHandler : MonoBehaviour
{
    public Action OnPooledObject;
    public Transform holder;
    public int weaponLayerIndex;
    public int idGroup;
    public Action OnTriggerWithCollectableItem;
    public List<RotatingWeapon> weaponsOwning;
    public Predicate<WeaponElement> CheckIsImmortal;

    [SerializeField] int startWeaponAmount = 10;
    public int StartWeaponAmount
    {
        get { return startWeaponAmount; }
        set { startWeaponAmount = value; }
    }

    public Action OnTriggerWithOtherWeapon;

    [SerializeField] float minRadius = 2f;

    public void Init()
    {
        for (int i = 0; i < startWeaponAmount; i++)
        {
            SpawnWeapon();
        }
    }
    //public (int activeWeaponAmount, List<RotatingWeapon> lstActiveObject) GetOwnWeaponsInfo()
    //{
    //    List<RotatingWeapon> pooledObjects = new List<RotatingWeapon>();
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        var child = transform.GetChild(i);
    //        if (child.gameObject.activeSelf) pooledObjects.Add(child.GetComponent<RotatingWeapon>());
    //    }
    //    if (pooledObjects == null) return(0, null);
    //    List<RotatingWeapon> lstActiveObject = new List<RotatingWeapon>();
    //    for (int i = 0; i < pooledObjects.Count; i++)
    //    {
    //        if (pooledObjects[i].gameObject.activeSelf) lstActiveObject.Add(pooledObjects[i]);
    //    }
    //    var activeWeaponAmount = lstActiveObject.Count;
    //    return (activeWeaponAmount, lstActiveObject);
    //}
    public void SetAmountWeapon(int AmountToSet)
    {
        if (AmountToSet >= weaponsOwning.Count) return;
        for (int i = 0; i < AmountToSet - 1; i++)
        {
            weaponsOwning[i].gameObject.SetActive(false);
        }
    }

    public void SpawnWeapon(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            var obj = PoolManager.GetPoolByName(PoolNameConst.ROTATING_WEAPON ).GetPooledObject(true);
            obj.transform.SetParent(holder);
            obj.transform.localScale = Vector3.one;
            var wp = obj.GetComponent<RotatingWeapon>();
            weaponsOwning.Add(wp);
            wp.weaponElement.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
            wp.idGroup = idGroup;
            wp.gameObject.layer = weaponLayerIndex;

            wp.OnTriggerWithCollectableItem -= OnTriggerWithCollectableItem;
            wp.OnTriggerWithCollectableItem += OnTriggerWithCollectableItem;

            wp.OnTriggerWithOtherWeapon -= OnTriggerWithOtherWeapon;
            wp.OnTriggerWithOtherWeapon += OnTriggerWithOtherWeapon;

            wp.weaponElement.CheckIsImmortal -= CheckIsImmortal;
            wp.weaponElement.CheckIsImmortal += CheckIsImmortal;

            wp.OnLosingWeapon -= OnChangeWeaponAmount;
            wp.OnLosingWeapon += OnChangeWeaponAmount;
        }
        
        CaculateNewTranform();
        OnPooledObject?.Invoke();
    }
    public void OnChangeWeaponAmount(RotatingWeapon wp)
    {
        if (weaponsOwning.Contains(wp)) weaponsOwning.Remove(wp);
        else return;
        CaculateNewTranform();
    }
    private void CaculateNewTranform()
    {
        //Get avtice Weapon
        
        var activeWeaponAmount = weaponsOwning.Count;
        var lstActiveObject = weaponsOwning;
        if (activeWeaponAmount == 0) return;
        //Caculation
        var rotationBase = new Vector3(0, 1, 0) * (360f / activeWeaponAmount);
        float radiusMultiplier = 1.5f;
        var radius = radiusMultiplier * activeWeaponAmount / (2 * Mathf.PI * 2);//(radiusMultiplier * (1 + (0.5f * objectAmount / startWeaponAmount)));
        if (radius < minRadius) radius = minRadius;
        for (int i = 0; i < lstActiveObject.Count; i++)
        {
            var obj = lstActiveObject[i];
            obj.SetBoxCollierByRadius(radius);
            obj.transform.SetRotation(rotationBase * (i + 1));
            obj.transform.localPosition = Vector3.zero;
            obj.Init();
            var rotatingWeapon = obj.weaponElement;
            rotatingWeapon.transform.SetLocalPosX(radius);
        }
    }

}

