using UnityEngine;
using System.Collections;

public enum HERO_STATUS
{
	HRO_IDLE		= 0 ,
	HRO_RUN				,
	HRO_RUNFORATTACK	,
	HRO_ATTACK 			,

	HRO_NUM				,
} ;

public class Hero : AIObject
{
	public HERO_STATUS 		GamePlayStatus ;

	public Hero ()
	{
		;
	}

	public override void 	AttackEnemy ( AIObject enemy )
	{
		if ( enemy == null )
		{
			return ;
		}
			
		if ( !enemy.IsAlive() )
		{
			return ;
		}


		Enemy = enemy ;

		float attackDist = Vector3.Distance ( Position , enemy.Position ) ;
		if ( attackDist <= AttackRange )
		{
			GamePlayStatus = HERO_STATUS.HRO_ATTACK ;
		}
		else
		{
			GamePlayStatus = HERO_STATUS.HRO_RUNFORATTACK ;
		}
	}

	public void 			RunTo ( Vector3 destPosition )
	{
		GamePlayStatus = HERO_STATUS.HRO_RUN ;
		Destination = destPosition ;
	}

	public void 			Idle ()
	{
		GamePlayStatus = HERO_STATUS.HRO_IDLE ;
	}
	
	public override void 	Tick ( float deltaTime )
	{
		base.Tick ( deltaTime ) ;
		UpdateGamePlayStatus ( deltaTime ) ;
	}

	public override void 	OnExecuteCmd ( Command cmd )
	{
		base.OnExecuteCmd ( cmd ) ;

		if ( cmd.CommandType == AIOBJECT_COMMAND.ATC_RUN )
		{
			RunCommand cCmd = cmd as RunCommand ;
			Destination = cCmd.Position ;
		}
		else if ( cmd.CommandType == AIOBJECT_COMMAND.ATC_ATTACK )
		{
			AttackCommand cCmd = cmd as AttackCommand ;
			Enemy = cCmd.ObjectToAttack ;
		}
	}

	protected void 			UpdateGamePlayStatus ( float deltaTime )
	{
		UnityEngine.Debug.Log ( "UpdateGamePlayStatus: " + GamePlayStatus.ToString() ) ;

		switch ( GamePlayStatus )
		{
			case HERO_STATUS.HRO_IDLE :
			{
				break ;
			}

			case HERO_STATUS.HRO_RUN :
			{
				Vector3 distVector = Destination - Position ;
				float dist = distVector.magnitude ;
				if ( dist < 0.1f )
				{
					GamePlayStatus = HERO_STATUS.HRO_IDLE ;
					PushCmd ( new Command () ) ;
				}
				else
				{
					RunCommand cmd = new RunCommand () ;
					cmd.Position = Destination ;
					PushCmd ( cmd ) ;
				}

				break ;
			}

			case HERO_STATUS.HRO_RUNFORATTACK :
			{
				Vector3 distVector = Destination - Position ;
				float dist = distVector.magnitude ;
				if ( dist < AttackRange )
				{
					GamePlayStatus = HERO_STATUS.HRO_ATTACK ;
					AttackCommand cmd = new AttackCommand () ;
					cmd.ObjectToAttack = Enemy ;
					PushCmd ( cmd ) ;
				}
				else
				{
					RunCommand cmd = new RunCommand () ;
					cmd.Position = Destination ;
					PushCmd ( cmd ) ;
				}

				break ;
			}

			case HERO_STATUS.HRO_ATTACK :
			{
				AttackCommand cmd = new AttackCommand () ;
				cmd.ObjectToAttack = Enemy ;
				PushCmd ( cmd ) ;
				break ;
			}

			default :
			{
				break ;
			}
		}
	}

}
