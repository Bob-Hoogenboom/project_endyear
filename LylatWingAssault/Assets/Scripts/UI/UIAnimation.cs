using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiAnimation : MonoBehaviour
{
    public Image m_Image;

    public Sprite[] sprites;
    public float speed = .02f;
    public bool playOnStart;

    private int spriteIndex;
    Coroutine animCoroutine;

    bool IsDone;


    private void Start()
    {
        if (playOnStart)
        {
            Func_PlayUIAnim();
        }
    }

    public void Func_PlayUIAnim()
    {
        IsDone = false;
        StartCoroutine(Func_PlayAnimUI());
    }

    public void Func_StopUIAnim()
    {
        IsDone = true;
        StopCoroutine(Func_PlayAnimUI());
    }
    IEnumerator Func_PlayAnimUI()
    {
        yield return new WaitForSeconds(speed);
        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
            IsDone = true;
        }
        m_Image.sprite = sprites[spriteIndex];
        spriteIndex += 1;
        if (IsDone == false)
            animCoroutine = StartCoroutine(Func_PlayAnimUI());
        else if(IsDone == true)
        {
            yield return null;
        }
    }
}