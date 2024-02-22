using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.Melee
{
    [CreateAssetMenu(fileName = "Melee Attack Datas", menuName = "Melee/Melee Attack Datas")]
    public class MeleeAttackDatas : ScriptableObject
    {
        public List<MeleeAttackData> data;
    }
}