using System;
using System.Threading;
using UnityEngine;

public class WaitForThreadedJobWithVerifier : CustomYieldInstruction
{
    private JobCompleteVerifier checker;

    public WaitForThreadedJobWithVerifier(Action task, JobCompleteVerifier checker, System.Threading.ThreadPriority priority = System.Threading.ThreadPriority.Normal)
	{
		this.checker = checker;
		new Thread(delegate()
		{
			task();
		}).Start(priority);
	}

	public override bool keepWaiting
	{
		get
		{
			return !checker.isTaskCompleted;
		}
	}
}
