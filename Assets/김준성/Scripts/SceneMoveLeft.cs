using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveLeft : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public string targetSceneName = "의무실 복도"; // 이동할 맵 씬 이름
    public string currentSceneName = "초기 복도"; // 현재 씬 이름
    public string beforeSceneName = "개인감옥"; // 이전 씬 이름(개인감옥 이동 제외 안씀) 


    public float targetSceneTransitionX = 13f; // 이동할 맵으로 전환되는 X 값
    public float currentSceneTransitionStartX = 24f; // 현재 씬에서 이동 가능한 시작 X 값
    public float currentSceneTransitionEndX = 25f; // 현재 씬에서 이동 가능한 끝 X 값

    void Update()
    {
        // 플레이어의 x 좌표가 targetSceneTransitionX 이하일 때 이동할 맵 씬으로 전환
        if (player.position.x <= targetSceneTransitionX && SceneManager.GetActiveScene().name != targetSceneName)
        {
            SceneManager.LoadScene(targetSceneName); // 이동할 맵 씬으로 전환
        }

        // 현재 씬이 targetSceneName이고, 플레이어가 currentSceneTransitionStartX ~ currentSceneTransitionEndX 사이에 있으며 Space를 입력했을 때 개인감옥 씬으로 이동
        if (SceneManager.GetActiveScene().name == targetSceneName &&
            player.position.x >= currentSceneTransitionStartX && player.position.x <= currentSceneTransitionEndX && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(beforeSceneName); // 개인감옥 씬으로 전환
        }
    }
}
