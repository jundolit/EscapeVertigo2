using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    private Animator anim;
    public GameObject playerPrefab; // 플레이어 프리팹을 드래그하여 할당
    private Vector2 playerPosition; // 플레이어 위치 저장 변수

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 필요하다면 업데이트 로직을 추가할 수 있습니다.
    }

    public void GameStart()
    {
        // 씬 전환 전에 플레이어의 위치를 저장합니다.
        GameLoad(); // 게임 시작 시 위치 불러오기
        SceneManager.LoadScene("개인감옥"); // 개인감옥 씬으로 전환
        Debug.Log("Play");
    }

    public void GameExit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터 모드에서 게임 종료
#endif
    }

    public void GameLoad()
    {
        // SaveManager 인스턴스를 찾기
        MainSaveManager saveManager = FindObjectOfType<MainSaveManager>();

        // PlayerPrefs에서 저장된 플레이어 위치를 불러오기
        Vector2 playerPosition = saveManager.LoadPlayerPosition();

        // 저장된 씬 이름 가져오기
        string lastSceneName = PlayerPrefs.GetString("LastSceneName", "");

        if (!string.IsNullOrEmpty(lastSceneName))
        {
            // 해당 씬으로 전환
            saveManager.LoadScene(lastSceneName);

            // 플레이어 오브젝트 생성 (해당 씬에서 생성)
            GameObject player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
            Debug.Log("플레이어 위치를 불러왔습니다: " + playerPosition);
        }
        else
        {
            Debug.LogWarning("저장된 씬이 없습니다.");
        }
    }

    public void GameSetting()
    {
        // 게임 설정 UI를 여는 로직을 여기에 추가할 수 있습니다.
        Debug.Log("게임 설정을 열었습니다.");
    }

    public void Close()
    {
        StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay()
    {
        anim.SetTrigger("close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        anim.ResetTrigger("close");
    }
}
 