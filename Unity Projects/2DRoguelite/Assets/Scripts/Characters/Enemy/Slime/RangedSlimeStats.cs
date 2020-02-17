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

        GameObject slime1 = Instantiate(meleeSlime, transform.position + new Vector3(rand1, rand2, 0.0f), Quaternion.identity);
        GameObject slime2 = Instantiate(meleeSlime, transform.position + new Vector3(rand3, rand4, 0.0f), Quaternion.identity);

        slime1.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        slime1.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);

        base.CharacterDeath();
    }
}
