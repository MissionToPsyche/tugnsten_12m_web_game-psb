using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Grading
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TimerAccuracy()
    {
        GameObject go = new();
        GameTimer timer = go.AddComponent<GameTimer>();
        timer.startTimer();
        yield return new WaitForSeconds(2);
        timer.stopTimer();

        Assert.AreEqual(2, timer.getTime(), 0.05);
    }
}
