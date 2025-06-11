using System;
using System.Collections.Generic;

namespace Project.Scripts.GameFlowScripts
{
    public class TimeService : IPausable
    {
        private readonly List<IPausable> _pausables;

        public TimeService(List<IPausable> pausables)
        {
            _pausables = pausables;
        }

        public void PauseAttack()
        {
            foreach (var p in _pausables)
                p.PauseAttack();
        }

        public void ResumeAttack()
        {
            foreach (var p in _pausables)
                p.ResumeAttack();
        }
    }
}