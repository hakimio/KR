using UnityEngine;
using System.Collections;

public class ArenaLoader: MonoBehaviour
{
    GUIText loadingText;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            Messenger.RemoveAllListeners();
            Application.LoadLevelAsync("Arena");
            loadingText = GameObject.Find("Loading Text").guiText;
            loadingText.material.color = new Color(0, 56f / 255f, 168f / 255f);
            loadingText.enabled = true;
            StartCoroutine(animateLoadingText());
        }
    }

    IEnumerator animateLoadingText()
    {
        yield return new WaitForSeconds(0.75f);
        changeText();
        StartCoroutine(animateLoadingText());
    }

    void changeText()
    {
        if (loadingText.text.Length < 10)
            loadingText.text = loadingText.text + ".";
        else
            loadingText.text = loadingText.text.Replace("...", "");
    }
}
