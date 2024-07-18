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
    [SerializeField] Transform rightHandWeaponAnchor, leftHandWeaponAnchor;
    [SerializeField] List<WeaponEquip> weaponEquipPoints;
    [SerializeField] List<WeaponStow> weaponStowPoints;
    
    private TimerGroup<MeleeState> stateTimers;
    private MeleeWeapon _activeWeapon;
    private MeleeAttack _activeAttack;
    private Dictionary<MeleeState, Action<MeleeHitInfo>> _handleAttackWhileInState;

    public Action<MeleeHitInfo> TookHit;
    public Action<MeleeHitInfo> BlockedHit;
    public Action<MeleeHitInfo> ParriedHit;
    public Action<MeleeHitInfo> SentHitBlocked;
    public Action<MeleeHitInfo> SentHitParried;
    public Action<MeleeHitInfo> SentHitSuccess;
    public Action<MeleeAttack> AttackStart;
    public Action<MeleeAttack> AttackEnd;
    public Action<string> RequestWeaponSwitch;
    
    void Awake()
    {
        _handleAttackWhileInState = new Dictionary<MeleeState, Action<MeleeHitInfo>>(){
            {MeleeState.Default, HandleAttackInDefault},
            {MeleeState.Attack, HandleAttackInAttack},
            {MeleeState.Block, HandleAttackInBlock},
            {MeleeState.Parry, HandleAttackInParry}
        };
        stateTimers = new TimerGroup<MeleeState>((MeleeState[])Enum.GetValues(typeof(MeleeState)));
        
        var rightHandWeapon = rightHandWeaponAnchor?.GetComponentInChildren<MeleeWeapon>();
        var leftHandWeapon = leftHandWeaponAnchor?.GetComponentInChildren<MeleeWeapon>();
        if (rightHandWeapon != null)
            SetActiveWeapon(rightHandWeapon);
        else if (leftHandWeapon != null)
            SetActiveWeapon(leftHandWeapon);
    }

    public Transform GetWeaponEquipTransform( WeaponEquipPoint type ) => weaponEquipPoints.Find( x => x.type == type )?.point;
    public void SetWeaponEquipTransform(WeaponEquipPoint type, Transform point)
    {
        var weaponEquip = weaponEquipPoints.Find(x => x.type == type);
        if (weaponEquip != null)
            weaponEquip.point = point;
        else
            Debug.LogWarning($"Could not find equip point of type {type}");
    }
    public Transform GetWeaponStowTransform(WeaponStowPoint type) => weaponStowPoints.Find(x => x.type == type)?.point;
    public void SetWeaponStowTransform(WeaponStowPoint type, Transform point)
    {
        var weaponStow = weaponStowPoints.Find(x => x.type == type);
        if (weaponStow != null)
            weaponStow.point = point;
        else
            Debug.LogWarning($"Could not find stow point of type {type}");
    }

    public Transform GetRightHandWeaponAnchor() => rightHandWeaponAnchor;
    public void SetRightHandWeaponAnchor(Transform anchor) => rightHandWeaponAnchor = anchor;
    public Transform GetLeftHandWeaponAnchor() => leftHandWeaponAnchor;
    public void SetLeftHandWeaponAnchor(Transform anchor) => leftHandWeaponAnchor = anchor;
    public void SetParryWindow(float window) => PARRY_WINDOW = window;
    public float GetParryWindow() => PARRY_WINDOW; 
    public MeleeAttack GetActiveAttack() => _activeAttack;
    public MeleeWeapon GetActiveWeapon() => _activeWeapon;
    public MeleeState GetMeleeState() => meleeState;

    public void SetActiveWeapon(MeleeWeapon weapon) {
        _activeWeapon = weapon;
        _activeWeapon?.Init(this);
    }    

    public void InitializeAttack(AttackType attackType, AnimatorStateInfo stateInfo, float damageStart, float damageEnd, string meleeWeapon, float pushScale, MeleeAttackData attackData)
    {
        RequestWeaponSwitch?.Invoke(meleeWeapon);
        _activeAttack = new MeleeAttack(this, _activeWeapon, attackType, stateInfo, damageStart, damageEnd, pushScale, attackData);
    }
    
    public void InitializeAttack(AttackType attackType, AnimatorStateInfo stateInfo, float damageStart, float damageEnd, string meleeWeapon, float pushScale)
    {
        RequestWeaponSwitch?.Invoke(meleeWeapon);
        _activeAttack = new MeleeAttack(this, _activeWeapon, attackType, stateInfo, damageStart, damageEnd, pushScale, null);
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

        
    }
    
    private void HandleAttackInDefault(MeleeHitInfo meleeHitInfo){
        TookHit?.Invoke( meleeHitInfo );
        meleeHitInfo.sender.SentAttackSuccess( meleeHitInfo );
    }
    private void HandleAttackInAttack(MeleeHitInfo meleeHitInfo){
        TookHit?.Invoke( meleeHitInfo );
        meleeHitInfo.sender.SentAttackSuccess( meleeHitInfo );
    }
    private void HandleAttackInBlock(MeleeHitInfo meleeHitInfo){
        BlockedHit?.Invoke( meleeHitInfo );
        meleeHitInfo.sender.SentAttackBlocked( meleeHitInfo );
    }
    private void HandleAttackInParry(MeleeHitInfo meleeHitInfo){
        ParriedHit?.Invoke( meleeHitInfo );
        meleeHitInfo.sender.SentAttackParried( meleeHitInfo );
    }
    public void SentAttackBlocked(MeleeHitInfo meleeHitInfo){
        SentHitBlocked?.Invoke(meleeHitInfo);
    }
    public void SentAttackParried(MeleeHitInfo meleeHitInfo){
        SentHitParried?.Invoke(meleeHitInfo);
    }
    public void SentAttackSuccess(MeleeHitInfo meleeHitInfo){
        SentHitSuccess?.Invoke(meleeHitInfo);
    }

    private MeleeHitInfo GenerateHitInfo(MeleeAttack attack, RaycastHit hitMeHere){
        return new MeleeHitInfo(){
            damage = attack.GetDamage(),
            ray = hitMeHere,
            sender = attack.sender,
            receiver = this,
            weapon = attack.weapon,
            attackType = attack.attackType,
            pushScale = attack.pushScale,
            attackData = attack.attackData
        };
    }

}
}
