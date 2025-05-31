using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Helper.Tween;

[RequireComponent(typeof(Button))]
public class TweenHelperButton : MonoBehaviour
{
    [SerializeField] [Range(-0.3f, 0)]float size = -0.1f;

    public UnityEvent OnClick;
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        GetComponent<Button>().onClick.AddListener(Click);
    }

    void Click()
    {
        ButtonTween tween = new ButtonTween(
            () => rectTransform.localScale,
            size => rectTransform.localScale = size,
            0.25f,
            size
        );
    
        tween.OnComplete(() =>
        {
            // Reset scale to original size
            rectTransform.localScale = Vector3.one;
            OnClick?.Invoke();
        });
        tween.Start();
    }

}
