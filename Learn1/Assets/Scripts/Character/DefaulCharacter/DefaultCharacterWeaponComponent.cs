using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCharacterWeaponComponent : CharacterWeaponComponent
{
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private GameObject bulletPrefab;

    public override void Init(CharacterConfiguration config)
    {
        base.Init(config);
        ObjectPoolManager.Instance.CreatePool("DefaultCharacter_Bullet", bulletPrefab, 20);
        if(bulletPoint == null)
        {
            Debug.LogError("Bullet Point is not assigned in MingWeaponComponent.");
        }
    }
    public override void Attack()
    {
        var bullet = ObjectPoolManager.Instance.GetPool("DefaultCharacter_Bullet").GetObject();
        bullet.transform.position = bulletPoint.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, Tool.GetMouseAngle(weaponPivot));
        var b = bullet.GetComponent<DefaultCharacter_Bullet>();
        //b.Init(new BulletDataPack(20f, Damage.FinalValue));
    }
}
