using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sickbow.Melee
{
    [Serializable]
    public class WeaponEquip
    {
        [SerializeField] public WeaponEquipPoint type;
        [SerializeField] public Transform point;
    }
}