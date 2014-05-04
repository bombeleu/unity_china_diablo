using UnityEngine;
using System.Collections;

public class RunCommand : Command 
{
	public Vector3 			Position ;

	public RunCommand ()
	{
		CommandType	= AIOBJECT_COMMAND.ATC_RUN ;
	}

	public override void 	Execute ()
	{
		;
	}

	public override bool 	IsEqualTo ( Command cmd )
	{
		RunCommand otherCmd = cmd as RunCommand ;
		if ( otherCmd == null )
		{
			return false ;
		}

		if ( ExecuteObject != otherCmd.ExecuteObject )
		{
			return false ;
		}

		if ( CommandType != otherCmd.CommandType )
		{
			return false ;
		}

		if ( Position != otherCmd.Position )
		{
			return false ;
		}

		return true ;
	}
}
