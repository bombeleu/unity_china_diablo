using UnityEngine;
using System.Collections;

public class Command
{
	public AIObject 			ExecuteObject ;
	public AIOBJECT_COMMAND 	CommandType ;

	public Command ()
	{
		ExecuteObject 	= null ;
		CommandType		= AIOBJECT_COMMAND.ATC_NONE ;
	}

	public virtual void 		Execute ()
	{
		;
	}

	public virtual bool 		IsEqualTo ( Command cmd )
	{
		if ( ExecuteObject != cmd.ExecuteObject )
		{
			return false ;
		}

		if ( CommandType != cmd.CommandType )
		{
			return false ;
		}

		return true ;
	}
}
