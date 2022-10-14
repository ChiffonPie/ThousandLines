using UnityEngine;
public class MovingBoard : MonoBehaviour
{
    public Material defaultM;
    public float speed;

    Vector2 direction = new Vector2(1, 0);
    Vector2 uvOffset;
    string textureName = "_MainTex";

    private void Awake()
    {
        //var material = new Material(defaultM.shader);
        //defaultM = material;
    }

    public void Update()
    {
        uvOffset += direction * speed * Time.deltaTime;
        defaultM.SetTextureOffset(textureName, uvOffset);
    }
}