using UnityEngine;

public class SelectLVL : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text textLVL;
    [SerializeField] int sceneIndex;
    public void SelectLVLIndex(int i)
    {
        sceneIndex = i;
        textLVL.text = (sceneIndex + 1).ToString();
    }
}
