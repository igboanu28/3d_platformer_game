using System;

namespace Platformer
{
    public abstract class Timer
    {
        protected float initialTime;
        protected float Time { get; set; }
        public bool IsRunning { get; protected set; }

        public float Progress => initialTime > 0 ? Time / initialTime : 0; // Avoid divide by zero

        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };

        protected Timer(float value)
        {
            initialTime = value;
            IsRunning = false;
        }

        public virtual void Start()
        {
            Time = initialTime;
            if (!IsRunning)
            {
                IsRunning = true;
                OnTimerStart.Invoke();
            }
        }

        public virtual void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                OnTimerStop.Invoke();
            }
        }

        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;

        public abstract void Tick(float deltaTime);

        public float RemainingSeconds => Time;
        public float InitialDuration => initialTime;
    }

    // countdown/cooldown timer

    public class CountdownTimer : Timer
    {
        public CountdownTimer(float value) : base(value) { }
        public override void Tick(float deltaTime)
        {
            if (IsRunning && Time > 0)
            {
                Time -= deltaTime;
            }

            if (IsRunning && Time <= 0)
            {
                Stop();
            }
        }

        public bool IsFinished => Time <= 0;

        public void Reset()
        {
            Time = initialTime;
            IsRunning = false;
        }

        public void Reset(float newTime)
        {
            initialTime = newTime;
            Reset();
        }

        // IMPORTANT MODIFICATION: Ensure Time is set to 0 when explicitly stopped
        public override void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                Time = 0; // Explicitly set time to 0 to signal completion
                OnTimerStop.Invoke();
            }
        }
    }

    // stopwatch timer

    public class StopwatchTimer : Timer
    {
        public StopwatchTimer() : base(0) { }

        public override void Tick(float deltaTime)
        {
            if (IsRunning)
            {
                Time += deltaTime;
            }
        }

        public void Reset() => Time = 0;

        public float GetTime() => Time;
    }
}