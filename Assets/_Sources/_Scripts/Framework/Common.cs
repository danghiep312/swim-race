#if UNITY_EDITOR
#define DEBUG
#define ASSERT
#endif
using UnityEngine;
using System.Collections;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Common
{
	
	public static bool BLOCK_DEBUG;
	//-----------------------------------
	//--------------------- Log , warning, 

	[Conditional("DEBUG")]

	public static void FetchData()
	{
		BLOCK_DEBUG = PlayerPrefs.GetInt("block_debug", 0) == 1;
	}
	
	public static void Log(object message, Object context = null)
	{
		if (BLOCK_DEBUG) return;
		if (context != null)
		{
			Debug.Log(message + " " + context.name, context);
		}
		else
		{
			Debug.Log(message, context);
		}
	}

	[Conditional("DEBUG")]
	public static void Log(string format, params object[] args)
	{
		if (BLOCK_DEBUG) return;
		Debug.Log(string.Format(format, args));
	}

	[Conditional("DEBUG")]
	public static void LogWarning(object message, Object context)
	{
		if (BLOCK_DEBUG) return;
		Debug.LogWarning(message, context);
	}

	[Conditional("DEBUG")]
	public static void LogWarning(Object context, string format, params object[] args)
	{
		if (BLOCK_DEBUG) return;
		Debug.LogWarning(string.Format(format, args), context);
	}



	[Conditional("DEBUG")]
	public static void Warning(bool condition, object message)
	{
		if (BLOCK_DEBUG) return;
		if ( ! condition) Debug.LogWarning(message);
	}

	[Conditional("DEBUG")]
	public static void Warning(bool condition, object message, Object context)
	{
		if (BLOCK_DEBUG) return;
		if ( ! condition) Debug.LogWarning(message, context);
	}

	[Conditional("DEBUG")]
	public static void Warning(bool condition, Object context, string format, params object[] args)
	{
		if (BLOCK_DEBUG) return;
		if ( ! condition) Debug.LogWarning(string.Format(format, args), context);
	}


	//---------------------------------------------
	//------------- Assert ------------------------

	/// Thown an exception if condition = false
	[Conditional("ASSERT")]
	public static void Assert(bool condition)
	{
		if (BLOCK_DEBUG) return;
		if (! condition) throw new UnityException();
	}

	/// Thown an exception if condition = false, show message on console's log
	[Conditional("ASSERT")]
	public static void Assert(bool condition, string message)
	{
		if (BLOCK_DEBUG) return;
		if (! condition) throw new UnityException(message);
	}

	/// Thown an exception if condition = false, show message on console's log
	[Conditional("ASSERT")]
	public static void Assert(bool condition, string format, params object[] args)
	{
		if (BLOCK_DEBUG) return;
		if (! condition) throw new UnityException(string.Format(format, args));
	}
	
	[Conditional("ASSERT")]
	public static void LogFormat(string format, params object[] args)
	{
		if (BLOCK_DEBUG) return;
		Debug.LogFormat(format, args);
	}
}
