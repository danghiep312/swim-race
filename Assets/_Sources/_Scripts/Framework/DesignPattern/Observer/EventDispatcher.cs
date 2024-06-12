
namespace Framework.DesignPattern.Observer
{


	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using System;
	using Sirenix.OdinInspector;

	public class EventDispatcher : MonoBehaviour
	{
		#region Singleton

		private static EventDispatcher _sInstance;

		public static EventDispatcher Instance
		{
			get
			{
				// instance not exist, then create new one
				if (_sInstance == null)
				{
					// create new Gameobject, and add EventDispatcher component
					GameObject singletonObject = new GameObject();
					_sInstance = singletonObject.AddComponent<EventDispatcher>();
					singletonObject.name = "Singleton - EventDispatcher";
					Common.Log("Create singleton : {0}", singletonObject.name);
				}

				return _sInstance;
			}
		}

		public static bool HasInstance()
		{
			return _sInstance != null;
		}

		void Awake()
		{
			// check if there's another instance already exist in scene
			if (_sInstance != null && _sInstance.GetInstanceID() != this.GetInstanceID())
			{
				// Destroy this instances because already exist the singleton of EventsDispatcer
				Common.Log("An instance of EventDispatcher already exist : <{1}>, So destroy this instance : <{2}>!!",
					_sInstance.name, name);
				Destroy(gameObject);
			}
			else
			{
				// set instance
				_sInstance = this as EventDispatcher;
			}
		}


		void OnDestroy()
		{
			// reset this static var to null if it's the singleton instance
			if (_sInstance == this)
			{
				ClearAllListener();
				_sInstance = null;
			}
		}

		#endregion


		#region Fields

		/// Store all "listener"
		[ShowInInspector] Dictionary<EventID, Action<object>> _listeners = new Dictionary<EventID, Action<object>>();

		#endregion


		#region Add Listeners, Post events, Remove listener

		/// <summary>
		/// Register to listen for eventID
		/// </summary>
		/// <param name="eventID">EventID that object want to listen</param>
		/// <param name="callback">Callback will be invoked when this eventID be raised</param>
		public void RegisterListener(EventID eventID, Action<object> callback)
		{

			// check if listener exist in dictionary
			if (_listeners.ContainsKey(eventID))
			{
				// add callback to our collection
				_listeners[eventID] += callback;
			}
			else
			{
				// add new key-value pair
				_listeners.Add(eventID, null);
				_listeners[eventID] += callback;
			}
		}

		/// <summary>
		/// Posts the event. This will notify all listener that register for this event
		/// </summary>
		/// <param name="eventID">EventID.</param>
		/// <param name="param">Parameter. Can be anything (struct, class ...), Listener will make a cast to get the data</param>
		public void PostEvent(EventID eventID, object param = null)
		{
			if (!_listeners.ContainsKey(eventID))
			{
				Common.Log("No listeners for this event : {0}", eventID);
				return;
			}

			// posting event
			var callbacks = _listeners[eventID];
			// if there's no listener remain, then do nothing
			if (callbacks != null)
			{
				callbacks(param);
			}
			else
			{
				Common.Log("PostEvent {0}, but no listener remain, Remove this key", eventID);
				_listeners.Remove(eventID);
			}
		}

		/// <summary>
		/// Removes the listener. Use to Unregister listener
		/// </summary>
		/// <param name="eventID">EventID.</param>
		/// <param name="callback">Callback.</param>
		public void RemoveListener(EventID eventID, Action<object> callback)
		{
			// checking params
			Common.Assert(callback != null, "RemoveListener, event {0}, callback = null !!", eventID.ToString());
			Common.Assert(eventID != EventID.None, "AddListener, event = None !!");

			if (_listeners.ContainsKey(eventID))
			{
				_listeners[eventID] -= callback;
			}
			else
			{
				//Common.Warning(false, "RemoveListener, not found key : " + eventID);
			}
		}

		/// <summary>
		/// Clears all the listener.
		/// </summary>
		public void ClearAllListener()
		{
			_listeners.Clear();
		}

		#endregion
	}


	#region Extension class

	/// <summary>
	/// Declare some "shortcut" for using EventDispatcher easier
	/// </summary>
	public static class EventDispatcherExtension
	{
		/// Use for registering with EventsManager
		public static void RegisterListener(this MonoBehaviour listener, EventID eventID, Action<object> callback)
		{
			EventDispatcher.Instance.RegisterListener(eventID, callback);
		}

		/// Post event with param
		public static void PostEvent(this MonoBehaviour listener, EventID eventID, object param)
		{
			EventDispatcher.Instance.PostEvent(eventID, param);
		}

		/// Post event with no param (param = null)
		public static void PostEvent(this MonoBehaviour sender, EventID eventID)
		{
			EventDispatcher.Instance.PostEvent(eventID, null);
		}

		public static void RemoveListener(this MonoBehaviour listener, EventID eventID, Action<object> callback)
		{
			EventDispatcher.Instance.RemoveListener(eventID, callback);
		}
	}

	#endregion
}