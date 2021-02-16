using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class CardsKeeper : MonoBehaviour
{
    [SerializeField]
    private GameObject _loading;
    [SerializeField]
    private GameObject _changeButton;
    [SerializeField]
    private GameObject _generateCard;

    private string _path = "https://picsum.photos/id/";

    public CardStruct[] CardsVariants;

    public List<Sprite> Sprites;

    private IEnumerator Start()
    {
        if (CheckForInternetConnection())
        {
            string url;
            Sprite sprite;

            for (int i = 0; i < CardsVariants.Length; i++)
            {
                url = _path + CardsVariants[i].ImageId;
                WWW www = new WWW(url);
                yield return www;
                Texture2D texture;
                texture = www.texture;
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                Sprites.Add(sprite);
            }
            _loading.SetActive(false);
            _changeButton.SetActive(true);
            _generateCard.SetActive(true);
            GameObject.FindWithTag("Hand").GetComponent<Hand>().StartGenerate();
        }
        else
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    public bool CheckForInternetConnection()
    {
        try
        {
            using (var client = new WebClient())
            using (var stream = client.OpenRead("http://www.google.com"))
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
}
