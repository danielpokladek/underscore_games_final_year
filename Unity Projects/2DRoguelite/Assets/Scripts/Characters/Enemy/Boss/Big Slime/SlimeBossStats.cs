using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBossStats : EnemyStats
{
    override protected void CharacterDeath()
    {
        GetComponent<Animator>().SetTrigger("dead");
    }
}
