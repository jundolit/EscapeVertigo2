using UnityEngine;
using UnityEngine.UI;

public class BackgroundColorChanger : MonoBehaviour
{
    public Image leftImage; // 왼쪽 이미지
    public Image rightImage; // 오른쪽 이미지
    public Image animationImage; // 애니메이션의 Image 컴포넌트

    void Update()
    {
        // 애니메이션의 맨 좌측 위 모서리 색상 가져오기
        Color topLeftColor = GetTopLeftColor(animationImage);

        // 왼쪽 및 오른쪽 이미지 색상 변경
        leftImage.color = Color.Lerp(leftImage.color, topLeftColor, Time.deltaTime);
        rightImage.color = Color.Lerp(rightImage.color, topLeftColor, Time.deltaTime);
    }

    // 애니메이션 이미지의 맨 좌측 위 모서리 색상 가져오는 메서드
    private Color GetTopLeftColor(Image img)
    {
        // 텍스처를 가져옴
        Rect rect = img.rectTransform.rect;
        Texture2D texture = img.sprite.texture;

        // 좌측 위 모서리의 색상 픽셀을 가져옴
        int x = Mathf.FloorToInt(rect.x);
        int y = Mathf.FloorToInt(rect.y + rect.height); // 위쪽에서 아래쪽으로
        Color color = texture.GetPixel(x, y);

        return color;
    }
}
