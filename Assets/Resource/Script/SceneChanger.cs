using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    public void LoadSceneAsync(string sceneName)
    {
        // �񵿱�� Scene�� �ε�
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    /* LoadSceneCoroutine
     * �ڷ�ƾ���� ���� �ҷ����� �Լ�.
     * 
     * ���� �θ��鼭, ���� �����, ���� ������ ���� �ϱ� ���ؼ�.
     * ��, ��Ƽ������ ���� ȿ���� �ֱ� ���ؼ� �̿Ͱ��� �Լ��� ����ߴ�.
     **/
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // �񵿱�� ���� �ҷ��� ��ü ����.
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // �ε��� �Ϸ�� ������ ��ٸ�
        while (!operation.isDone)
        {
            // ����� Ȯ�� ���� (0.0 ~ 1.0)
            Debug.Log("Loading progress: " + operation.progress);

            yield return null; // ���� �����ӱ��� ��ٸ�
        }
    }
}