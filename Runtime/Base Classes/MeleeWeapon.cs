using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Sickbow.Melee{
public abstract class MeleeWeapon : MonoBehaviour
{
    
    [SerializeField] int damage;
    [SerializeField] WeaponType weaponType;
    [SerializeField] float hitBoxWidthAndHeight; 
    [SerializeField] Transform hitBoxTip, hitBoxTail; 
    [SerializeField] bool debugAttackRegion;
    [SerializeField] WeaponEquipPoint equipPoint;
    
    protected MeleeActor _owner;

    public int GetDamage() => damage;
    public WeaponType GetWeaponType() => weaponType;
    public Transform GetTip() => hitBoxTip;
    public Transform GetTail() => hitBoxTail;
    public float GetHitboxWidthAndHeight() => hitBoxWidthAndHeight;

    public virtual void Init(MeleeActor owner) => _owner = owner;
    
    public virtual List<RaycastHit> CalculateHits()
    {
        return BoxCast();
    }

    public virtual void DebugAttackRegion(List<RaycastHit> hits)
    {
        bool hitAnything = (hits.Count > 0 && _owner.GetMeleeState() == MeleeState.Attack);
        Color col = hitAnything ? Color.red : Color.blue;
        DebugBoxCast.Draw(hitBoxTail.position, Vector3.one * hitBoxWidthAndHeight, hitBoxTip.position - hitBoxTail.position, col);
    }
    
    public virtual List<RaycastHit> BoxCast(){
        float halfWidthAndHeight = hitBoxWidthAndHeight/2;

        var hits = Physics.BoxCastAll(
            hitBoxTail.position, 
            Vector3.one * halfWidthAndHeight,
            hitBoxTip.position - hitBoxTail.position,
            Quaternion.identity, 
            (hitBoxTip.position - hitBoxTail.position).magnitude
        ).ToList<RaycastHit>();

        return hits;
    }

    public virtual void Attack(MeleeAttack attack){
        
        var hits = CalculateHits();
        if (debugAttackRegion) DebugAttackRegion(hits);
        foreach (RaycastHit hit in hits){
            var actor = hit.transform.root.GetComponent<MeleeActor>();
            actor?.ReceiveAttack(attack, hit);
        }
    }

}
}
