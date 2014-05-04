using UnityEngine;
using System.Collections;

public enum AIOBJECT_STATUS
{
	ATS_IDLE 		= 0 ,
	ATS_STUN			,
	ATS_WALK			,
	ATS_RUN				,
	ATS_ATTACK			,
	ATS_SPELL			,
	ATS_PAIN			,
	ATS_DEATH			,

	ATS_NUM				,
} ;

public enum AIOBJECT_COMMAND
{
	ATC_NONE		= 0 ,
	ATC_IDLE 			,
	ATC_STUN			,
	ATC_WALK			,
	ATC_RUN				,
	ATC_ATTACK			,
	ATC_SPELL			,
	ATC_PAIN			,
	ATC_DEATH			,

	ATC_NUM				,
}


public class AIObject : AnimObject
{
	public AIOBJECT_STATUS 		CurrentAIStatus ;
	public Command 				CurrentCommand ;

	public float 				CurrentStatusTimeAcc ;
	public float 				CurrentStatusTimeTotal ;

	public float 				AttackRange ;
	public float 				AttackRate ;
	public float 				RunRate ;

	public AIObject 			Enemy ;
	public Vector3 				Destination ;

	public float 				CurrentHP ;
	public float 				HPMax ;
	public float 				CurrentMana ;
	public float 				ManaMax ;

	public float 				MoveSpeed ;

	public AIObject ()
	{
		CurrentAIStatus 		= AIOBJECT_STATUS.ATS_IDLE ;
		CurrentCommand 			= null ;
		CurrentStatusTimeAcc 	= 0.0f ;
		CurrentStatusTimeTotal 	= 1.0f ;

		AttackRange 			= 1.0f ;
		AttackRate 				= 1.0f ;
		RunRate 				= 1.0f ;

		MoveSpeed 				= 5.0f ;

		Enemy 					= null ;
		Destination 			= Vector3.zero ;
	}

	public void 			PushCmd ( Command cmd )
	{
		if ( CurrentCommand != null &&  
			 CurrentCommand.IsEqualTo ( cmd ) )
		{
			return ;
		}

		CurrentCommand = cmd ;

		switch ( CurrentCommand.CommandType )
		{
			case AIOBJECT_COMMAND.ATC_NONE :
			case AIOBJECT_COMMAND.ATC_IDLE :
			{
				StatusBegin ( AIOBJECT_STATUS.ATS_IDLE ) ;
				break ;
			}

			case AIOBJECT_COMMAND.ATC_STUN :
			{
				StatusBegin ( AIOBJECT_STATUS.ATS_STUN ) ;
				break ;
			}

			case AIOBJECT_COMMAND.ATC_WALK :
			{
				StatusBegin ( AIOBJECT_STATUS.ATS_WALK ) ;
				break ;
			}

			case AIOBJECT_COMMAND.ATC_RUN :
			{
				StatusBegin ( AIOBJECT_STATUS.ATS_RUN ) ;
				break ;
			}

			case AIOBJECT_COMMAND.ATC_ATTACK :
			{
				StatusBegin ( AIOBJECT_STATUS.ATS_ATTACK ) ;
				break ;
			}

			case AIOBJECT_COMMAND.ATC_SPELL :
			{
				StatusBegin ( AIOBJECT_STATUS.ATS_SPELL ) ;
				break ;
			}

			case AIOBJECT_COMMAND.ATC_PAIN :
			{
				StatusBegin ( AIOBJECT_STATUS.ATS_PAIN ) ;
				break ;
			}

			case AIOBJECT_COMMAND.ATC_DEATH :
			{
				StatusBegin ( AIOBJECT_STATUS.ATS_DEATH ) ;
				break ;
			}

			default :
			{
				break ;
			}
		}


		OnExecuteCmd ( cmd ) ;
		SetActionAnimation ( CurrentAIStatus ) ;
	}

	public virtual void 	OnExecuteCmd ( Command cmd )
	{
		cmd.Execute () ;
	}

