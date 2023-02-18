using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using Sickbow.Melee;

public class AttackTest
{
    [UnityTest]
    public IEnumerator KatanaSwingAttackHit()
    {
        var attackerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Sickbow/Prefabs/Actor.prefab");
        var attacker = GameObject.Instantiate(attackerPrefab, new Vector3(0,0,0),Quaternion.identity);
        var attackerAnimator = attacker.GetComponentInChildren<Animator>();
        
        var defenderPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Sickbow/Prefabs/Actor.prefab");
        var defender = GameObject.Instantiate(defenderPrefab, new Vector3(0,0,1.21200001f), Quaternion.AngleAxis(180,Vector3.up));
        var defenderHealth = defender.GetComponent<Health>();
        
        attackerAnimator.SetTrigger("Attack");
        
        yield return new WaitForSecondsRealtime(.65f);// attack takes .5 seconds realtime

        Assert.AreEqual(true, (defenderHealth.GetHealth() < 100)); //attacker successfully did damage 
    }

    [UnityTest]
    public IEnumerator KatanaSwingBlocked()
    {
        var attackerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Sickbow/Prefabs/Actor.prefab");
        var attacker = GameObject.Instantiate(attackerPrefab, new Vector3(0,0,0),Quaternion.identity);
        var attackerAnimator = attacker.GetComponentInChildren<Animator>();
        
        var defenderPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Sickbow/Prefabs/Actor.prefab");
        var defender = GameObject.Instantiate(defenderPrefab, new Vector3(0,0,1.21200001f), Quaternion.AngleAxis(180,Vector3.up));
        var defenderAnimator = defender.GetComponentInChildren<Animator>();
        var defenderHealth = defender.GetComponent<Health>();
        
        defenderAnimator.SetBool("Blocking", true);
        attackerAnimator.SetTrigger("Attack");
        
        yield return new WaitForSecondsRealtime(.65f);// attack takes .5 seconds realtime

        Assert.AreEqual((defenderHealth.GetHealth() == 100), true); //defender successfully blocked damage 
    }

    [UnityTest]
    public IEnumerator KatanaSwingParried()
    {
        var attackerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Sickbow/Prefabs/Actor.prefab");
        var attacker = GameObject.Instantiate(attackerPrefab, new Vector3(0,0,0),Quaternion.identity);
        var attackerAnimator = attacker.GetComponentInChildren<Animator>();
        
        var defenderPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Sickbow/Prefabs/Actor.prefab");
        var defender = GameObject.Instantiate(defenderPrefab, new Vector3(0,0,1.21200001f), Quaternion.AngleAxis(180,Vector3.up));
        var defenderAnimator = defender.GetComponentInChildren<Animator>();
        var defenderHealth = defender.GetComponent<Health>();
        var defenderActor = defender.GetComponent<MeleeActor>();
        
        bool defenderParried = false;
        
        defenderActor.ParriedHit += (MeleeHitInfo hitInfo) => defenderParried = true;
        defenderActor.SetParryWindow(100f);

        defenderAnimator.SetBool("Blocking", true);
        attackerAnimator.SetTrigger("Attack");
        
        yield return new WaitForSecondsRealtime(.65f);// attack takes .5 seconds realtime

        Assert.AreEqual(defenderParried, true); //defender successfully parried
    }
}