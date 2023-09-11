using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingWeapon : MonoBehaviour
{
    public Action OnTriggerWithCollectableItem;
    public Action OnTriggerWithOtherWeapon;
    public Action<RotatingWeapon> OnLosingWeapon;
    public BoxCollider boxCollider;
    [HideInInspector]
    public Vector3 startColliderSize;
    [HideInInspector]
    public Vector3 startColliderCenter;
    public WeaponElement weaponElement;
    public int idGroup;
    private void Awake()
    {
        startColliderSize = boxCollider.size;
        startColliderCenter = boxCollider.center;
    }
    public void SetBoxCollierByRadius(float radius)
    {
        var colliderSize = boxCollider.size;
        colliderSize.y = startColliderSize.y + radius / 2;
        boxCollider.size = colliderSize;
        var colliderCenter = boxCollider.center;
        colliderCenter.y = startColliderCenter.y - radius / 4;
        boxCollider.center = colliderCenter;
    }
    public void Init()
    {
        if(weaponElement != null)
        {
            weaponElement.idGroup = idGroup;
            weaponElement.gameObject.layer = this.gameObject.layer;

            weaponElement.OnTriggerWithOtherWeapon -= onTriggerWithOtherWeapon;
            weaponElement.OnTriggerWithOtherWeapon += onTriggerWithOtherWeapon;

            weaponElement.OnTriggerWithCollectableItem -= OnTriggerWithCollectableItem;
            weaponElement.OnTriggerWithCollectableItem += OnTriggerWithCollectableItem;
        }

    }
    public void onTriggerWithOtherWeapon()
    {
        gameObject.SetActive(false);
        OnTriggerWithOtherWeapon?.Invoke();
    }
    private void OnDisable()
    {
        OnLosingWeapon?.Invoke(this);
        transform.DOKill();
        boxCollider.size = startColliderSize;
        boxCollider.center = startColliderCenter;
        OnTriggerWithCollectableItem = null;
    }
}
