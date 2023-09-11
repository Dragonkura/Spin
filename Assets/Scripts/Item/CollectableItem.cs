using System.Collections;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    private Coroutine _coroutine;
    public ItemType itemType;
    private bool ableToTrigger;
    public bool AbleToTrigger
    {
        get
        { return ableToTrigger; }
        private set
        {
            ableToTrigger = value;
        }
    }
    public enum ItemType
    {
        ItemWeapon,
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == TagNameConst.COLLECTABLE_WEAPON) return;
        if (gameObject.activeSelf) _coroutine = StartCoroutine(ISetAbleToCollect());
    }
    IEnumerator ISetAbleToCollect()
    {
        var objectMaterial = gameObject.GetComponent<MeshRenderer>().material;
        // Set the material's transparency to 50%
        objectMaterial.color = new Color(objectMaterial.color.r, objectMaterial.color.g, objectMaterial.color.b, 25f / 255);
        var delayTime = 0.5f;

        yield return new WaitForSeconds(delayTime);
        objectMaterial.color = new Color(objectMaterial.color.r, objectMaterial.color.g, objectMaterial.color.b, 1f);
        SetTrigger(true);
        _coroutine = null;
        yield break;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!ableToTrigger) return;
        switch (itemType)
        {
            case ItemType.ItemWeapon:
                if (other.gameObject.tag == TagNameConst.WEAPON)
                {
                    var wp = other.GetComponent<WeaponElement>();
                    if(!wp.CheckIsImmortal.Invoke(wp)) gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
    private void OnDisable()
    {
        SetTrigger(false);
    }

    private void SetTrigger(bool setment)
    {
        ableToTrigger = setment;
    }
}
