using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    [SerializeField] protected PlayerController playerController;

    public void Item(PlayerController _playerController)
    {
        playerController = _playerController;
    }
}
