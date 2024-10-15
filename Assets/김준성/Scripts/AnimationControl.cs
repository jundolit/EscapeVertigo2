using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        // 애니메이션을 한 번 실행
        animator.SetTrigger("Play");
    }

    // 애니메이션을 멈추는 메서드
    public void StopAnimation()
    {
        // 애니메이터의 Stop 트리거를 활성화하여 애니메이션을 멈춤
        animator.SetTrigger("Stop");
    }
}
