using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Player
{
    [SerializeField] float maxBoundX = 20f;
    [SerializeField] float maxBoundZ = 20f;
    private Coroutine _autoMoveCoroutine;
    private Vector3 direction;
    private void Awake()
    {
        //need to overide the parent class
    }
    public override void Start()
    {
        SetLayerForWeapon(PlayerType.Player);
        base.Start();
        _autoMoveCoroutine = StartCoroutine(IAutoMove());
    }
    IEnumerator IAutoMove()
    {
        yield return new WaitUntil(() => GameManager.instance.isGamePlaying);
        while (GameManager.instance.isGamePlaying)
        {
            direction = (Vector3.forward * Random.Range(-1, 1) + Vector3.right * Random.Range(-1, 1)).normalized;
            if (transform.position.x < -maxBoundX) direction.x = 1;
            else if (transform.position.x > maxBoundX) direction.x = -1;
            if (transform.position.z < -maxBoundZ) direction.z = 1;
            else if (transform.position.z > maxBoundZ) direction.z = -1;
            Move();
            float delayTime = Random.Range(3, 5);
            yield return new WaitForSeconds(delayTime);
        }
    }
    //public override void OnTriggerWithOtherWeapon()
    //{
    //    CheckState();
    //    UpdateHealthBar(weaponSpawner.weaponsOwning.Count);
    //}
    public override void Move()
    {
        rb.velocity = direction * moveSpeed * Time.fixedDeltaTime;
        var charRotation = Mathf.Atan2(direction.x, direction.z) * 180 / Mathf.PI + 180;
        if (character != null)
        {
            character.SetRotation(new Vector3(0, charRotation, 0));
        }
        OnPlayerMovement?.Invoke(transform);
    }
    private void OnDisable()
    {
        if(_autoMoveCoroutine != null) StopCoroutine(_autoMoveCoroutine);
        _autoMoveCoroutine = null;
    }

    public override void Die()
    {
        gameObject.SetActive(false);
        OnPlayerMovement = null;
    }
}
