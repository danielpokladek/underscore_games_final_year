using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHearts : MonoBehaviour
{
    [SerializeField] private Transform heartContainer;

    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartHalf;
    [SerializeField] private Sprite heartEmpty;

    private List<HeartImage> heartImageList;
    [HideInInspector] public PlayerHeartSystem heartSystem;

    private void Awake()
    {
        heartImageList = new List<HeartImage>();
    }

    public void SetHeartSystem(PlayerHeartSystem heartSystem)
    {
        this.heartSystem = heartSystem;

        List<PlayerHeartSystem.Heart> heartList = heartSystem.GetHeartList();

        for (int i = 0; i < heartList.Count; i++)
        {
            PlayerHeartSystem.Heart heart = heartList[i];
            CreateHeartImage().SetHeartLevel(heart.GetHeartLevel());
        }

        heartSystem.OnDamaged += HeartSystem_OnDamaged;
        heartSystem.OnHealed += HeartSystem_OnHealed;
        heartSystem.OnRefresh += HeartSystem_OnRefresh;
        heartSystem.OnAddHeart += HeartSystem_OnAddHeart;
    }

    private void HeartSystem_OnAddHeart(object sender, System.EventArgs e)
    {
        CreateHeartImage().SetHeartLevel(1);
        RefreshAllHearts();
    }

    private void HeartSystem_OnRefresh(object sender, System.EventArgs e)
    {
        RefreshAllHearts();
    }

    private void HeartSystem_OnDamaged(object sender, System.EventArgs e)
    {
        RefreshAllHearts();
    }

    private void HeartSystem_OnHealed(object sender, System.EventArgs e)
    {
        RefreshAllHearts();
    }

    public void RefreshAllHearts()
    {
        List<PlayerHeartSystem.Heart> heartList = heartSystem.GetHeartList();

        for (int i = 0; i < heartImageList.Count; i++)
        {
            HeartImage heartImage = heartImageList[i];
            PlayerHeartSystem.Heart heart = heartList[i];
            heartImage.SetHeartLevel(heart.GetHeartLevel());
        }
    }

    private HeartImage CreateHeartImage()
    {
        GameObject heartObject = new GameObject("heart", typeof(Image));

        heartObject.transform.SetParent(heartContainer);

        heartObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        heartObject.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 60);
        heartObject.transform.SetParent(heartContainer);

        Image heartImageUI = heartObject.GetComponent<Image>();
        heartImageUI.sprite = heartFull;

        HeartImage heartImage = new HeartImage(this, heartImageUI);
        heartImageList.Add(heartImage);

        return heartImage;
    }

    public class HeartImage
    {
        private Image heartImage;
        private PlayerHearts playerHearts;

        public HeartImage(PlayerHearts playerHearts, Image heartImage)
        {
            this.heartImage = heartImage;
            this.playerHearts = playerHearts;
        }

        public void SetHeartLevel(float heartLevel)
        {
            switch (heartLevel)
            {
                case 0.0f:
                    heartImage.sprite = playerHearts.heartEmpty;
                    break;

                case 0.5f:
                    heartImage.sprite = playerHearts.heartHalf;
                    break;

                case 1.0f:
                    heartImage.sprite = playerHearts.heartFull;
                    break;
            }
        }
    }
}
