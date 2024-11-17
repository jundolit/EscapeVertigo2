using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{
    public GameObject messagePanel; // 메시지를 표시할 패널
    public Text messageText; // 메시지 텍스트
    public string targetSceneName = "의무실"; // 이동할 기본 씬 이름
    private GameObject scanObject; // 현재 상호작용 가능한 오브젝트
    public bool isAction = false; // 패널 활성화 여부

    public void Action(GameObject scanObj)
    {
        ObjData objData = scanObj.GetComponent<ObjData>(); // ObjData 컴포넌트 가져오기

        if (isAction)
        {
            isAction = false;
            messagePanel.SetActive(false);
        }
        else
        {
            isAction = true;
            scanObject = scanObj;

            if (objData != null && objData.id == 500) // ID가 500인 경우
            {
                messageText.text = "문이 열렸습니다.";
                StartCoroutine(OpenDoorAndChangeScene("의무실"));
            }
            else if (objData != null && objData.id == 501) // ID가 501인 경우
            {
                messageText.text = "문이 열렸습니다.";
                StartCoroutine(OpenDoorAndChangeScene("기믹"));
            }
            else
            {
                messageText.text = "이것의 이름은 " + scanObj.name + "이라고 한다.";
            }

            messagePanel.SetActive(isAction);
        }
    }

    private IEnumerator OpenDoorAndChangeScene(string sceneName)
    {
        yield return new WaitForSeconds(2f); // 2초 대기
        messagePanel.SetActive(false); // 패널 비활성화
        isAction = false; // 상호작용 플래그 초기화
        SceneManager.LoadScene(sceneName); // 지정된 씬으로 전환
        Debug.Log("씬 전환 완료: " + sceneName);
    }
}
