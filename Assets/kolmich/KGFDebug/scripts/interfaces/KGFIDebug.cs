using UnityEngine;
using System.Collections;

/// <summary>
/// the Debug Module interface of the Kolmich Game Framework.
/// </summary>
/// <remarks>
/// To create your own custom log classes implement this interface. Make sure you add the instance with KGFDebug.Add() to the list of current active loggers.
/// </remarks>
public interface KGFIDebug
{
	/// <summary>
	/// Use this method to get the name of the KGFILog implementation
	/// </summary>
	/// <remarks>
	/// All loggers can have a name for identification. It is allowed to have multiple loggers with the same name registered.
	/// </remarks>
	///<returns>returns the name of this KGFILog implementation</returns>
	string GetName();
	
	/// <summary>
	/// Logs a message
	/// </summary>
	/// <remarks>
	/// Logs to the KGFIDebug implementation
	/// </remarks>
	/// <param name="theLog">the log entry</param>
	void Log(KGFDebug.KGFDebugLog theLog);
	
	/// <summary>
	/// Logs a message with no reference to a MonoBehaviour game object and no stack trace
	/// </summary>
	/// <remarks>
	/// Logs a basic log message with no reference to the gameObject that created this log messages.
	/// </remarks>
	/// <param name="theLevel">log level of this log entry</param>
	/// <param name="theCategory">category of this log entry</param>
	/// <param name="theMessage">message of this log entry</param>
	void Log(KGFeDebugLevel theLevel, string theCategory, string theMessage);
	
	/// <summary>
	/// logs a message with stack trace but no reference to a MonoBehaviour game object
	/// </summary>
	/// <remarks>
	/// Logs a message with category and stack trace, but no refrence to the object that created this log entry.
	/// </remarks>
	/// <param name="theLevel">log level of this log entry</param>
	/// <param name="theCategory">category of this log entry</param>
	/// <param name="theMessage">message of this log entry</param>
	/// <param name="theStackTrace">stack trace of this log entry</param>
	void Log(KGFeDebugLevel theLevel, string theCategory, string theMessage, string theStackTrace);
	
	/// <summary>
	/// logs a message with reference to a MonoBehaviour game object and stack trace
	/// </summary>
	/// <remarks>
	/// Creates an extended log entry with stack trace and a reference to the objext that created this gameObject.
	/// </remarks>
	/// <param name="theLevel">log level of this log entry</param>
	/// <param name="theCategory">category of this log entry</param>
	/// <param name="theMessage">message of this log entry</param>
	/// <param name="theStackTrace">stack trace of this log entry</param>
	/// <param name="theObject">game object referenced to this log entry</param>
	void Log(KGFeDebugLevel theLevel, string theCategory, string theMessage, string theStackTrace, MonoBehaviour theObject);
	
	/// <summary>
	/// defines the minimum log level this implementaion is wirting. all lower rated categorys are not handled.
	/// </summary>
	/// <remarks>
	/// With this method the minimum log level can be set. If the minimum log level is set to error all logs lower than error (warning, info, debug) will no longer be received by this logger.
	/// To keep performance overhead from logging as low as possible, keep the minimum log level as high as possible.
	/// </remarks>
	/// <param name="theLevel">the minimum log level</param>
	void SetMinimumLogLevel(KGFeDebugLevel theLevel);
	
	/// <summary>
	/// use this method to get the current minimum log level of this KFGILog implemenation
	/// </summary>
	/// <remarks>
	/// Use this method to determine the current minimum log level of this KGFIDebug implementation.
	/// </remarks>
	///<returns>returns the current minimum log level of this KGFILog implementation</returns>
	KGFeDebugLevel GetMinimumLogLevel();
}