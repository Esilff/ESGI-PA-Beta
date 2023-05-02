using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    [SerializeField] int sceneIndex;
    [SerializeField] Animation anim;

    public void ChangeScene()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        anim.gameObject.SetActive(true);
        float timer = anim.clip.length;
        anim.Play();
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}