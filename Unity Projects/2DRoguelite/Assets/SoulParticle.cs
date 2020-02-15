using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SoulParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AIDestinationSetter>().target = GameManager.current.bossPortalRef.transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            LevelManager.instance.AddSoul();

            Destroy(gameObject);
        }
    }
}
