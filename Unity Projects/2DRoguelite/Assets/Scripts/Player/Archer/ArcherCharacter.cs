﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherCharacter : RangedController
{
    [Header("Archer Settings")]
    [SerializeField] private float bowDrawLength;
    [SerializeField] private float dodgeCooldown;
    [SerializeField] private Transform[] specialFirePoints;
    [SerializeField] private float tripleShotCooldown;
    [SerializeField] private float extraShotCooldown;

    [SerializeField] private float specialCooldown;

    // ------------------
    private int   currentSpecial = 1;
    private float currentBowDraw;
    private float currentTripleCooldown;
    private float currentExtraCooldown;
    private bool  extraShot;

    private float currentSpecialCooldown;
    private float currentDodgeCooldown;
    private float selectedSpecial;

    override protected void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.Alpha1))
            currentSpecial = 1;

        if (Input.GetKey(KeyCode.Alpha2))
            currentSpecial = 2;

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
        if (Input.GetButtonDown("RMB"))
        {
            SecAttack();
        }
        #endregion

        #region Dodge

        if (currentDodgeCooldown >= dodgeCooldown)
        {
            if (Input.GetButtonDown("Dodge"))
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

    override protected void PrimAttack()
    {
        shootDirection = mousePosition - playerRB.position;

        GameObject tempProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D projectileRB = tempProjectile.GetComponent<Rigidbody2D>();
        PlayerBullet bulletScript = tempProjectile.GetComponent<PlayerBullet>();

        projectileRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
        bulletScript.SetDamage(playerDamage);

        if (extraShot)
        {
            playerDamage /= 2;
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

        playerDamage *= 2;
        moveSpeed    /= 2;

        extraShot = true;
        currentExtraCooldown = 0;
    }

    private void TripleShot()
    {
        shootDirection = mousePosition - playerRB.position;

        GameObject tempProjectile;
        Rigidbody2D projectileRB;
        PlayerBullet bulletScript;

        foreach (Transform _firePoint in specialFirePoints)
        {
            tempProjectile = Instantiate(projectilePrefab, _firePoint.position, firePoint.rotation);
            projectileRB = tempProjectile.GetComponent<Rigidbody2D>();
            bulletScript = tempProjectile.GetComponent<PlayerBullet>();

            projectileRB.AddForce(firePoint.up * 20, ForceMode2D.Impulse);
            bulletScript.SetDamage(playerDamage);

            tempProjectile.GetComponent<TrailRenderer>().startColor = Color.blue;
            tempProjectile.GetComponent<TrailRenderer>().endColor = Color.blue;
        }

        currentTripleCooldown = 0;
    }

    IEnumerator Dodge()
    {
        moveSpeed += 20f;
        yield return new WaitForSeconds(.3f);
        moveSpeed -= 20f;
    }

    // --- TEMP STUFF --- //
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 40, 200, 20), "HP: " + currentHealth.ToString("000"));
        GUI.Label(new Rect(10, 55, 200, 20), "Bow: " + currentBowDraw.ToString("0.0") + " / " + bowDrawLength.ToString("0.0"));
        GUI.Label(new Rect(10, 70, 500, 20), "Pos: " + transform.position.ToString("0.000"));
    }

    public float GetCurrentHealth { get { return currentHealth; } }
    public float GetBowDraw { get { return bowDrawLength; } }
    public float GetCurrentBowDraw { get { return currentBowDraw; } }
    public float GetDodge { get { return dodgeCooldown; } }
    public float GetCurrentDodge { get { return currentDodgeCooldown; } }
    public float GetTripleCurrent { get { return currentTripleCooldown; } }
    public float GetTripleCooldown { get { return tripleShotCooldown; } }
    public float GetExtraCurrent { get { return currentExtraCooldown; } }
    public float GetExtraCooldown { get { return extraShotCooldown; } }
}
