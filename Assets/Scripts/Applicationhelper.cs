using UnityEngine;

public static class ApplicationHelper
{
	public static bool IsQuitting { get; private set; }
	static ApplicationHelper()
	{
		Application.quitting += () => IsQuitting = true;
	}
}