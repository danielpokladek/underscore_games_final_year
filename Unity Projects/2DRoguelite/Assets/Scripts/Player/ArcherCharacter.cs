using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherCharacter : RangedController
{
    [SerializeField] private float drawLength;
    [SerializeField] private float dodgeLength;
    [SerializeField] private float dodgeCooldown;
    [SerializeField] private Transform[] tripleShotPoint;
    [SerializeField] private float tripleShotCooldown;
    [SerializeField] private float arrowNockCooldown;

    // ------------------
    private float currentDrawLength;
    private float currentTripleCooldown;
    private float currentNockCooldown;
    private float currentDodgeCooldown;

    // ---

    override protected void Update()
    {
        base.Update();

        #region Primary Attack
        if (Input.GetButton("LMB"))
        {
            currentDrawLength += Time.deltaTime;

            if (currentDrawLength >= drawLength)
                currentDrawLength = drawLength;
        }

        if (Input.GetButtonUp("LMB"))
            PrimAttack();

        #endregion

        #region Specials
        if (Input.GetKeyDown(KeyCode.Q) && currentNockCooldown >= arrowNockCooldown)
            ExtraDamageShot();

        if (Input.GetKeyDown(KeyCode.E) && currentTripleCooldown >= tripleShotCooldown)
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

        projectileRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
        bulletScript.SetDamage(dmg);

        currentDrawLength = 0;
    }

    private void ExtraDamageShot()
    {
        shootDirection = mousePosition - playerRB.position;

        GameObject tempProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D projectileRB = tempProjectile.GetComponent<Rigidbody2D>();
        PlayerProjectile bulletScript = tempProjectile.GetComponent<PlayerProjectile>();

        projectileRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
        bulletScript.SetDamage(currentDamage * 2);

        currentNockCooldown = 0;
    }

    private void TripleShot()
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

            projectileRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
            bulletScript.SetDamage(1);

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

        float drawPerc = (currentDrawLength / drawLength) * 100;
        float damage   = (currentDamage / 100) * drawPerc;

        damage = Mathf.Round(damage);
        damage = Mathf.Clamp(damage, 0.5f, 999f);

        return damage;
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
    public float GetExtraCurrent { get { return currentNockCooldown; } }
    public float GetExtraCooldown { get { return arrowNockCooldown; } }
}
