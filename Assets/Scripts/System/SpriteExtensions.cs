using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteExtensions
{
    public static List<SpriteRenderer> GetSpriteList(GameObject baseObject)
    {
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

        if (baseObject.GetComponent<SpriteRenderer>() != null)
            spriteRenderers.Add(baseObject.GetComponent<SpriteRenderer>());

        GetSprite(spriteRenderers, baseObject);
        return spriteRenderers;
    }

    private static void GetSprite(List<SpriteRenderer> spriteRenderers, GameObject baseObject)
    {
        for (int i = 0; i < baseObject.transform.childCount; i++)
        {
            if (baseObject.transform.GetChild(i).GetComponent<SpriteRenderer>() != null)
            {
                spriteRenderers.Add(baseObject.transform.GetChild(i).GetComponent<SpriteRenderer>());
                GetSprite(spriteRenderers, baseObject.transform.GetChild(i).gameObject);
            }
        }
    }

    public static Sequence SetSpritesColor(List<SpriteRenderer> spriteRenderers, Color color, float duration)
    {
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            sequence.Join(spriteRenderers[i].DOColor(color, duration));
        }
        return sequence;
    }

    public static void HideSpriteObject(List<SpriteRenderer> spriteRenderers)
    {
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            spriteRenderers[i].color = Color.clear;
        }
    }
}
