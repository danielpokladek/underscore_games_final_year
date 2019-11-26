using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherCharacter : RangedController
{
    [SerializeField] private float maxBowDraw;
    [SerializeField] private float dodgeLength;
    [SerializeField] private float dodgeCooldown;
    [SerializeField] private Transform[] specialFirePoints;
    [SerializeField] private float tripleShotCooldown;
    [SerializeField] private float extraShotCooldown;
    [SerializeField] private int   specialsNumber = 2;
    [SerializeField] private float specialCooldown;

    // ------------------
    [SerializeField] private int   currentSpecial = 1;
    [SerializeField] private float currentBowDraw;
    private float currentTripleCooldown;
    private float currentExtraCooldown;
    private bool  extraShot;

    private float currentSpecialCooldown;
    private float currentDodgeCooldown;
    private float selectedSpecial;

    override protected void Update()
    {
        base.Update();

        #region Primary Attack

        if (Input.GetButton("LMB"))
        {
            currentBowDraw += Time.deltaTime;

            if (currentBowDraw > maxBowDraw)
                currentBowDraw = maxBowDraw;
        }

        if (Input.GetButtonUp("LMB"))
            PrimAttack(currentDamage);

        #endregion

        #region Specials
        if (Input.GetKey(KeyCode.Q) && currentExtraCooldown >= extraShotCooldown)
            ArrowNock();

        if (Input.GetKey(KeyCode.E) && currentTripleCooldown >= tripleShotCooldown)
            TripleShot();
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

        if (currentExtraCooldown <= extraShotCooldown || extraShot)
            currentExtraCooldown += Time.deltaTime;
        #endregion
    }

    override protected void PrimAttack(float _damageAmount)
    {
        shootDirection = mousePosition - playerRB.position;

        GameObject tempProjectile     = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D projectileRB      = tempProjectile.GetComponent<Rigidbody2D>();
        PlayerProjectile bulletScript = tempProjectile.GetComponent<PlayerProjectile>();

        projectileRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
        bulletScript.SetDamage(CalculateDamage());

        if (extraShot)
        {
            currentDamage    = playerDamage;
            currentMoveSpeed = playerMoveSpeed;
            extraShot        = false;
        }

        currentBowDraw = 0;
    }

    private float CalculateDamage()
    {
        // Calculate the bow draw percentage (based on currentDraw & maxDraw),
        //  using that percentage calculate player's damage (using damageAmount),
        //  and return the damage player's will deal.

        float drawPerc = (currentBowDraw / maxBowDraw) * 100;
        float damage = (currentDamage / 100) * drawPerc;
        print(damage);

        damage = (float)System.Math.Round(damage);
        damage = Mathf.Clamp(damage, 1, 999);

        return damage;
    }

    private void ArrowNock()
    {
        if (extraShot)
            return;

        currentDamage    *= 2;
        currentMoveSpeed /= 2;

        currentExtraCooldown = 0;
        extraShot            = true;
    }

    private void TripleShot()
    {
        shootDirection = mousePosition - playerRB.position;

        GameObject       playerProjectile;
        Rigidbody2D      projectileRB;
        PlayerProjectile bulletScript;

        foreach (Transform _firePoint in specialFirePoints)
        {
            playerProjectile  = Instantiate(projectilePrefab, _firePoint.position, firePoint.rotation);

            projectileRB    = playerProjectile.GetComponent<Rigidbody2D>();
            bulletScript    = playerProjectile.GetComponent<PlayerProjectile>();

            projectileRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
            bulletScript.SetDamage(playerDamage);

            playerProjectile.GetComponent<TrailRenderer>().startColor = Color.blue;
            playerProjectile.GetComponent<TrailRenderer>().endColor = Color.blue;
        }

        if (extraShot)
        {
            extraShot            = false;
            currentDamage       /= playerDamage;
            currentMoveSpeed    *= playerMoveSpeed;
        }

        currentTripleCooldown = 0;
    }

    override protected IEnumerator Dodge()
    {
        allowMovement = false;
        playerMoveSpeed += 15f;
        yield return new WaitForSeconds(dodgeLength);
        playerMoveSpeed -= 15f;
        allowMovement = true;
    }

    public float GetDodge { get { return dodgeCooldown; } }
    public float GetCurrentDodge { get { return currentDodgeCooldown; } }
    public float GetTripleCurrent { get { return currentTripleCooldown; } }
    public float GetTripleCooldown { get { return tripleShotCooldown; } }
    public float GetExtraCurrent { get { return currentExtraCooldown; } }
    public float GetExtraCooldown { get { return extraShotCooldown; } }
}
