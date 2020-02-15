using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyContainer", menuName = "Enemy Container Object")]
public class EnemyContainer : ScriptableObject
{
    public GameObject[] enemyPrefabs;
}
