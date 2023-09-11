

using UnityEngine;
using Watermelon;

public static class ExtentionHelper 
{
    public static void SetRotation(this Transform me, Vector3 value)
    {
        Quaternion newRot = Quaternion.Euler(value.x, value.y, value.z);
        me.transform.rotation = newRot;
    }
    public static void SetLocalPosX(this Transform me, float value)
    {
        var tmp = me.transform.localPosition;
        tmp.x = value;
        me.transform.localPosition = tmp;
    }
    public static Pool GetPoolByName (this PoolManager me, string name)
    {
        var pool = me.pools.Find(x => string.Equals(x.name, name));
        return pool;
    }
    public static void SetLocalPosFromAngelAndDistance(this Transform me, float angle, float distance)
    {
            var newPosX = distance * Mathf.Cos(angle);
            var newPosZ = distance * Mathf.Sin(angle);
            me.localPosition = new Vector3(newPosX, 0, newPosZ);
    }
}
