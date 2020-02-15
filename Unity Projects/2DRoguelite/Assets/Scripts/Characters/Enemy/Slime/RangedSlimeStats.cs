using UnityEngine;

public class RangedSlimeStats : EnemyStats
{
    [SerializeField] private GameObject meleeSlime;

    override protected void CharacterDeath()
    {
        Instantiate(meleeSlime, transform.position + new Vector3(1.5f, 1.5f, 0.0f), Quaternion.identity);
        Instantiate(meleeSlime, transform.position + new Vector3(-1.5f, -1.5f, 0.0f), Quaternion.identity);

        base.CharacterDeath();
    }
}
