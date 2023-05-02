using System.Collections;
using UnityEngine;

public class MenuTransition : MonoBehaviour
{
    [SerializeField] Animation anim;

    void Start()
    {
        StartCoroutine(playAnim());
    }

    IEnumerator playAnim()
    {
        float timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        anim.Play();
        timer = anim.clip.length;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        anim.RemoveClip(anim.clip);
        anim.clip = anim.GetClip("ChangeSceneAnim");
        gameObject.SetActive(false);
    }
}