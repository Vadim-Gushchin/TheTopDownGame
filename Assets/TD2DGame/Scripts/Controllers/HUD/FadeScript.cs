using System;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] CinemachineCamera vcam;
    public static FadeScript Instance { get; private set; }

    private CinemachinePositionComposer composer;
    private Vector3 originalDamper;





    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);

        composer = vcam.GetComponentInChildren<CinemachinePositionComposer>();
        originalDamper = composer.Damping;
    }


    async Task Fade(float targetTransparancy)
    {
        float start = canvasGroup.alpha, t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, targetTransparancy, t / fadeDuration);
            await Task.Yield();
        }
        canvasGroup.alpha = targetTransparancy;
    }
    public async Task FadeOut()
    {
        await Fade(1);
        SetDamping(Vector3.zero);
     
    }

    public async Task FadeIn()
    {
        await Fade(0);
        SetDamping(originalDamper);
    }

    void SetDamping(Vector3 damping)
    {
        if (!composer) return;

        composer.Damping = damping;
    }
}
