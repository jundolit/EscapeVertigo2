using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리용

public class SceneSaveManager : MonoBehaviour
{
    // 플레이어 위치 저장 메서드
    public void SavePlayerPosition(Vector2 position)
    {
        if (MainSaveManager.instance != null)
        {
            string sceneName = SceneManager.GetActiveScene().name; // 현재 씬 이름 가져오기
            MainSaveManager.instance.SavePlayerData(position, sceneName); // 씬 이름과 위치를 함께 저장
            Debug.Log($"씬에서 플레이어 위치가 저장되었습니다: ({position.x}, {position.y}) in scene {sceneName}");
        }
        else
        {
            Debug.LogWarning("MainSaveManager 인스턴스를 찾을 수 없습니다.");
        }
    }

    // 씬 데이터를 저장하는 메서드
    public void SaveSceneData()
    {
        GameObject player = GameObject.FindWithTag("Player"); // "Player" 태그가 붙은 오브젝트 찾기
        if (player != null)
        {
            Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            SavePlayerPosition(playerPosition); // 플레이어 위치 저장
        }
        else
        {
            Debug.LogWarning("플레이어 오브젝트를 찾을 수 없습니다.");
        }
    }
}
