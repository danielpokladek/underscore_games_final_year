using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARG_SpriteSelector : MonoBehaviour
{
    // Using class just to organise the sprites a little, to make inspector neater to look at.
    // Ideally later on, this should be changed to something more managable.

    [System.Serializable]
    public class SpriteList
    {
        public Sprite spU, spD, spR, spL,
            spUD, spRL, spUR, spUL, spDR, spDL,
            spULD, spRUL, spDRU, spLDR, spUDRL;
    }

    [System.Serializable]
    public class RoomColors
    {
        public Color normalRoom;
        public Color entryRoom;
        public Color bossRoom;
        public Color shopRoom;
    }

    [Tooltip("This is where the sprites should be assigned for the level generator to use." +
        "This should only be adjusted on the empty prefab for the rooms," +
        "as this will ensure the changes are saved for all rooms that use the prefab.")]
    public SpriteList m_SpriteList;

    [Tooltip("This is where the colours for the rooms are assigned. Once more rooms are added," +
        "more options will be available here. These can be adjusted on the go too.")]
    public RoomColors m_RoomColors;

    public bool up, down, left, right;

    public int type;                        // Room Type:
                                            // 0: Normal Room
                                            // 1: Entry Room
                                            // 2: Final Room
                                            // 3: Shop Room

    private Color mainColor;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainColor = m_RoomColors.normalRoom;
        PickSprite();
        PickColor();
    }

    private void PickSprite()
    {
        if (up)
        {
            if (down)
            {
                if (right)
                {
                    if (left)
                    {
                        spriteRenderer.sprite = m_SpriteList.spUDRL;
                    }
                    else
                    {
                        spriteRenderer.sprite = m_SpriteList.spDRU;
                    }
                }
                else if (left)
                {
                    spriteRenderer.sprite = m_SpriteList.spULD;
                }
                else
                {
                    spriteRenderer.sprite = m_SpriteList.spUD;
                }
            }
            else
            {
                if (right)
                {
                    if (left)
                    {
                        spriteRenderer.sprite = m_SpriteList.spRUL;
                    }
                    else
                    {
                        spriteRenderer.sprite = m_SpriteList.spUR;
                    }
                }
                else if (left)
                {
                    spriteRenderer.sprite = m_SpriteList.spUL;
                }
                else
                {
                    spriteRenderer.sprite = m_SpriteList.spU;
                }
            }

            return;
        }

        if (down)
        {
            if (right)
            {
                if (left)
                {
                    spriteRenderer.sprite = m_SpriteList.spLDR;
                }
                else
                {
                    spriteRenderer.sprite = m_SpriteList.spDR;
                }
            }
            else if (left)
            {
                spriteRenderer.sprite = m_SpriteList.spDL;
            }
            else
            {
                spriteRenderer.sprite = m_SpriteList.spD;
            }

            return;
        }
        if (right)
        {
            if (left)
            {
                spriteRenderer.sprite = m_SpriteList.spRL;
            }
            else
            {
                spriteRenderer.sprite = m_SpriteList.spR;
            }
        }
        else
        {
            spriteRenderer.sprite = m_SpriteList.spL;
        }
    }

    private void PickColor()
    {
        switch (type)
        {
            case 0:
                mainColor = m_RoomColors.normalRoom;
                break;

            case 1:
                mainColor = m_RoomColors.entryRoom;
                break;

            case 2:
                mainColor = m_RoomColors.bossRoom;
                break;

            case 3:
                mainColor = m_RoomColors.shopRoom;
                break;
        }

        spriteRenderer.color = mainColor;
    }
}
