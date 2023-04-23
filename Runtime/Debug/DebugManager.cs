using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Sickbow.Melee{
public class DebugManager : MonoBehaviour
{
    public static DebugManager manager;
    [SerializeField] List<ActorWeaponDebugPair> _playerWeaponDebugPairs;
    [SerializeField] bool debugAllMelee; 
    private bool _oldDebugAllMelee;

    void OnValidate(){
        if (_oldDebugAllMelee != debugAllMelee)
            foreach (ActorWeaponDebugPair pwd in _playerWeaponDebugPairs)
                pwd.debug = debugAllMelee;
        
        _oldDebugAllMelee = debugAllMelee;
    }

    void Start()
    {
        if (manager == null)
            manager = this;
        else
        {
            Debug.LogError($"GameObject Singleton of type '{this.GetType()}', already exists! Destroying duplicate GameObject '{gameObject.name}' ");
            Destroy(gameObject);
        }

        
        GetPlayerWeaponPairs();
    }

    private void GetPlayerWeaponPairs()
    {
        var players = GameObject.FindGameObjectsWithTag("Player").ToList<GameObject>();
        _playerWeaponDebugPairs = new List<ActorWeaponDebugPair>(players.Count);
        foreach (GameObject g in players)
        {
            ActorWeaponDebugPair pair = new ActorWeaponDebugPair(g.GetComponent<MeleeActor>(), g.GetComponent<MeleeActor>()?.GetActiveWeapon());
            _playerWeaponDebugPairs.Add(pair);
        }
    }

    void Update()
    {
        DebugWeapons();
    }

    private void DebugWeapons()
    {
        foreach (ActorWeaponDebugPair pwd in _playerWeaponDebugPairs)
        {
            if (pwd.debug)
            {
                float hitBoxWidthAndHeight = pwd.weapon.GetHitboxWidthAndHeight();
                var tail = pwd.weapon.GetTail();
                var tip = pwd.weapon.GetTip();
                bool hitAnything = (pwd.weapon.BoxCast().Count > 0 && pwd.player.GetMeleeState() == MeleeState.Attack);
                Color col = hitAnything ? Color.red : Color.blue;
                DebugBoxCast.Draw(tail.position, Vector3.one * hitBoxWidthAndHeight, tip.position - tail.position, col);
            }
        }
    }

    
}

[Serializable]
public class ActorWeaponDebugPair {
    public ActorWeaponDebugPair(){}
    public ActorWeaponDebugPair(MeleeActor player, MeleeWeapon weapon){
        this.player = player;
        this.weapon = weapon;
    }
    public MeleeActor player;
    public MeleeWeapon weapon;
    public bool debug = true;
}
}
