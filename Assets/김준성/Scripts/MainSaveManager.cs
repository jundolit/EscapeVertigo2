using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSaveManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static MainSaveManager instance;

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 오브젝트 유지
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 중복된 오브젝트 파괴
        }
    }

    // 플레이어 위치와 씬 이름을 저장할 메서드
    public void SavePlayerData(Vector2 position, string sceneName)
    {
        PlayerPrefs.SetFloat("MainPlayerPosX", position.x); // X좌표 저장
        PlayerPrefs.SetFloat("MainPlayerPosY", position.y); // Y좌표 저장
        PlayerPrefs.SetString("LastSceneName", sceneName); // 씬 이름 저장
        PlayerPrefs.Save(); // 변경 사항을 저장
        Debug.Log($"메인 UI 플레이어 위치가 저장되었습니다: ({position.x}, {position.y}) in scene {sceneName}");
    }

    // 게임 불러오기 메서드
    public Vector2 LoadPlayerPosition()
    {
        float posX = PlayerPrefs.GetFloat("MainPlayerPosX", 0f); // 기본값은 0
        float posY = PlayerPrefs.GetFloat("MainPlayerPosY", 0f); // 기본값은 0
        Vector2 position = new Vector2(posX, posY);

        // 씬 이름 불러오기
        string lastSceneName = PlayerPrefs.GetString("LastSceneName", ""); // 기본값은 빈 문자열

        // 유효성 검사
        if (position.x < -100 || position.x > 100 || position.y < -100 || position.y > 100)
        {
            Debug.LogWarning("불러온 플레이어 위치가 유효하지 않습니다. 기본 위치로 초기화합니다.");
            position = Vector2.zero; // 기본 위치
        }

        Debug.Log($"불러온 메인 UI 플레이어 위치: {position} in scene {lastSceneName}");
        return position; // 플레이어 위치 반환
    }

    // 씬 전환 메서드
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // 지정된 씬으로 전환
    }

    // 플레이어 위치를 설정하는 메서드 (씬 로드 후 호출)
    public void SetPlayerPosition(GameObject player)
    {
        Vector2 position = LoadPlayerPosition();
        player.transform.position = position; // 플레이어의 위치 설정
    }

    // 특정 키 삭제
    public void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
        Debug.Log($"키 '{key}' 삭제되었습니다.");
    }

    // 모든 데이터 삭제
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("모든 데이터가 삭제되었습니다.");
    }
}
