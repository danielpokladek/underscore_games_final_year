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

    [Header("SFX")]
    [SerializeField] private AudioClip normalShot;
    [SerializeField] private AudioClip heavyShot;

    // ---
    private bool bowFullyCharged = false;
    private bool rapidFire = false;



    override protected void Update()
    {
        base.Update();

        if (!playerAlive)
            return;

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
        if (Input.GetButtonDown("RMB") && _abilityOneCooldown >= playerStats.abilityOneCooldown.GetValue())
        {
            StartCoroutine(playerMovement.PlayerDash());
            _abilityOneCooldown = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.Q) && _abilityTwoCooldown >= playerStats.abilityTwoCooldown.GetValue())
            SecondAbility();

        if (Input.GetKeyDown(KeyCode.E) && _abilityThreeCooldown >= playerStats.abilityThreeCooldown.GetValue() && !rapidFire)
            ThirdAbility();

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
            AudioManager.current.PlaySFX(heavyShot);
        }
        else
        {
            proj = ObjectPooler.instance.PoolItem("playerNormal", firePoint.position, firePoint.rotation);
            AudioManager.current.PlaySFX(normalShot);
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

        rapidFire = true;
        _abilityTwoCooldown = 0;

        StartCoroutine(RapidFire());
    }

    // -- FIRE BOTTLE -- //
    private void ThirdAbility()
    {   
        GameObject proj = ObjectPooler.instance.PoolItem("playerThrow", firePoint.position, firePoint.rotation);
            proj.GetComponent<Projectile>().SetDamage(playerStats.characterAttackDamage.GetValue());
            proj.GetComponent<Rigidbody2D>().AddForce(firePoint.up * 10, ForceMode2D.Impulse);

        _abilityThreeCooldown = 0;
    }

    // -- RAPID FIRE COROUTINE -- //
    private IEnumerator RapidFire()
    {
        for (int i = 0; i < rapidArrowAmnt; i++)
        {
            GameObject proj = ObjectPooler.instance.PoolItem("playerNormal", firePoint.position, firePoint.rotation);
                proj.GetComponent<Projectile>().SetDamage(playerStats.characterAttackDamage.GetValue());
                proj.GetComponent<Rigidbody2D>().AddForce(firePoint.up * 15, ForceMode2D.Impulse);

            AudioManager.current.PlaySFX(heavyShot);

            yield return new WaitForSeconds(0.15f);
        }

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