using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : WeaponOwner
{
    public VariableJoystick variableJoystick;
    public Transform character;
    public Action<Transform> OnPlayerMovement;
    private Coroutine speedUpRoutine;
    private void Awake()
    {
        var speedUpBtn = GameObject.Find("Speed Up Button").GetComponent<Button>();
        speedUpBtn.onClick.AddListener(OnSpeedUpButtonClick);
    }
    public override void Start()
    {
        SetLayerForWeapon(PlayerType.Player);
        base.Start();
    }
    private void OnSpeedUpButtonClick()
    {
        if (speedUpRoutine != null) speedUpRoutine = null;
        speedUpRoutine = StartCoroutine(IESpeedUp(3));  
    }
    IEnumerator IESpeedUp(float duration)
    {
        moveSpeed *= 2;
        yield return new WaitForSeconds(duration);
        moveSpeed /= 2;
    }
    public override void Move()
    {
        Vector3 direction = (Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal).normalized;
        rb.velocity = direction * moveSpeed * Time.fixedDeltaTime;

        var charRotation = Mathf.Atan2(direction.x, direction.z) * 180 / Mathf.PI + 180;
        if (character != null)
        {
            if (variableJoystick.isOnHandle)
                character.SetRotation(new Vector3(0, charRotation, 0));
        }
        OnPlayerMovement?.Invoke(transform);
    }

    public override void OnTriggerWithCollectableItem()
    {
        base.OnTriggerWithCollectableItem();
        var wpInfo = weaponSpawner.weaponsOwning;
        if (wpInfo.Count >= config.maxWeaponAmount)
        {
            isImmortal = true;
            TitanUp();
        }
    }
    //public override void OnTriggerWithOtherWeapon()
    //{
    //    CheckState();
    //    UpdateHealthBar(weaponSpawner.weaponsOwning.Count);
    //}


    [ContextMenu("Tian Up")]
    public void TitanUp()
    {
        var wpInfo = weaponSpawner.weaponsOwning;
        float comsumeWeaponAmountInPercent = 0.5f;
        transform.DOScale(1.5f, 1f);
        for (int i = 0; i < wpInfo.Count; i++)
        {
            if (i % 2 == 0) wpInfo[i].weaponElement.transform.DOLocalRotate(Vector3.zero, 1f);
            else wpInfo[i].weaponElement.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        moveSpeed *= 1.5f;
        heathBar.OnEndEffect -= OnEndEffect;
        heathBar.OnEndEffect += OnEndEffect;

        heathBar.DoEffectOnTitanMode(comsumeWeaponAmountInPercent, config.titanUpDuration);

    }
    public void OnEndEffect()
    {
        var wpInfo = weaponSpawner.weaponsOwning;
        float comsumeWeaponAmountInPercent = 0.5f;
        int am = (int)(wpInfo.Count * comsumeWeaponAmountInPercent);
        weaponSpawner.SetAmountWeapon(am);

        transform.DOScale(1f, 0.1f).OnComplete(() =>
        {
            for (int i = 0; i < wpInfo.Count; i++)
            {
                var trans = wpInfo[i].weaponElement.transform.DOLocalRotate(new Vector3(0, 0, -90), 0.1f);
                if (i == wpInfo.Count - 1) trans.OnComplete(() =>
                {
                    isImmortal = false;
                    moveSpeed /= 1.5f;
                });
            }

        });
    }
    public virtual void FixedUpdate()
    {
        Move();
    }
    public override void Die()
    {
        GameManager.instance.EndGame();
        OnPlayerMovement = null;
        base.Die();
    }
}
