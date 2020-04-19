using System;
using System.Collections.Generic;
using System.Diagnostics;

public class PlayerHeartSystem
{
    public const float MAX_HEART_LEVEL = 1;

    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnRefresh;
    public event EventHandler OnAddHeart;

    private List<Heart> heartList;

    public PlayerHeartSystem(int heartAmount, float playerHealth)
    {
        float it = 0;

        heartList = new List<Heart>();

        for (int i = 0; i < heartAmount; i++)
        {
            if (it < playerHealth)
            {
                Heart heart = new Heart(1);
                heartList.Add(heart);
            }
            else
            {
                Heart heart = new Heart(0);
                heartList.Add(heart);
            }

            it += 1;
        }
    }

    public List<Heart> GetHeartList()
    {
        return heartList;
    }

    public void AddHeart()
    {
        Heart heart = new Heart(1);
        Heart lastHeart = heartList[heartList.Count - 1];

        if (lastHeart.GetHeartLevel() < 1)
        {
            heart.SetHeartLevel(lastHeart.GetHeartLevel());
            lastHeart.SetHeartLevel(1);
            heartList.Add(heart);
        }
        else if (lastHeart.GetHeartLevel() >= 1)
        {
            heartList.Insert(0, heart);
        }

        if (OnAddHeart != null) OnAddHeart(this, EventArgs.Empty); 
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

        public void SetHeartLevel(float heartLevel)
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
