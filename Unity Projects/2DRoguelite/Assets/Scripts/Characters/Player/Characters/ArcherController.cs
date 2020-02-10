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

    // ---
    private bool bowFullyCharged = false;

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
            bowFullyCharged = false;
        }
        #endregion

        #region Specials
        if (Input.GetKeyDown(KeyCode.Q) && _abilityTwoCooldown >= playerStats.abilityTwoCooldown.GetValue())
            SecondAbility();

        if (Input.GetKeyDown(KeyCode.E) && _abilityThreeCooldown >= playerStats.abilityThreeCooldown.GetValue())
            ThirdAbility();

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

        GameObject       proj   = Instantiate(normalProjectile, firePoint.position, firePoint.rotation);
        Rigidbody2D      projRB = proj.GetComponent<Rigidbody2D>();
        PlayerProjectile pProj  = proj.GetComponent<PlayerProjectile>();

        if (!bowFullyCharged)
        {
            proj.GetComponent<TrailRenderer>().startColor = Color.white;
            proj.GetComponent<TrailRenderer>().endColor = Color.white;
        }

        //tempProjectile.transform.localScale = new Vector3(
        //    tempProjectile.transform.localScale.x * projectileSizeMultiplier,
        //    tempProjectile.transform.localScale.y * projectileSizeMultiplier,
        //    tempProjectile.transform.localScale.z * projectileSizeMultiplier);

        projRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
        pProj.SetDamage(dmg);

        _attackDelay = 0;
    }

    private void SecondAbility()
    {
        GameObject       proj   = Instantiate(normalProjectile, firePoint.position, firePoint.rotation);
        Rigidbody2D      projRB = proj.GetComponent<Rigidbody2D>();
        PlayerProjectile pProj  = proj.GetComponent<PlayerProjectile>();

        //tempProjectile.transform.localScale = new Vector3(
        //    tempProjectile.transform.localScale.x * projectileSizeMultiplier,
        //    tempProjectile.transform.localScale.y * projectileSizeMultiplier,
        //    tempProjectile.transform.localScale.z * projectileSizeMultiplier);

        projRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
        pProj.SetDamage(playerStats.characterAttackDamage.GetValue() * 2);

        _abilityTwoCooldown = 0;
    }

    private void ThirdAbility()
    {
        throw new System.NotImplementedException();

        //GameObject       proj   = Instantiate(normalProjectile, firePoint.position, firePoint.rotation);
        //Rigidbody2D      projRB = proj.GetComponent<Rigidbody2D>();
        //PlayerProjectile pProj  = proj.GetComponent<PlayerProjectile>();

        //projRB.AddForce(firePoint.up * 10, ForceMode2D.Impulse);
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
