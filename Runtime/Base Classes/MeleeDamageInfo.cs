using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.Melee{
public class MeleeHitInfo
{
    public int damage;
    public RaycastHit ray;
    public MeleeActor sender;
    public MeleeActor receiver;
    public MeleeWeapon weapon;
    public AttackType attackType;
    public float pushScale;
}
}
