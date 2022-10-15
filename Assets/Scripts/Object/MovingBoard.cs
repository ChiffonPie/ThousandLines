﻿using UnityEngine;
public class MovingBoard : MonoBehaviour
{
    public SpriteRenderer m_LoopBoard;
    public Material defaultM;
    public float speed;

    Vector2 direction = new Vector2(-1, 0);
    Vector2 uvOffset;
    string textureName = "_MainTex";

    private void Awake()
    {
        this.MaterialInstancing();
    }

    private void Update()
    {
        if (speed != 0)
        {
            this.uvOffset += direction * speed * Time.deltaTime;
            this.defaultM.SetTextureOffset(textureName, uvOffset);
        }
    }

    private void MaterialInstancing()
    {
        this.defaultM = Instantiate(defaultM);
        this.m_LoopBoard.material = defaultM;
        this.defaultM.color = Color.clear;
    }
}