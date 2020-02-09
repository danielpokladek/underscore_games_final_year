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
    [SerializeField] private Transform  firePoint;
    [SerializeField] private GameObject normalProjectile;
    [SerializeField] private GameObject specialProjectile;


    [Header("Particle System")]
    [SerializeField] private ParticleSystem[] chargeParticles;
    [SerializeField] private ParticleSystem[] aimParticles;
    [SerializeField] private ParticleSystem[] shootParticles;

    [Header("Temp FX")]
    public Color particlesDraw;
    public Color particlesFullyDrawn;

    // ---
    private bool aimParticlesEnabled = false;

    override protected void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.O))
            onUIUpdateCallback.Invoke();

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
                if (!aimParticlesEnabled)
                {
                    aimParticlesEnabled = true;

                    foreach (ParticleSystem _ps in aimParticles)
                        _ps.Play();
                }

                _attackDelay = playerStats.characterAttackDelay.GetValue();
            }
        }

        if (Input.GetButtonUp("LMB"))
        {
            foreach (ParticleSystem _ps in chargeParticles)
            {
                _ps.Stop();
                _ps.Clear();
            }

            foreach (ParticleSystem _ps in aimParticles)
            {
                _ps.Stop();
                _ps.Clear();
            }

            if (_attackDelay >= playerStats.characterAttackDelay.GetValue())
            {
                ParticleSystem temp;

                foreach (ParticleSystem _ps in shootParticles)
                {
                    temp = Instantiate(_ps, firePoint.transform.position, firePoint.rotation);
                    Destroy(temp, .6f);
                }

                CameraShaker.Instance.ShakeOnce(4.0f, 2.0f, .1f, .5f);
            }

            PrimAttack();
            attackAnim.SetBool("drawingWeapon", false);
            aimParticlesEnabled = false;
        }
        #endregion

        #region Specials
        if (Input.GetKeyDown(KeyCode.Q) && _abilityTwoCooldown >= playerStats.abilityTwoCooldown.GetValue())
            ArrowNockAbility();

        if (Input.GetKeyDown(KeyCode.E) && _abilityThreeCooldown >= playerStats.abilityThreeCooldown.GetValue())
            //TripleShotAbility();

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

    override protected void PrimAttack()
    {
        float dmg = CalculateDamage();

        GameObject tempProjectile     = Instantiate(normalProjectile, firePoint.position, firePoint.rotation);
        Rigidbody2D projectileRB      = tempProjectile.GetComponent<Rigidbody2D>();
        PlayerProjectile bulletScript = tempProjectile.GetComponent<PlayerProjectile>();

        if (!aimParticlesEnabled)
        {
            tempProjectile.GetComponent<TrailRenderer>().startColor = Color.white;
            tempProjectile.GetComponent<TrailRenderer>().endColor = Color.white;
        }

        tempProjectile.transform.localScale = new Vector3(
            tempProjectile.transform.localScale.x * projectileSizeMultiplier,
            tempProjectile.transform.localScale.y * projectileSizeMultiplier,
            tempProjectile.transform.localScale.z * projectileSizeMultiplier);

        projectileRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
        bulletScript.SetDamage(dmg);

        _attackDelay = 0;
    }

    private void ArrowNockAbility()
    {
        GameObject tempProjectile = Instantiate(normalProjectile, firePoint.position, firePoint.rotation);
        Rigidbody2D projectileRB = tempProjectile.GetComponent<Rigidbody2D>();
        PlayerProjectile bulletScript = tempProjectile.GetComponent<PlayerProjectile>();

        tempProjectile.transform.localScale = new Vector3(
            tempProjectile.transform.localScale.x * projectileSizeMultiplier,
            tempProjectile.transform.localScale.y * projectileSizeMultiplier,
            tempProjectile.transform.localScale.z * projectileSizeMultiplier);

        projectileRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
        bulletScript.SetDamage(playerStats.characterAttackDamage.GetValue() * 2);

        _abilityTwoCooldown = 0;
    }

    //private void TripleShotAbility()
    //{
    //    GameObject       playerProjectile;
    //    Rigidbody2D      projectileRB;
    //    PlayerProjectile bulletScript;

    //    foreach (Transform _firePoint in tripleShotPoint)
    //    {
    //        playerProjectile  = Instantiate(normalProjectile, _firePoint.position, firePoint.rotation);

    //        projectileRB    = playerProjectile.GetComponent<Rigidbody2D>();
    //        bulletScript    = playerProjectile.GetComponent<PlayerProjectile>();

    //        playerProjectile.transform.localScale = new Vector3(
    //            playerProjectile.transform.localScale.x * projectileSizeMultiplier,
    //            playerProjectile.transform.localScale.y * projectileSizeMultiplier,
    //            playerProjectile.transform.localScale.z * projectileSizeMultiplier);

    //        projectileRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
    //        bulletScript.SetDamage(playerStats.characterAttackDamage.GetValue() / 2);

    //        playerProjectile.GetComponent<TrailRenderer>().startColor = Color.blue;
    //        playerProjectile.GetComponent<TrailRenderer>().endColor = Color.blue;
    //    }

    //    _abilityThreeCooldown = 0;
    //}

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
