using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueCharacter : MeleeController
{
    [Header("Rogue Settings")]
    [SerializeField] private GameObject rangerArrow;

    // ------------------------------
    private bool showDebug = true;

    override protected void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.F3))
            showDebug = !showDebug;
    }

    override protected void PrimAttack()
    {
        for (int i = 0; i < enemiesInRange.Length; i++)
        {
            DamageEnemy(enemiesInRange[i].gameObject);
        }

        currentAttackDelay = 0;
    }

    protected override void SecAttack()
    {
        Vector2 shootDirection = mousePosition - playerRB.position;

        GameObject projectile           = Instantiate(rangerArrow, attackPoint.position, attackPoint.rotation);
        Rigidbody2D projectileRB        = projectile.GetComponent<Rigidbody2D>();

        projectileRB.AddForce(attackPoint.up * 20, ForceMode2D.Impulse);
    }

    private void OnGUI()
    {
        if (!showDebug)
            return;

        GUI.Label(new Rect(10, 40, 200, 20), "HP: "  + currentHealth.ToString("000") + " | Delay Charge: " + (currentAttackDelay / attackDelay).ToString("0"));
        GUI.Label(new Rect(10, 55, 200, 20), "Enemies in range: " + enemiesInRange.Length);
        GUI.Label(new Rect(10, 70, 500, 20), "Pos: " + transform.position.ToString("0.000"));
    }

    IEnumerator Dodge()
    {
        playerMoveSpeed += 20f;
        yield return new WaitForSeconds(.3f);
        playerMoveSpeed -= 20f;
    }
}
