using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

public class DownTimerTests
{
    private ITimer timerUnderTest;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        GameObject timerObject = new GameObject();
        timerUnderTest = timerObject.AddComponent<DownTimer>();
        timerUnderTest.SetInitialTime(10f);
        timerUnderTest.ResetTimer();
    }
    [UnityTest]
    public IEnumerator DownTimerIsCountingDown()
    {
        float initialTime = timerUnderTest.GetTime();

        timerUnderTest.StartTimer();
        yield return null;

        float actualTime = timerUnderTest.GetTime();
        Assert.That(actualTime, Is.EqualTo(initialTime - Time.deltaTime).Using(FloatEqualityComparer.Instance));
    }
    [TearDown] 
    public void TearDown() 
    {
        timerUnderTest.StopTimer();
        timerUnderTest.ResetTimer();
    }
}
