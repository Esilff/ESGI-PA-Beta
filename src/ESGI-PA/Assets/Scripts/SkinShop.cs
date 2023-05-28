using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class SkinShop : MonoBehaviour
{
    public RectTransform contentParent; // Parent du contenu de la galerie
    public GameObject skinItemPrefab; // Préfabriqué de l'élément d'interface utilisateur pour chaque skin

    private List<SkinData> skinDataList; // Liste des données des skins

    void Start()
    {
        StartCoroutine(GetSkinDataFromAPI());
    }

    IEnumerator GetSkinDataFromAPI()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/skins"))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError && !www.isHttpError)
            {
                string jsonResponse = www.downloadHandler.text;

                // Convertir la réponse JSON en une liste de SkinData
                SkinDataList skinDataListWrapper = JsonUtility.FromJson<SkinDataList>(jsonResponse);
                skinDataList = skinDataListWrapper.skins;

                // Génère les éléments d'interface utilisateur pour chaque skin
                foreach (SkinData skinData in skinDataList)
                {
                    GameObject skinItemGO = Instantiate(skinItemPrefab, contentParent);
                    SkinItemUI skinItemUI = skinItemGO.GetComponent<SkinItemUI>();
                    skinItemUI.Initialize(skinData);
                }
            }
            else
            {
                Debug.LogError("Erreur lors de la requête API : " + www.error);
            }
        }
    }
}

[System.Serializable]
public class SkinDataList
{
    public List<SkinData> skins;
}

[System.Serializable]
public class SkinData
{
    public int id;
    public string name;
    public int value;
    public string data_added;
    public int character;
    public string imagepath;
    public string description;
    public int price;
}

public class SkinItemUI : MonoBehaviour
{
    public Image skinImage; // Image du skin
    public Text skinNameText; // Texte du nom du skin
    public Text skinPriceText; // Texte du prix du skin

    public void Initialize(SkinData skinData)
    {
        StartCoroutine(LoadSkinImage(skinData.imagepath));

        skinNameText.text = skinData.name;
        skinPriceText.text = "Prix : " + skinData.price.ToString();
    }

    IEnumerator LoadSkinImage(string imagePath)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imagePath))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError && !www.isHttpError)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);

                // Crée un sprite à partir de la texture téléchargée
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                // Applique le sprite à l'image du skin
                skinImage.sprite = sprite;
            }
            else
            {
                Debug.LogError("Erreur lors du chargement de l'image du skin : " + www.error);
            }
        }
    }
}
