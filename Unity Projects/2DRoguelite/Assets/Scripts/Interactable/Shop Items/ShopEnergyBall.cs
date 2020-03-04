using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopEnergyBall : InteractableItem
{
    public GameObject energyBall;

    override public void Interact(PlayerController playerController)
    {
        if (!CheckGems())
            return;

        PurchaseItem(itemPrice);
        GameObject _go = Instantiate(
            energyBall, new Vector3(
            playerController.transform.position.x + 2.5f,
            playerController.transform.position.y,
            playerController.transform.position.z),
            Quaternion.identity);

        _go.GetComponent<EnergyBallPowerUp>().rotateCenter = playerController.transform;
        _go.transform.SetParent(playerController.powerUpContainer);

        Destroy(gameObject);
    }
}