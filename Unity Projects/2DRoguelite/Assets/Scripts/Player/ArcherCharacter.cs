using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherCharacter : RangedController
{
    [SerializeField] private float bowDrawLength;
    [SerializeField] private float dodgeLength;
    [SerializeField] private float dodgeCooldown;
    [SerializeField] private Transform[] specialFirePoints;
    [SerializeField] private float tripleShotCooldown;
    [SerializeField] private float extraShotCooldown;
    [SerializeField] private int   specialsNumber = 2;
    [SerializeField] private float specialCooldown;

    // ------------------
    [SerializeField] private int   currentSpecial = 1;
    private float currentBowDraw;
    private float currentTripleCooldown;
    private float currentExtraCooldown;
    private bool  extraShot;

    private float currentSpecialCooldown;
    private float currentDodgeCooldown;
    private float selectedSpecial;

    private bool showDebug = true;

    override protected void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.Q))
            currentSpecial = 1;

        if (Input.GetKey(KeyCode.E))
            currentSpecial = 2;

        if (Input.GetKeyDown(KeyCode.F3))
            showDebug = !showDebug;

        if (Input.GetButtonDown("CTRL"))
            ChangeSpecial();

        #region Primary Attack

        if (currentBowDraw < bowDrawLength)
        {
            if (Input.GetButton("LMB"))
            {
                currentBowDraw += Time.deltaTime;
            }

            if (Input.GetButtonUp("LMB"))
            {
                currentBowDraw = 0;
            }
        }

        if (currentBowDraw >= bowDrawLength)
        {
            if (Input.GetButtonUp("LMB"))
            {
                PrimAttack();
                currentBowDraw = 0;
            }
        }
        #endregion

        #region Secondary Attack
        if (Input.GetButtonDown("SPACE"))
        {
            SecAttack();
        }
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

    protected void ChangeSpecial()
    {
        currentSpecial += 1;

        if (currentSpecial > specialsNumber)
            currentSpecial = 1;
    }

    override protected void PrimAttack()
    {
        shootDirection = mousePosition - playerRB.position;

        GameObject tempProjectile     = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D projectileRB      = tempProjectile.GetComponent<Rigidbody2D>();
        PlayerProjectile bulletScript = tempProjectile.GetComponent<PlayerProjectile>();

        projectileRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
        bulletScript.SetDamage(damageAmount);

        if (extraShot)
        {
            damageAmount /= 2;
            moveSpeed    *= 2;
            extraShot = false;
        }
    }

    protected override void SecAttack()
    {
        switch (currentSpecial)
        {
            case 1:
                if (currentExtraCooldown >= extraShotCooldown)
                    ExtraDamageShot();
                break;

            case 2:
                if (currentTripleCooldown >= tripleShotCooldown)
                    TripleShot();
                break;
        }
    }

    private void ExtraDamageShot()
    {
        if (extraShot)
            return;

        damageAmount *= 2;
        moveSpeed    /= 2;

        extraShot            = true;
        currentExtraCooldown = 0;
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
            bulletScript.SetDamage(damageAmount);

            playerProjectile.GetComponent<TrailRenderer>().startColor = Color.blue;
            playerProjectile.GetComponent<TrailRenderer>().endColor = Color.blue;
        }

        if (extraShot)
        {
            extraShot     = false;
            damageAmount /= 2;
            moveSpeed    *= 2;
        }

        currentTripleCooldown = 0;
    }

    override protected IEnumerator Dodge()
    {
        allowMovement = false;
        moveSpeed += 15f;
        yield return new WaitForSeconds(dodgeLength);
        moveSpeed -= 15f;
        allowMovement = true;
    }

    // --- TEMP STUFF --- //
    private void OnGUI()
    {
        if (!showDebug)
            return;

        GUI.Label(new Rect(10, 40, 200, 20), "HP: " + currentHealth.ToString("000"));
        GUI.Label(new Rect(10, 55, 200, 20), "Bow: " + currentBowDraw.ToString("0.0") + " / " + bowDrawLength.ToString("0.0"));
        GUI.Label(new Rect(10, 70, 500, 20), "Pos: " + transform.position.ToString("0.000"));
    }

    public float GetDodge { get { return dodgeCooldown; } }
    public float GetCurrentDodge { get { return currentDodgeCooldown; } }
    public float GetTripleCurrent { get { return currentTripleCooldown; } }
    public float GetTripleCooldown { get { return tripleShotCooldown; } }
    public float GetExtraCurrent { get { return currentExtraCooldown; } }
    public float GetExtraCooldown { get { return extraShotCooldown; } }
}
