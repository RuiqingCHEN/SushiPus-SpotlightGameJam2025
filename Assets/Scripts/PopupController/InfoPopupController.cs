using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPopupController : MonoBehaviour
{
    public static  InfoPopupController Instance { get; private set; }

    public GameObject popupPrefab;
    public int maxPopups = 3;
    public float popupDuration = 60f;

    public Sprite[] infoImages;

    private readonly Queue<GameObject> activePopups = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ShowInfo(0);
        ShowInfo(1);
    }

    public void ShowInfo(int imageIndex)
    {
        GameObject newPopup = Instantiate(popupPrefab, transform);

        Image itemImage = newPopup.GetComponent<Image>();
        if(itemImage)
        {
            itemImage.sprite = infoImages[imageIndex];
        }

        Button button = newPopup.GetComponent<Button>();
        button.onClick.AddListener(() => OnPopupClicked(newPopup));

        activePopups.Enqueue(newPopup);
        if (activePopups.Count > maxPopups)
        {
            Destroy(activePopups.Dequeue());
        }

        // Fade out and destroy
        StartCoroutine(FadeOutAndDestroy(newPopup));
    }
    
    private void OnPopupClicked(GameObject popup)
    {
        if(popup != null)
        {
            Destroy(popup);
        }
    }

    private IEnumerator FadeOutAndDestroy(GameObject popup)
    {
        yield return new WaitForSeconds(popupDuration);
        if(popup == null) yield break;

        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        for(float timePassed = 0f; timePassed < 1f; timePassed += Time.deltaTime)
        {
            if(popup == null) yield break;
            canvasGroup.alpha = 1f - timePassed;
            yield return null;
        }

        Destroy(popup);
    }
}
