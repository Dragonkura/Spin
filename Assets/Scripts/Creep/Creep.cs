using UnityEngine;
[System.Serializable]
public class Creep : WeaponOwner
{
    public CreepType creepType;
    public bool isArgo = false;
    public Transform target;
    public Vector3 startPos; 
    public override void Start()
    {
        SetLayerForWeapon(PlayerType.Creep);
        base.Start();
    }
    public void MoveToTarget(Vector3 target)
    {
        if((target - transform.position).sqrMagnitude < 0.1f)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        var direction = (target - transform.position).normalized;
        transform.position += direction * Time.fixedDeltaTime * moveSpeed;
    }
    public void Agro(GameObject target)
    {

    }

    public override void AddWeaponToOwner(int amount)
    {

    }
    public override void Move()
    {
    }
    private void FixedUpdate()
    {
        //if (!isArgo) return;
        if (isArgo)
        {
            if(target != null) MoveToTarget(target.position);
        }
        else
        {
            MoveToTarget(startPos);
        }
    }
}
public enum CreepType
    {
        Small,
        Normal,
        Big,
    }
