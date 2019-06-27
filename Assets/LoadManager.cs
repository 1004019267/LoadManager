/**
 *Copyright(C) 2019 by #COMPANY#
 *All rights reserved.
 *FileName:     #SCRIPTFULLNAME#
 *Author:       #AUTHOR#
 *Version:      #VERSION#
 *UnityVersion:#UNITYVERSION#
 *Date:         #DATE#
 *Description:   
 *History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LoadManager : Singleton<LoadManager>
{
    //加载场景的名字
    const string LoadSceneName = "LoadScene";
    Stack<string> sence = new Stack<string>();
    LoadScene loadScene = new LoadScene();
    //初始化添加第一个场景
    public void Init()
    {
        string startSceneName = SceneManager.GetActiveScene().name;
        if (!sence.Contains(startSceneName))
        {
            sence.Push(startSceneName);
        }

    }
    /// <summary>
    /// 加载界面 回调可以再加载场景释放静态资源
    /// 或者加载资源（不过这个没什么太大必要，下个场景脚本start也会在Load进度条之内）
    /// </summary>
    /// <param name="name"></param>
    public void Load(string name, Action callBack = null)
    {
        sence.Push(name);
        loadScene.callBack = callBack;
        loadScene.nowScene = GetNowSence();
        SceneManager.LoadScene(LoadSceneName);
    }

    //得到当前场景
    public string GetNowSence()
    {
        return sence.Peek();
    }

    /// <summary>
    /// 返回
    /// </summary>
    public void Back(Action callBack = null)
    {
        //如果就没有场景就退出程序(因为默认开打unity界面不会储存)
        if (sence.Count == 1)
            Application.Quit();
        else if (sence.Count > 1)
        {
            sence.Pop();
            loadScene.callBack = callBack;
            loadScene.nowScene = GetNowSence();
            SceneManager.LoadScene(LoadSceneName);
        }
    }

    //清空栈
    public void Clear()
    {
        sence.Clear();
    }

    //返回加载逻辑类
    public LoadScene LoadScene()
    {
        return loadScene;
    }
}

public class LoadScene
{
    public Action callBack;
    public string nowScene;
    //进度
    float displayProgress = 0;
    //加载场景
    public IEnumerator StartLoading()
    {
        yield return new WaitForSeconds(0.01f);

        displayProgress = 0;
        float toProgress = 0;

        AsyncOperation op = SceneManager.LoadSceneAsync(nowScene);
        //在允许之前不要让场景激活
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            //这里是用一个变量储存进度慢慢加载方式瞬间加载满 平缓过渡
            toProgress = op.progress;
            while (displayProgress < toProgress)
            {
                displayProgress += 0.01f;
                yield return new WaitForEndOfFrame();//等待这一帧调用完
            }
        }
        callBack?.Invoke();
        //下面为我们为了防止加载过快 会在0.9停顿一下，不然可能加载界面一闪而过
        toProgress = 1;
        while (displayProgress < toProgress)
        {
            displayProgress += 0.01f;
            yield return new WaitForEndOfFrame();
        }
        callBack = null;
        op.allowSceneActivation = true;
    }
    //获得进度条
    public float GetProgress()
    {
        return displayProgress;
    }
}
