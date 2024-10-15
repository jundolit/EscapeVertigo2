using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리용

public class SubMenuManager : MonoBehaviour
{
    public GameObject submenu; // 서브메뉴 UI 패널
    private bool isPaused = false; // 게임 일시정지 상태를 추적

    void Start()
    {
        // 서브메뉴를 숨김
        if (submenu != null)
        {
            submenu.SetActive(false);
        }
        else
        {
            Debug.LogWarning("submenu가 할당되지 않았습니다.");
        }
    }

    void Update()
    {
        // ESC 키 입력 시 서브메뉴 열기/닫기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                CloseSubMenu();
            }
            else
            {
                OpenSubMenu();
            }
        }
    }

    public void OpenSubMenu()
    {
        if (submenu != null)
        {
            // 서브메뉴 열기
            submenu.SetActive(true);
            Time.timeScale = 0; // 게임 일시정지
            isPaused = true; // 상태 업데이트
            Debug.Log("서브메뉴가 열렸습니다."); // 디버그 메시지 추가
        }
        else
        {
            Debug.LogWarning("submenu가 할당되지 않았습니다.");
        }
    }

    public void CloseSubMenu()
    {
        if (submenu != null)
        {
            // 서브메뉴 닫기
            submenu.SetActive(false);
            Time.timeScale = 1; // 게임 재개
            isPaused = false; // 상태 업데이트
            Debug.Log("서브메뉴가 닫혔습니다."); // 디버그 메시지 추가
        }
        else
        {
            Debug.LogWarning("submenu가 할당되지 않았습니다.");
        }
    }

    public void Setting() { }

    public void ResumeGame()
    {
        CloseSubMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터 모드에서 게임 종료
#endif
    }

    // 저장 및 메인 UI로 돌아가기 메서드
    public void SaveAndExitToMainUI()
    {
        // 씬의 SceneSaveManager 인스턴스를 찾기
        SceneSaveManager sceneSaveManager = FindObjectOfType<SceneSaveManager>();

        // SceneSaveManager가 존재할 경우 데이터를 저장
        if (sceneSaveManager != null)
        {
            // 씬 데이터를 저장 (플레이어 위치 포함)
            sceneSaveManager.SaveSceneData();
        }
        else
        {
            Debug.LogWarning("SceneSaveManager를 찾을 수 없습니다.");
        }

        // 메인 UI로 씬 전환
        UnityEngine.SceneManagement.SceneManager.LoadScene("메인UI");
    }

}
