using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sickbow.Melee
{
    [Serializable]
    public class WeaponStow
    {
        [SerializeField] public WeaponStowPoint type;
        [SerializeField] public Transform point;
    }
}
