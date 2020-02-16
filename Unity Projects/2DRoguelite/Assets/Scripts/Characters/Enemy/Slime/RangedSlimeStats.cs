using UnityEngine;

public class RangedSlimeStats : EnemyStats
{
    [SerializeField] private GameObject meleeSlime;

    override protected void CharacterDeath()
    {
        float rand1 = Random.Range(0.0f, 1.5f);
        float rand2 = Random.Range(0.0f, 1.5f);
        float rand3 = Random.Range(0.0f, 1.5f);
        float rand4 = Random.Range(0.0f, 1.5f);

        Instantiate(meleeSlime, transform.position + new Vector3(rand1, rand2, 0.0f), Quaternion.identity);
        Instantiate(meleeSlime, transform.position + new Vector3(rand3, rand4, 0.0f), Quaternion.identity);

        base.CharacterDeath();
    }
}
