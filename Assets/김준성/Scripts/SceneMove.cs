using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public string initialHallwaySceneName = "초기 복도"; // 초기 복도 씬 이름
    public string prisonSceneName = "개인감옥"; // 개인감옥 씬 이름

    void Start()
    {
    }

    void Update()
    {
        // 플레이어의 x 좌표가 13일 때 초기 복도 씬으로 전환
        if (player.position.x >= 13 && SceneManager.GetActiveScene().name != initialHallwaySceneName)
        {
            SceneManager.LoadScene(initialHallwaySceneName); // 초기 복도 씬으로 전환
        }

        // 현재 씬이 초기 복도이고, 플레이어가 x 좌표 24~25 사이에 있으며 Space를 입력했을 때 개인감옥 씬으로 이동
        if (SceneManager.GetActiveScene().name == initialHallwaySceneName &&
            player.position.x >= 24 && player.position.x <= 25 && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(prisonSceneName); // 개인감옥 씬으로 전환
        }
    }

   
}
