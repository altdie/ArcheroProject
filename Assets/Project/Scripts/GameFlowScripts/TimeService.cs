using System;

namespace Project.Scripts.GameFlowScripts
{
    public class TimeService : IPausable
    {
        public event Action OnPause;
        public event Action OnResume;

        public void PauseAttack()
        {
            OnPause?.Invoke();
        }

        public void ResumeAttack()
        {
            OnResume?.Invoke();
        }
    }
}