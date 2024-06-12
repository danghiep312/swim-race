using System.Collections;
using System.Collections.Generic;
using Framework.DesignPattern.Singleton;
using UnityEngine;

public class BackButtonController : Singleton<BackButtonController>
{
    public bool canBack = false;
    private Stack<IBack> stack = new Stack<IBack>();

    [SerializeField] private float timeBetweenBack = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        canBack = true;
    }
    public void UnRegister(IBack panel)
    {
        if (stack.Count > 0 && stack.Peek() == panel)
        {
            stack.Pop();
        }
    }

    public void Register(IBack panel, bool setBackManually = true)
    {
        if (!stack.Contains(panel))
        {
            Common.Log("REG: " + panel.ToString());
            stack.Push(panel);
            canBack = false;
            if (!setBackManually)
            {
                Util.Delay(1f, SetBack);
            }
        }
    }

    void SetBack()
    {
        canBack = true;
    }

    public void Back()
    {
        if (stack.Count > 0)
        {
            stack.Pop().OnBack();
        }
    }

    float timer = 0;

    private void Update()
    {
        if (canBack && timer <= 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
            timer = timeBetweenBack;
        }
        else if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
}


public interface IBack
{
    void OnBack(bool adBreak = false);
}