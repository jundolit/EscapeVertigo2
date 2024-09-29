using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    public Light2D light2D; // 2D Light 컴포넌트
    public float minIntensity = 0.1f; // 최소 밝기
    public float maxIntensity = 1.0f; // 최대 밝기
    public float flickerMinDuration = 0.1f; // 최소 깜빡임 시간
    public float flickerMaxDuration = 0.3f; // 최대 깜빡임 시간
    public float waitMinTime = 0.1f; // 최소 대기 시간
    public float waitMaxTime = 1.0f; // 최대 대기 시간

    private void Start()
    {
        if (light2D == null)
        {
            light2D = GetComponent<Light2D>(); // Light2D 컴포넌트를 자동으로 찾습니다.
        }
        StartCoroutine(Flicker()); // Flicker 코루틴 시작
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            // 빛을 최소 밝기로 설정
            light2D.intensity = minIntensity;
            float flickerDuration = Random.Range(flickerMinDuration, flickerMaxDuration);
            yield return new WaitForSeconds(flickerDuration);

            // 빛을 최대 밝기로 설정
            light2D.intensity = maxIntensity;
            flickerDuration = Random.Range(flickerMinDuration, flickerMaxDuration);
            yield return new WaitForSeconds(flickerDuration);

            // 다음 깜빡임 전 대기
            float waitTime = Random.Range(waitMinTime, waitMaxTime);
            yield return new WaitForSeconds(waitTime);
        }
    } // Flicker 메서드 종료
} // FlickeringLight 클래스 종료