	public virtual void 	Tick ( float deltaTime )
	{
		UpdateStatus ( deltaTime ) ;
	}

	public virtual void 	AttackEnemy ( AIObject enemy )
	{
		;
	}

	public bool 			IsAlive ()
	{
		return CurrentHP > 0.0f ;
	}

	protected void 			SetActionAnimation ( AIOBJECT_STATUS status )
	{
		switch ( status )
		{
			case AIOBJECT_STATUS.ATS_IDLE :
			{
				PlayAction ( "idle" ) ;
				break ;
			}

			case AIOBJECT_STATUS.ATS_STUN :
			{
				PlayAction ( "pain" , 0.0f ) ;
				break ;
			}

			case AIOBJECT_STATUS.ATS_WALK :
			{
				PlayAction ( "walk" ) ;
				break ;
			}

			case AIOBJECT_STATUS.ATS_RUN :
			{
				PlayAction ( "run" , RunRate ) ;
				break ;
			}

			case AIOBJECT_STATUS.ATS_ATTACK :
			{
				PlayAction ( "attack" , AttackRate ) ;
				break ;
			}

			case AIOBJECT_STATUS.ATS_SPELL :
			{
				StatusBegin ( AIOBJECT_STATUS.ATS_SPELL ) ;
				break ;
			}

			case AIOBJECT_STATUS.ATS_PAIN :
			{
				PlayAction ( "pain" ) ;
				break ;
			}

			case AIOBJECT_STATUS.ATS_DEATH :
			{
				PlayAction ( "death" ) ;
				break ;
			}

			default :
			{
				break ;
			}
		}
	}

	protected void 			StatusBegin ( AIOBJECT_STATUS status )
	{
		CurrentAIStatus 		= status ;
		CurrentStatusTimeAcc 	= 0.0f ;
		CurrentStatusTimeTotal 	= 1.0f ;

		OnStatusBegin ( CurrentAIStatus ) ;
	}

	protected virtual void  OnStatusBegin ( AIOBJECT_STATUS status )
	{
		;
	}

	protected void 			StatusUpdate ( float deltaTime )
	{
		CurrentStatusTimeAcc += deltaTime ;
		if ( CurrentStatusTimeAcc >= CurrentStatusTimeTotal )
		{
			StatusEnd () ;
			return ;
		}

		OnStatusUpdate ( deltaTime ) ;
	}

	protected virtual void  OnStatusUpdate ( float deltaTime )
	{
		;
	}

	protected void 			StatusEnd ()
	{
		OnStatusEnd () ;

		CurrentAIStatus 		= AIOBJECT_STATUS.ATS_IDLE ;
		CurrentStatusTimeAcc 	= 0.0f ;
		CurrentStatusTimeTotal 	= 0.0f ;
	}

	protected virtual void  OnStatusEnd ()
	{
		;
	}

	protected void 			UpdateStatus ( float deltaTime )
	{
		if ( CurrentAIStatus != AIOBJECT_STATUS.ATS_IDLE )
		{
			StatusUpdate ( deltaTime ) ;
			if ( CurrentAIStatus == AIOBJECT_STATUS.ATS_RUN || 
				 CurrentAIStatus == AIOBJECT_STATUS.ATS_WALK )
			{
				Vector3 distVector 	= Destination - Position ;
				Vector3 moveDir 	= distVector.normalized ;
				Vector3 move 		= moveDir * MoveSpeed * deltaTime ;
				SetPosition ( Position + move ) ;

				float faceDirRad = Mathf.Acos ( -moveDir.z ) ;
				float faceDirAngle = faceDirRad * 180.0f / Mathf.PI ;

				if ( moveDir.x > 0.0f )
				{
					SetRotation ( Quaternion.AngleAxis ( 180.0f - faceDirAngle , Vector3.up ) ) ;
				}
				else
				{
					SetRotation ( Quaternion.AngleAxis ( faceDirAngle - 180.0f , Vector3.up ) ) ;
				}
			}
		}
	}
}
