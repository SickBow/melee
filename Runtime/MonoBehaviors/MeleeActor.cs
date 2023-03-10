using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sickbow.Utilities;

namespace Sickbow.Melee{
public class MeleeActor : MonoBehaviour
{   
    [SerializeField] float PARRY_WINDOW = .25f;

    [SerializeField] MeleeState meleeState;
    [SerializeField] Transform weaponAnchor;
    
    private TimerGroup<MeleeState> stateTimers;
    private MeleeWeapon _activeWeapon;
    private MeleeAttack _activeAttack;
    private Dictionary<MeleeState, Action<MeleeHitInfo>> _handleAttackWhileInState;

    public Action<MeleeHitInfo> TookHit;
    public Action<MeleeHitInfo> BlockedHit;
    public Action<MeleeHitInfo> ParriedHit;
    
    void Awake()
    {
        _handleAttackWhileInState = new Dictionary<MeleeState, Action<MeleeHitInfo>>(){
            {MeleeState.Default, HandleAttackInDefault},
            {MeleeState.Attack, HandleAttackInAttack},
            {MeleeState.Block, HandleAttackInBlock},
            {MeleeState.Parry, HandleAttackInParry}
        };
        stateTimers = new TimerGroup<MeleeState>((MeleeState[])Enum.GetValues(typeof(MeleeState)));
        
        SetActiveWeapon(weaponAnchor?.GetComponentInChildren<MeleeWeapon>());
    }

    public void SetParryWindow(float window) => PARRY_WINDOW = window; 
    public MeleeAttack GetActiveAttack() => _activeAttack;
    public MeleeWeapon GetActiveWeapon() => _activeWeapon;
    public MeleeState GetMeleeState() => meleeState;

    public void SetActiveWeapon(MeleeWeapon weapon) {
        _activeWeapon = weapon;
        _activeWeapon?.Init(this);
    }    

    public void InitializeAttack(AttackType attackType){
        _activeAttack = new MeleeAttack(this, _activeWeapon, attackType);
    }

    public void Attack(){
        meleeState = MeleeState.Attack;
        UpdateTimers();

        _activeWeapon.Attack(_activeAttack);
    }             
    
    public void Block(){
        //Cannot enter parry state from block
        bool underParryTimeLimit = stateTimers.GetTime(MeleeState.Parry) < PARRY_WINDOW;
        bool notBlocking = (meleeState != MeleeState.Block);
        meleeState = (underParryTimeLimit && notBlocking) ? MeleeState.Parry : MeleeState.Block;
        UpdateTimers();

    }

    public void Default(){
        meleeState = MeleeState.Default;
        UpdateTimers();
    }

    void UpdateTimers() {
        stateTimers.ResetOtherTimers( meleeState );
        stateTimers.Tick( meleeState );
    }

    public void ReceiveAttack(MeleeAttack attack, RaycastHit hitMeHere){
        if (attack.sender == this || attack.HitAlreadyRegistered(gameObject)) 
            return;

        attack.RegisterHit(gameObject);
        
        _handleAttackWhileInState[meleeState]?.Invoke(GenerateHitInfo(attack, hitMeHere));

        Debug.Log($"MeleeActor.ReceiveAttack called on gameObject: '{gameObject.name}' ");
        
    }
    
    private void HandleAttackInDefault(MeleeHitInfo meleeHitInfo){
        TookHit?.Invoke( meleeHitInfo );
    }
    private void HandleAttackInAttack(MeleeHitInfo meleeHitInfo){
        TookHit?.Invoke( meleeHitInfo );
    }
    private void HandleAttackInBlock(MeleeHitInfo meleeHitInfo){
        BlockedHit?.Invoke( meleeHitInfo );
    }
    private void HandleAttackInParry(MeleeHitInfo meleeHitInfo){
        ParriedHit?.Invoke( meleeHitInfo );
    }

    private MeleeHitInfo GenerateHitInfo(MeleeAttack attack, RaycastHit hitMeHere){
        return new MeleeHitInfo(){
            damage = attack.GetDamage(),
            ray = hitMeHere,
            sender = attack.sender,
            weapon = attack.weapon,
            attackType = attack.attackType
        };
    }

}
}