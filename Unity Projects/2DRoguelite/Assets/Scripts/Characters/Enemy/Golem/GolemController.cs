using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemController : EnemyRanged
{
    [SerializeField] private GameObject[] golemRockAttackPoints;
    [SerializeField] private GameObject groundAttackEffect;

    public GameObject spikething;

    GameObject[] projList;

    private float _spikeCooldown;
    private float _rockCooldown;
    private bool _canSpike;
    private bool _canRock;

    override protected void Start()
    {
        base.Start();

        projList = new GameObject[golemRockAttackPoints.Length];
    }

    override protected void Update()
    {
        base.Update();

        if (_spikeCooldown >= GetComponent<GolemStats>().golemSpikeCooldown.GetValue())
            StartCoroutine(SpikeAttack());
        if (_rockCooldown >= GetComponent<GolemStats>().golemRockCooldown.GetValue())
            StartCoroutine(GolemAttack());

        if (_spikeCooldown <= GetComponent<GolemStats>().golemSpikeCooldown.GetValue())
            _spikeCooldown += Time.deltaTime;
        if (_rockCooldown <= GetComponent<GolemStats>().golemRockCooldown.GetValue())
            _rockCooldown += Time.deltaTime;
    }

    // Golem uses custom attacks, thus it doesn't need anything in the AttackPlayer() function.
    // Overriding it and making sure its empty will avoid any issues later down the line.
    override protected void AttackPlayer() { }

    private IEnumerator GolemAttack()
    {
        _rockCooldown = 0;
        
        int i = 0;

        foreach (GameObject _attackPoint in golemRockAttackPoints)
        {
            GameObject proj = ObjectPooler.instance.PoolItem("golemNormal", _attackPoint.transform.position, Quaternion.identity);
            projList[i] = proj;
            i++;
        }

        yield return new WaitForSeconds(.5f);

        foreach (GameObject _proj in projList)
        {
            _proj.GetComponent<Rigidbody2D>().AddForce(attackPoint.up * 19, ForceMode2D.Impulse);
        }

        yield break;
    }

    private IEnumerator SpikeAttack()
    {
        _spikeCooldown = 0;

        GameObject spike = Instantiate(spikething, enemyMovement.playerTrans.position, Quaternion.identity);
        spike.GetComponent<GolemSpikeAttack>().SpikeAttack(enemyStats.characterAttackDamage.GetValue());

        yield break;
    }
}