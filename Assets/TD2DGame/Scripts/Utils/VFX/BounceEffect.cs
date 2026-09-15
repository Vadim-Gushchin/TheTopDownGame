using System.Collections;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    [SerializeField] float bounceHight = 0.3f;
    [SerializeField] float bounceDuration = 1f;
    [SerializeField] int bounceCount = 3;

    public void StartBounce()
    {
        StartCoroutine(BounceHandler());
    }
    private IEnumerator BounceHandler()
    {
        Vector3 startPositon = transform.position;
        float localHeight = bounceHight;
        float localDuration = bounceDuration;

        for(int i= 0; i < bounceCount; i++)
        {
            yield return Bounce(startPositon, localHeight, localDuration / 2);
            localHeight *= 0.5f;
            localDuration *= 0.5f;
        }
        transform.position = startPositon;
    }

    private IEnumerator Bounce(Vector3 start, float height, float duartion)
    {
        Vector3 peak = start + Vector3.up * height;
        float elasped = 0f;

        while (elasped < duartion)
        {
            transform.position = Vector3.Lerp(start, peak, elasped / duartion);
            elasped += Time.deltaTime*2;
            yield return null;
        }
        elasped = 0f;
        while (elasped < duartion)
        {
            transform.position = Vector3.Lerp(peak, start, elasped / duartion);
            elasped += Time.deltaTime * 2;
            yield return null;
        }
    }

}
