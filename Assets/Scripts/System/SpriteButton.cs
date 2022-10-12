using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpriteButton : Button
{
	private Image m_Normal;
	public TextMeshProUGUI m_Text;

    protected override void Awake()
    {
        this.InitializeNormalSprite();
    }

    private void InitializeNormalSprite()
    {
        if (this.GetComponent<Image>() != null)
            m_Normal = this.GetComponent<Image>();
    }

    public string Label
	{
		set { this.m_Text.text = value; }
	}

	public Sprite NormalSprite
    {
		set {
            if (m_Normal == null) return;
            m_Normal.sprite = value; 
        }
    }
}
