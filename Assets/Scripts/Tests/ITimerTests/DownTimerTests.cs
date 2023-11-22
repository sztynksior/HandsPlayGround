using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

public class DownTimerTests
{
    [Test]
    public void GetTime_ReturnsCurrentTime()
    {
        ITimer timerUnderTest = new DownTimer(10f, 10f, false);

        float actualTime = timerUnderTest.GetTime();

        Assert.That(actualTime, Is.EqualTo(10f).Using(FloatEqualityComparer.Instance));
    }

    [Test]
    public void ResetTimer_SetsTimeToInitialValue()
    {
        ITimer timerUnderTest = new DownTimer(10f, 0f, false);

        timerUnderTest.ResetTimer();

        float actualTime = timerUnderTest.GetTime();
        Assert.That(actualTime, Is.EqualTo(10f).Using(FloatEqualityComparer.Instance));
    }

    [Test]
    public void SetInitialTime_ChangeValueOfInitialTimerTime()
    {
        ITimer timerUnderTest = new DownTimer();

        timerUnderTest.SetInitialTime(10f);

        timerUnderTest.ResetTimer();
        float actualTime = timerUnderTest.GetTime();
        Assert.That(actualTime, Is.EqualTo(10f).Using(FloatEqualityComparer.Instance));
    }

    [Test]
    public void StartTimer_TurnsOnTimeRunnig()
    {
        ITimer timerUnderTest = new DownTimer(10f, 10f, false);

        timerUnderTest.StartTimer();

        timerUnderTest.UpdateTime();
        float actualTime = timerUnderTest.GetTime();
        Assert.That(actualTime, Is.Not.EqualTo(10f).Using(FloatEqualityComparer.Instance));
    }



    [Test]
    public void StopTimer_TurnsOfTimeRunning()
    {
        ITimer timerUnderTest = new DownTimer(10f, 10f, true);

        timerUnderTest.StopTimer();

        timerUnderTest.UpdateTime();
        float actualTime = timerUnderTest.GetTime();
        Assert.That(actualTime, Is.EqualTo(10f).Using(FloatEqualityComparer.Instance));
    }

    [Test]
    public void UpdateTime_DecreseTime()
    {
        ITimer timerUnderTest = new DownTimer(10f, 10f, true);

        timerUnderTest.UpdateTime();

        float actualTime = timerUnderTest.GetTime();
        Assert.That(actualTime, Is.EqualTo(10f - Time.deltaTime).Using(FloatEqualityComparer.Instance));
    }
}
