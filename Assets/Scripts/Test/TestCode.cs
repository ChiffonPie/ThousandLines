using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using ThousandLines;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_tr;
    private Vector3[] m_Pos;
    // Start is called before the first frame update
    void Start()
    {
        InitializedPos();
        MoveSequence();
    }
    private void InitializedPos()
    {
        this.m_Pos = new Vector3[this.m_tr.Length];
        for (int i = 0; i < this.m_Pos.Length; i++)
        {
            this.m_Pos[i] = this.m_tr[i].position;
        }
    }
    private void MoveSequence()
    {
        Debug.Log(this.name + " : ÀÌµ¿Áß");
        var sequence = DOTween.Sequence();
        sequence.Append(ThousandLinesManager.Instance.transform.DOMove(this.m_Pos[0], 3)).OnComplete(() =>
        {
            Debug.LogError("µµÂø");
        }); ;
    }
}
