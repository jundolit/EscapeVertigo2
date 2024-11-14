using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroEvent : MonoBehaviour
{
    public GameObject dialogueObject; // Dialogue 오브젝트 (SC)
    private Dialogue dialogueScript;  // Dialogue 스크립트 참조


    void Start()
    {
       
        StartCoroutine(EventSequence());

    }

    private IEnumerator EventSequence()
    {
        // Dialogue 오브젝트 활성화
        dialogueObject.SetActive(true);

        // 대화가 끝날 때까지 대기
        yield return new WaitUntil(() => dialogueObject.activeSelf == false);

        SceneManager.LoadScene("PersonalCell"); // 이동할 맵 씬으로 전환

    }
}
