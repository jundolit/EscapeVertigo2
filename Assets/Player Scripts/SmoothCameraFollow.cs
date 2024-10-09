using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target; // 따라갈 대상 (주인공)
    public float smoothSpeed = 0.125f; // 카메라 이동 속도
    public Vector3 offset; // 오프셋 (카메라와 주인공 사이의 거리)

    void LateUpdate()
    {
        if (target == null) return; // 대상이 없으면 리턴

        // 원하는 위치 계산 (주인공 위치 + 오프셋)
        Vector3 desiredPosition = target.position + offset;
        // Z 축을 -10으로 고정
        desiredPosition.z = -10f;

        desiredPosition.y = Mathf.Max(desiredPosition.y, 0.37f);


        // 부드러운 이동 처리
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // 카메라 위치 갱신
        transform.position = smoothedPosition;
    }
}
