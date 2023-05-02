using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField] Animation animTrans;
    public void ReturnMenu()
    {
        StartCoroutine(playAnim());
    }
    
    IEnumerator playAnim()
    {
        animTrans.gameObject.SetActive(true);
        animTrans.Play();
        float timer = animTrans.clip.length;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(0);
    }
}