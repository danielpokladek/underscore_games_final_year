using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeartSystem
{
    public const float MAX_HEART_LEVEL = 1;

    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;

    private List<Heart> heartList;

    public PlayerHeartSystem(int heartAmount)
    {
        heartList = new List<Heart>();

        for (int i = 0; i < heartAmount; i++)
        {
            Heart heart = new Heart(1);
            heartList.Add(heart);
        }
    }

    public List<Heart> GetHeartList()
    {
        return heartList;
    }

    public void Damage(float damageAmount)
    {
        for (int i = heartList.Count - 1; i >= 0; i--)
        {
            Heart heart = heartList[i];

            if (damageAmount > heart.GetHeartLevel())
            {
                damageAmount -= heart.GetHeartLevel();
                heart.Damage(heart.GetHeartLevel());
            }
            else
            {
                heart.Damage(damageAmount);
                break;
            }
        }

        if (OnDamaged != null) OnDamaged(this, EventArgs.Empty);
    }

    public void Heal(float healAmount)
    {
        for (int i = 0; i < heartList.Count; i++)
        {
            Heart heart = heartList[i];
            float missingLevel = MAX_HEART_LEVEL - heart.GetHeartLevel();

            if (healAmount > missingLevel)
            {
                healAmount -= missingLevel;
                heart.Heal(missingLevel);
            }
            else
            {
                heart.Heal(healAmount);
                break;
            }
        }

        if (OnHealed != null) OnHealed(this, EventArgs.Empty);
    }

    public class Heart
    {
        private float heartLevel;

        public Heart(float heartLevel)
        {
            this.heartLevel = heartLevel;
        }

        public void SetHeartLevel(int heartLevel)
        {
            this.heartLevel = heartLevel;
        }

        public float GetHeartLevel()
        {
            return heartLevel;
        }

        public void Damage(float damageAmount)
        {
            if (damageAmount >= heartLevel)
            {
                heartLevel = 0;
            }
            else
            {
                heartLevel -= damageAmount;
            }
        }

        public void Heal(float healAmount)
        {
            if (heartLevel + healAmount > MAX_HEART_LEVEL)
            {
                heartLevel = MAX_HEART_LEVEL;
            }
            else
            {
                heartLevel += healAmount;
            }
        }
    }
}
