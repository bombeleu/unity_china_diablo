using UnityEngine;
using System.Collections;

public class AttackCommand : Command 
{
	public AIObject 		ObjectToAttack ;

	public AttackCommand ()
	{
		CommandType	= AIOBJECT_COMMAND.ATC_ATTACK ;
	}

	public override void 	Execute ()
	{
		;
	}

	public override bool 	IsEqualTo ( Command cmd )
	{
		AttackCommand otherCmd = cmd as AttackCommand ;
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

		if ( ObjectToAttack != otherCmd.ObjectToAttack )
		{
			return false ;
		}

		return true ;
	}
}
