using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using AdvancedInspector;

[RequireComponent(typeof(PlayableDirector))]
public class SingletonLoopSeek : SingletonLoopSeekBase<SingletonLoopSeek>
{
    PlayableDirector playableDirector;

    [SerializeField]
    Dictionary<int, double> dicLabelTime = new Dictionary<int, double>();

    bool canSetTime = true;

    void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    void Start() { }

    public void ClearDic()
    {
        dicLabelTime.Clear();
    }

    public void AddLabelTime(int label, double time)
    {
        dicLabelTime.Add(label, time);
    }

    public void ErrorCheck(int label)
    {
        if (!dicLabelTime.ContainsKey(label))
            Debug.LogError("dicLabelTime does not contain label:" + label.ToString() + " !", transform);
    }

    // TimelineのLoopSeekBehaviourからの呼び出しの場合trueを指定する。
    // それ以外の場合は
    public void SetTime(int label, bool fromTimeline = false)
    {
        if (fromTimeline)
        {
            if (canSetTime)
            {
                ErrorCheck(label);
                playableDirector.time = dicLabelTime[label];
            }
        }
        else
        {
            StartCoroutine("SetTimeCoroutine", label);
        }
    }

    IEnumerator SetTimeCoroutine(int _label)
    {
        ErrorCheck(_label);

        canSetTime = false;

        playableDirector.time = dicLabelTime[_label];

        yield return new WaitForSeconds(0.1f);

        canSetTime = true;
    }

    [Inspect(0), Title(FontStyle.Bold, "Test"), Spacing(Before = 3)]
    int testLabel;

    [Inspect(1)]
    void Test()
    {
        StartCoroutine("SetTimeCoroutine", testLabel);
    }
}
