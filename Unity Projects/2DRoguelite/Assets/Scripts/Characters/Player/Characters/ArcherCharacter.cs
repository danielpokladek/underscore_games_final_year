using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ArcherCharacter : RangedController
{
    [SerializeField] private float dodgeLength;
    [SerializeField] private float dodgeCooldown;
    [SerializeField] private Transform[] tripleShotPoint;
    [SerializeField] private float tripleShotCooldown;
    [SerializeField] private float arrowNockCooldown;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem[] chargeParticles;
    [SerializeField] private ParticleSystem[] aimParticles;
    [SerializeField] private ParticleSystem[] shootParticles;

    [SerializeField] private ParticleSystem bowParticles;
    [SerializeField] private ParticleSystem shootParticles_old;

    [Header("Temp FX")]
    public Color particlesDraw;
    public Color particlesFullyDrawn;

    bool test = false;

    // ------------------
    private float currentAttackDelay;
    private float currentTripleCooldown;
    private float currentNockCooldown;
    private float currentDodgeCooldown;

    // ---

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
            currentAttackDelay += Time.deltaTime;

            attackAnim.SetBool("drawingWeapon", true);
            attackAnim.SetFloat("perc", currentAttackDelay / attackDelay);

            if (currentAttackDelay >= attackDelay)
            {
                if (!test)
                {
                    test = true;

                    foreach (ParticleSystem _ps in aimParticles)
                        _ps.Play();
                }

                currentAttackDelay = attackDelay;
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

            if (currentAttackDelay >= attackDelay)
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
            test = false;
        }
        #endregion

        #region Specials
        if (Input.GetKeyDown(KeyCode.Q) && currentNockCooldown >= arrowNockCooldown)
            ArrowNockAbility();

        if (Input.GetKeyDown(KeyCode.E) && currentTripleCooldown >= tripleShotCooldown)
            TripleShotAbility();

        #endregion

        #region Dodge

        if (currentDodgeCooldown >= dodgeCooldown)
        {
            if (Input.GetButtonDown("RMB"))
            {
                StartCoroutine(Dodge());
                currentDodgeCooldown = 0;
            }
        }
        else if (currentDodgeCooldown <= dodgeCooldown)
        {
            currentDodgeCooldown += Time.deltaTime;
        }

        #endregion

        #region Cooldowns
        if (currentTripleCooldown <= tripleShotCooldown)
            currentTripleCooldown += Time.deltaTime;

        if (currentNockCooldown <= arrowNockCooldown)
            currentNockCooldown += Time.deltaTime;
        #endregion
    }

    override protected void PrimAttack()
    {
        float dmg = CalculateDamage();

        shootDirection = mousePosition - playerRB.position;

        GameObject tempProjectile     = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D projectileRB      = tempProjectile.GetComponent<Rigidbody2D>();
        PlayerProjectile bulletScript = tempProjectile.GetComponent<PlayerProjectile>();

        if (!test)
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

        currentAttackDelay = 0;
    }

    private void ArrowNockAbility()
    {
        shootDirection = mousePosition - playerRB.position;

        GameObject tempProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D projectileRB = tempProjectile.GetComponent<Rigidbody2D>();
        PlayerProjectile bulletScript = tempProjectile.GetComponent<PlayerProjectile>();

        tempProjectile.transform.localScale = new Vector3(
            tempProjectile.transform.localScale.x * projectileSizeMultiplier,
            tempProjectile.transform.localScale.y * projectileSizeMultiplier,
            tempProjectile.transform.localScale.z * projectileSizeMultiplier);

        projectileRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
        bulletScript.SetDamage(playerStats.characterAttackDamage.GetValue() * 2);

        currentNockCooldown = 0;
    }

    private void TripleShotAbility()
    {
        shootDirection = mousePosition - playerRB.position;

        GameObject       playerProjectile;
        Rigidbody2D      projectileRB;
        PlayerProjectile bulletScript;

        foreach (Transform _firePoint in tripleShotPoint)
        {
            playerProjectile  = Instantiate(projectilePrefab, _firePoint.position, firePoint.rotation);

            projectileRB    = playerProjectile.GetComponent<Rigidbody2D>();
            bulletScript    = playerProjectile.GetComponent<PlayerProjectile>();

            playerProjectile.transform.localScale = new Vector3(
                playerProjectile.transform.localScale.x * projectileSizeMultiplier,
                playerProjectile.transform.localScale.y * projectileSizeMultiplier,
                playerProjectile.transform.localScale.z * projectileSizeMultiplier);

            projectileRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
            bulletScript.SetDamage(playerStats.characterAttackDamage.GetValue() / 2);

            playerProjectile.GetComponent<TrailRenderer>().startColor = Color.blue;
            playerProjectile.GetComponent<TrailRenderer>().endColor = Color.blue;
        }

        currentTripleCooldown = 0;
    }

    private float CalculateDamage()
    {
        // Calculate the bow draw percentage (based on currentDraw & maxDraw),
        //  using that percentage calculate player's damage (using damageAmount),
        //  and return the damage player's will deal.

        float drawPerc = (currentAttackDelay / attackDelay) * 100;
        float damage   = (playerStats.characterAttackDamage.GetValue() / 100) * drawPerc;

        damage = Mathf.Round(damage);
        damage = Mathf.Clamp(damage, 0.5f, 999f);

        return damage;
    }

    override protected IEnumerator Dodge()
    {
        allowMovement = false;
        playerStats.characterSpeed.AddModifier(15);

        yield return new WaitForSeconds(dodgeLength);

        playerStats.characterSpeed.RemoveModifier(15);
        allowMovement = true;
    }

    public float GetDodge { get { return dodgeCooldown; } }
    public float GetCurrentDodge { get { return currentDodgeCooldown; } }
    public float GetTripleCurrent { get { return currentTripleCooldown; } }
    public float GetTripleCooldown { get { return tripleShotCooldown; } }
    public float GetExtraCurrent { get { return currentNockCooldown; } }
    public float GetExtraCooldown { get { return arrowNockCooldown; } }
}
