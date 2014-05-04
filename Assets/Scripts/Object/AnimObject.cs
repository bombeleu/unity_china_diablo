using UnityEngine;
using System.Collections;

public class AnimObject : PhysicsObject
{
	public string 				CurrentActionName ;
	public float 				ActionRate ;
	
	protected Animation 		anim ;
	protected AnimationState	animState ;
	
	public AnimObject ()
	{
		CurrentActionName 	= "" ;
		anim 				= null ;
		animState 			= null ;
		ActionRate 			= 1.0f ;
	}
	
	public override bool 	LoadObject ( string objectPath )
	{
		base.LoadObject ( objectPath ) ;
		
		if ( innerObject != null )
		{
			anim = innerObject.GetComponent ( typeof ( Animation ) ) as Animation ;
		}

		return ( innerObject != null ) ;
	}

	public void 			PlayAction ( string actionName )
	{
		PlayAction ( actionName , 1.0f ) ;
	}

	public void 			PlayAction ( string actionName , float animRate )
	{
		animState = anim[actionName] ;
		if ( animState != null )
		{
			anim.Play ( actionName ) ;
			CurrentActionName = actionName ;
			SetCurrentActionRate ( animRate ) ;
		}
	}

	public void 			CrossFadeAction ( string actionName , float fadeTime )
	{
		CrossFadeAction ( actionName , fadeTime , 1.0f ) ;
	}

	public void 			CrossFadeAction ( string actionName , float fadeTime , float animRate )
	{
		animState = anim[actionName] ;
		if ( animState != null )
		{
			anim.CrossFade ( actionName , fadeTime ) ;
			CurrentActionName = actionName ;
			SetCurrentActionRate ( animRate ) ;
		}
	}

	public void 			SetCurrentActionRate ( float animRate )
	{
		if ( animState != null )
		{
			ActionRate = animRate ;
			animState.speed = ActionRate ;
		}
	}
}
