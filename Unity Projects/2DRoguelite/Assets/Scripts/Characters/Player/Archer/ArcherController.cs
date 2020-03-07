using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

/*
 * Archer Special Abilities:
 *      Ability 1 : Dash
 *      Ability 2 : Piercing Shot
 *      Ability 3 : Throwable Item
 */

public class ArcherController : PlayerController
{
    [Header("Attacks")]
    #if UNITY_EDITOR
    [Help(  "Attack    : LMB - Normal Attack\n" +
            "Ability 1 : RMB - Dash\n" +
            "Ability 2 : Q   - Piercing Shot\n" +
            "Ability 3 : E   - Throwable\n", UnityEditor.MessageType.None)]
    #endif
    [SerializeField] private Transform  firePoint;
    [SerializeField] private GameObject normalProjectile;
    [SerializeField] private GameObject specialProjectile;


    [Header("Particle System")]
    [SerializeField] private ParticleSystem[] chargeParticles;
    [SerializeField] private ParticleSystem[] aimParticles;
    [SerializeField] private ParticleSystem[] shootParticles;
    [SerializeField] private Color particlesDraw;
    [SerializeField] private Color particlesFullyDrawn;
    [SerializeField] private int rapidArrowAmnt = 20;

    // ---
    private bool bowFullyCharged = false;
    private bool rapidFire = false;

    override protected void Update()
    {
        base.Update();

        #region Primary Attack
        if (Input.GetButtonDown("LMB"))
        {
            //bowParticles.Play();
            foreach (ParticleSystem _ps in chargeParticles)
                _ps.Play();
        }

        if (Input.GetButton("LMB"))
        {
            _attackDelay += Time.deltaTime;

            attackAnim.SetBool("drawingWeapon", true);
            attackAnim.SetFloat("perc", _attackDelay / playerStats.characterAttackDelay.GetValue());

            if (_attackDelay >= playerStats.characterAttackDelay.GetValue())
            {
                if (!bowFullyCharged)
                {
                    bowFullyCharged = true;

                    foreach (ParticleSystem _ps in aimParticles)
                        _ps.Play();
                }

                _attackDelay = playerStats.characterAttackDelay.GetValue();
            }
        }

        if (Input.GetButtonUp("LMB"))
        {
            foreach (ParticleSystem _chargeParticles in chargeParticles)
            {
                _chargeParticles.Stop();
                _chargeParticles.Clear();
            }

            foreach (ParticleSystem _aimParticles in aimParticles)
            {
                _aimParticles.Stop();
                _aimParticles.Clear();
            }

            if (_attackDelay >= playerStats.characterAttackDelay.GetValue())
            {
                foreach (ParticleSystem _chargedParticles in shootParticles)
                {
                    ParticleSystem ps = Instantiate(_chargedParticles, firePoint.transform.position, firePoint.rotation);
                    Destroy(ps.gameObject, .6f);
                }

                CameraShaker.Instance.ShakeOnce(10.0f, 6.0f, .1f, .5f);
            }

            PrimAttack();
            attackAnim.SetBool("drawingWeapon", false);
            bowFullyCharged = false;
        }
        #endregion

        #region Specials
        if (Input.GetKeyDown(KeyCode.Q) && _abilityTwoCooldown >= playerStats.abilityTwoCooldown.GetValue())
            SecondAbility();

        if (Input.GetKeyDown(KeyCode.E) && _abilityThreeCooldown >= playerStats.abilityThreeCooldown.GetValue() && !rapidFire)
        {
            ThirdAbility();
            rapidFire = true;
        }

        #endregion

        #region Dodge

        if (_abilityOneCooldown >= playerStats.abilityOneCooldown.GetValue())
        {
            if (Input.GetButtonDown("RMB"))
            {
                StartCoroutine(playerMovement.PlayerDash());
                _abilityOneCooldown = 0;
            }
        }
        else if (_abilityOneCooldown <= playerStats.abilityOneCooldown.GetValue())
        {
            _abilityOneCooldown += Time.deltaTime;
        }

        #endregion

        #region Cooldowns
        if (_abilityTwoCooldown <= playerStats.abilityTwoCooldown.GetValue())
            _abilityTwoCooldown += Time.deltaTime;

        if (_abilityThreeCooldown <= playerStats.abilityThreeCooldown.GetValue())
            _abilityThreeCooldown += Time.deltaTime;
        #endregion
    }

    // -- PRIMARY "LMB" SHOT -- //
    override protected void PrimAttack()
    {
        float dmg = CalculateDamage();
        GameObject proj;


        if (bowFullyCharged)
        {
            proj = ObjectPooler.instance.PoolItem("playerPiercing", firePoint.position, firePoint.rotation);
            proj.GetComponent<TrailRenderer>().enabled = true;
        }
        else
        {
            proj = ObjectPooler.instance.PoolItem("playerNormal", firePoint.position, firePoint.rotation);
        }

        proj.GetComponent<Rigidbody2D>().AddForce(firePoint.up * 20, ForceMode2D.Impulse);
        proj.GetComponent<Projectile>().SetDamage(dmg);

        _attackDelay = 0;
    }

    // -- RAPID FIRE ABILITY -- //
    private void SecondAbility()
    {
        if (rapidFire)
            return;

        StartCoroutine(RapidFire());
    }

    // -- FIRE BOTTLE -- //
    private void ThirdAbility()
    {   
        GameObject proj = ObjectPooler.instance.PoolItem("playerThrow", firePoint.position, firePoint.rotation);
            proj.GetComponent<Projectile>().SetDamage(playerStats.characterAttackDamage.GetValue());
            proj.GetComponent<Rigidbody2D>().AddForce(firePoint.up * 10, ForceMode2D.Impulse);

        _abilityThreeCooldown = 0;
        rapidFire = false;
    }

    // -- RAPID FIRE COROUTINE -- //
    private IEnumerator RapidFire()
    {
        for (int i = 0; i < rapidArrowAmnt; i++)
        {
            GameObject proj = ObjectPooler.instance.PoolItem("playerNormal", firePoint.position, firePoint.rotation);
                proj.GetComponent<Projectile>().SetDamage(playerStats.characterAttackDamage.GetValue());
                proj.GetComponent<Rigidbody2D>().AddForce(firePoint.up * 10, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.15f);
        }

        _abilityTwoCooldown = 0;
        rapidFire = false;

        yield break;
    }


    private float CalculateDamage()
    {
        // Calculate the bow draw percentage (based on currentDraw & maxDraw),
        //  using that percentage calculate player's damage (using damageAmount),
        //  and return the damage player's will deal.

        float drawPerc = (_attackDelay / playerStats.characterAttackDelay.GetValue()) * 100;
        float damage   = (playerStats.characterAttackDamage.GetValue() / 100) * drawPerc;

        damage = Mathf.Round(damage);
        damage = Mathf.Clamp(damage, 0.5f, 999f);

        return damage;
    }
}