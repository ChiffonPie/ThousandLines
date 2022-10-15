using UnityEngine;
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
        uvOffset += direction * speed * Time.deltaTime;
        defaultM.SetTextureOffset(textureName, uvOffset);
    }

    private void MaterialInstancing()
    {
        defaultM = Instantiate(defaultM);
        m_LoopBoard.material = defaultM;
    }
}