using UnityEngine;
using UnityEngine.SceneManagement;

public class SubMenuManager : MonoBehaviour
{
    public GameObject submenu; // 서브메뉴 UI 패널
    private bool isPaused = false; // 게임 일시정지 상태를 추적
    public Transform player; // 플레이어 Transform

    void Start()
    {
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
            submenu.SetActive(true);
            Time.timeScale = 0; // 게임 일시정지
            isPaused = true;
            Debug.Log("서브메뉴가 열렸습니다.");
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
            submenu.SetActive(false);
            Time.timeScale = 1; // 게임 재개
            isPaused = false;
            Debug.Log("서브메뉴가 닫혔습니다.");
        }
        else
        {
            Debug.LogWarning("submenu가 할당되지 않았습니다.");
        }
    }

    public void SaveAndExitToMainUI()
    {
        // QUSTID와 플레이어 위치, 현재 씬 이름을 저장
        PlayerPrefs.SetInt("QUSTID", PlayerPrefs.GetInt("QUSTID", 0)); // 현재 QUSTID 값을 저장
        PlayerPrefs.SetFloat("PlayerPosX", player.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", player.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", player.position.z);

        // 현재 씬 이름 저장
        string currentSceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastScene", currentSceneName);

        PlayerPrefs.Save();

        Debug.Log("QUSTID, 플레이어 위치, 현재 씬(" + currentSceneName + ")이 저장되었습니다.");

        // 메인 UI로 씬 전환
        SceneManager.LoadScene("메인UI");
    }
    public void Exit()
    {
        Application.Quit();

    }
    public void Setting() { }

}