using UnityEngine;
using System;
using System.Collections.Generic;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();

    // 큐에 메인 스레드에서 실행할 액션을 추가하는 메서드
    public static void Enqueue(Action action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }

    // 매 프레임마다 큐에서 액션을 꺼내서 실행
    void Update()
    {
        while (_executionQueue.Count > 0)
        {
            _executionQueue.Dequeue().Invoke();
        }
    }
}