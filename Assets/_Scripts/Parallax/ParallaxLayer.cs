using UnityEngine;

public class ParallaxLayer : MonoBehaviour {
    [SerializeField] private Vector2 parallaxFactor;

    private float textureUnitSizeX;

    Vector3 targetPos;

    private void Start() {
        Sprite sprite = GetComponentInChildren<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width * transform.lossyScale.x / sprite.pixelsPerUnit;

        targetPos = transform.localPosition;
    }

    [SerializeField] private float speed = 25f;

    private void LateUpdate() {
        float cameraXPos = Camera.main.transform.position.x;
        if (Mathf.Abs(cameraXPos - transform.position.x) >= textureUnitSizeX) {
            float offsetPositionX = (cameraXPos - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraXPos + offsetPositionX, transform.position.y);
        }
    }


    public void Move(Vector3 delta) {
        targetPos -= new Vector3(delta.x * parallaxFactor.x, delta.y * parallaxFactor.y);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed * Time.deltaTime);
    }
}
