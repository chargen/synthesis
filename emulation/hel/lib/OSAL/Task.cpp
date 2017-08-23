/*----------------------------------------------------------------------------*/
/* Copyright (c) FIRST 2008. All Rights Reserved.							  */
/* Open Source Software - may be modified and shared by FRC teams. The code   */
/* must be accompanied by the FIRST BSD license file in $(WIND_BASE)/WPILib.  */
/*----------------------------------------------------------------------------*/

#include "OSAL/Task.h"

#include "NetworkCommunication/UsageReporting.h"
#include "WPIErrors.h"
#include <string.h>
#include <stdint.h>
#include <stdio.h>

#if USE_WINAPI
#include <Windows.h>
#elif USE_POSIX
#include <pthread.h>
#include <signal.h>
#endif

/**
* Create but don't launch a task.
* @param name The name of the task.  "FRC_" will be prepended to the task name.
* @param function The address of the function to run as the new task.
* @param priority The VxWorks priority for the task.
* @param stackSize The size of the stack for the task
*/
NTTask::NTTask(const char* name, PTHREAD_START_ROUTINE function, NTTaskPriority priority)
{
	m_Handle = 0;
	m_function = function;
	m_priority = priority;
	m_taskName = new char[strlen(name) + 5];
	m_Arg = NULL;
	valid = false;
	strcpy(m_taskName, "FRC_");
	strcpy(m_taskName+4, name);

	static int32_t instances = 0;
	instances++;
	//nUsageReporting::report(nUsageReporting::kResourceType_Task, instances, 0, m_taskName);
}

NTTask::~NTTask()
{
	if (m_Handle != NULL) Stop();
	delete [] m_taskName;
	m_taskName = NULL;
	valid = false;
}

/**
* Starts this task.
* If it is already running or unable to start, it fails and returns false.
*/
bool NTTask::Start(void *arg)
{
	//m_taskID = taskSpawn(m_taskName,
	//					m_priority,
	//					VX_FP_TASK,							// options
	//					m_stackSize,						// stack size
	//					m_function,							// function to start
	//					arg0, arg1, arg2, arg3, arg4,	// parameter 1 - pointer to this class
	//					arg5, arg6, arg7, arg8, arg9);// additional unused parameters
	m_Arg = arg;
#if USE_WINAPI
	DWORD m_ID;
	m_Handle = CreateThread(NULL,20000, m_function,arg, 0,&m_ID);
	valid = m_Handle != NULL;
#elif USE_POSIX
	pthread_attr_t attr;
	pthread_attr_init(&attr);
	int creation = pthread_create(&m_Handle, &attr, funcWrapper, this);
	if (creation != 0){
		fprintf(stderr, "PThread Create Failed: %d\n", creation);
		valid = false;
	}
	valid = true;
#endif
	return valid;
}

/**
* Restarts a running task.
* If the task isn't started, it starts it.
* @return false if the task is running and we are unable to kill the previous instance
*/
bool NTTask::Restart()
{
	if (!Stop()) return false;
	return Start(m_Arg);
}

/**
* Kills the running task.
* @returns true on success false if the task doesn't exist or we are unable to kill it.
*/
bool NTTask::Stop()
{
	bool ok = true;
	if (Verify())
	{
#if USE_WINAPI
		DWORD exit = 0;
		ok = HandleError("NTTask::Stop()", TerminateThread(m_Handle, exit));
		CloseHandle(m_Handle);
#elif USE_POSIX
		pthread_kill(m_Handle, 15);	// SEGTERM
#endif
	}
	valid = false;
	m_Handle = 0;	// Break the reference
	return ok;
}

/**
* Returns true if the task is ready to execute (i.e. not suspended, delayed, or blocked).
* @return true if ready, false if not ready.
*/
bool NTTask::IsReady()
{
	return true;
}

/**
* Returns true if the task was explicitly suspended by calling Suspend()
* @return true if suspended, false if not suspended.
*/
bool NTTask::IsSuspended()
{
	return false;//GetThread(m_taskID);
}

/**
* Pauses a running task.
* Returns true on success, false if unable to pause or the task isn't running.
*/
bool NTTask::Suspend()
{
	//return HandleError("NTTask::Suspend", SuspendThread(m_Handle));
	printf("I hate you (NTTask::Suspend)\n");
	return false;
}

/**
* Resumes a paused task.
* Returns true on success, false if unable to resume or if the task isn't running/paused.
*/
bool NTTask::Resume()
{
	//return HandleError("NTTask::Resume", ResumeThread(m_Handle));
	printf("I hate you (NTTask::Resume)\n");
	return false;
}

/**
* Verifies a task still exists.
* @returns true on success.
*/
bool NTTask::Verify()
{
	if (!valid) {
		return false;
	}
#if USE_WINAPI
	DWORD code = 0;
	if (!GetExitCodeThread(m_Handle, &code)) {
		return false;
	}
	return code == STILL_ACTIVE;
#elif USE_POSIX
	return true;
#endif
}

#if USE_POSIX
void* NTTask::funcWrapper(void *task) {
	Task *internal = ((Task*)task);
	int code = internal->m_function(internal->m_Arg);
	internal->valid = false;
	return (void*) code;
}
#endif

/**
* Returns the name of the task.
* @returns Pointer to the name of the task or NULL if not allocated
*/
const char* NTTask::GetName()
{
	return m_taskName;
}

/**
* Handles errors generated by task related code.
*/
bool NTTask::HandleError(char *lpszFunction, int code)
{
	if (code == 0) return true;
#if USE_WINAPI
	LPVOID lpMsgBuf;
	DWORD dw = GetLastError();
	if (dw == 0) return true;

	FormatMessage(
		FORMAT_MESSAGE_ALLOCATE_BUFFER | 
		FORMAT_MESSAGE_FROM_SYSTEM |
		FORMAT_MESSAGE_IGNORE_INSERTS,
		NULL,
		dw,
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
		(LPTSTR) &lpMsgBuf,
		0, NULL );

	// Display the error message.
	printf_s(TEXT("%s failed with error %d: %s"), 
		lpszFunction, dw, lpMsgBuf);

	// Free error-handling buffer allocations.

	LocalFree(lpMsgBuf);
#elif USE_POSIX
	printf("PThread Error: %s\n", lpszFunction);
#endif
	return false;
}
