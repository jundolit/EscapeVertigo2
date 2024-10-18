using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    private Animator anim;

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
        // 저장된 값 초기화
        PlayerPrefs.SetInt("QUSTID", 0); // QUSTID 초기화
        PlayerPrefs.SetFloat("PlayerPosX", 0f); // 플레이어 X 위치 초기화
        PlayerPrefs.SetFloat("PlayerPosY", 0f); // 플레이어 Y 위치 초기화
        PlayerPrefs.SetFloat("PlayerPosZ", 0f); // 플레이어 Z 위치 초기화
        PlayerPrefs.SetString("LastScene", "개인감옥"); // 기본 씬 설정
        PlayerPrefs.Save(); // 변경사항 저장

        // 개인감옥 씬으로 이동
        SceneManager.LoadScene("개인감옥");
        Debug.Log("새 게임 시작: 모든 값 초기화 후 개인감옥 씬으로 전환.");
    }

    public void LoadGame()
    {
        // 저장된 씬 이름과 QUSTID 및 플레이어 위치 불러오기
        string lastScene = PlayerPrefs.GetString("LastScene", "개인감옥"); // 기본값으로 개인감옥 설정
        int questID = PlayerPrefs.GetInt("QUSTID", 0);
        float playerPosX = PlayerPrefs.GetFloat("PlayerPosX", 0f);
        float playerPosY = PlayerPrefs.GetFloat("PlayerPosY", 0f);
        float playerPosZ = PlayerPrefs.GetFloat("PlayerPosZ", 0f);

        // 불러온 값을 디버그 메시지로 확인
        Debug.Log("불러오기: 저장된 씬 이름 = " + lastScene);
        Debug.Log("불러오기: 저장된 QUSTID = " + questID);
        Debug.Log("불러오기: 저장된 플레이어 위치 = (" + playerPosX + ", " + playerPosY + ", " + playerPosZ + ")");

        // 저장된 씬으로 이동
        SceneManager.LoadScene(lastScene);
        Debug.Log("게임 로드: " + lastScene + " 씬으로 전환.");
    }

    public void GameExit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터 모드에서 게임 종료
#endif
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
