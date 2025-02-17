using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    [SerializeField]
    public Animator animator;
    /** �� �̸� ����
     * MainScreen :     ���� ��
     * PlayScreen :   �÷��� ��
     * LoadingScreen :  �ε� ��
     * 
     */

    public void LoadSceneAsync(string sceneName)
    {
        // �񵿱�� Scene�� �ε�
        // SceneName�� �ش� ��ư�� �Ű������� �Ҵ���.
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
        //// �ִϸ����Ͱ� �����ϴ� ��쿡�� ����.
        //if(animator != null)
        //{
        //    // �ִϸ��̼� A ���� (1ȸ)
        //    animator.Play("AnimationA");
        //    yield return new WaitForSeconds(2f); // �ִϸ��̼� ���� �ð�

        //    // �ִϸ��̼� B ���� (�ݺ�)
        //    animator.Play("AnimationB");
        //}

        // ���� �� �񵿱� �ε� ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // �ڵ� �� ��ȯ ����

        // �ε� ����� Ȯ�� (0.9 �̻��̸� �ε� ���� �Ϸ�)
        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log("�� �ε� �Ϸ�, ���� �� �غ� ��ȣ ��� ��...");
            yield return null;
        }

        //if(animator != null)
        //{
        //    // �ִϸ��̼� C ���� (�ݺ�)
        //    animator.Play("AnimationC");
        //}

        Debug.Log("���� �� �غ� �Ϸ�! �� ��ȯ ����");
        yield return new WaitForSeconds(1f); // �߰� ���� �ð�

        asyncLoad.allowSceneActivation = true; // ���� ������ �̵�
    }
}