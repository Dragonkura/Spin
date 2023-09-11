using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponElement : MonoBehaviour
{ 
    public Action OnTriggerWithOtherWeapon;
    public Action OnTriggerWithCollectableItem;
    public Predicate<WeaponElement> CheckIsImmortal ;
    public int idGroup;
    private void OnTriggerEnter(Collider other)
    {
        if (CheckIsImmortal.Invoke(this)) return;
        switch (other.gameObject.tag)
        {
            case TagNameConst.WEAPON:
                var wp = other.GetComponent<WeaponElement>();
                if (idGroup != wp.idGroup)
                {
                    LauchWeapon();
                    OnTriggerWithOtherWeapon?.Invoke();
                }
                break;
            case TagNameConst.COLLECTABLE_WEAPON:
                var collectWp = other.GetComponent<CollectableItem>();
                if (collectWp.AbleToTrigger)
                {
                     OnTriggerWithCollectableItem?.Invoke();
                }
                break;
            default:
                break;
        }
    }
    [SerializeField] float forceStrength = 0.01f;

    void LauchWeapon()
    {
        var obj = Watermelon.PoolManager.GetPoolByName(PoolNameConst.COLLECTABLE_WEAPON).GetPooledObject(true);
        var temp = transform.position;
        temp.y += 1;
        obj.transform.position = temp;

        var rb = obj.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * forceStrength, ForceMode.Force);
    }
    private void OnDisable()
    {
        OnTriggerWithOtherWeapon = null;
        OnTriggerWithCollectableItem = null;
        CheckIsImmortal = null;
    }
}
