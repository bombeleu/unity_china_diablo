using UnityEngine;
using UnityEditor ;
using System.Collections;

public class BaseObject
{
	public Vector3 			Position ;
	public Vector3 			Scale ;
	public Quaternion		Rotation ;

	protected GameObject 	innerObject ;
	protected BaseObject 	parent ;

	public BaseObject ()
	{
		Position 	= Vector3.zero ;
		Scale		= Vector3.one ;
		Rotation 	= Quaternion.identity ;

		innerObject = null ;
		parent 		= null ;
	}

	public GameObject 		GetGameObject ()
	{
		return innerObject ;
	}

	public Transform 		GetTransform ()
	{
		if ( innerObject != null )
		{
			return innerObject.transform ;
		}

		return null ;
	}

	public BaseObject 		GetParent ()
	{
		return parent ;
	}

	public virtual bool 	LoadObject ( string objectPath )
	{
		GameObject tmpObj = Resources.Load ( objectPath , typeof ( GameObject ) ) as GameObject ;
		innerObject = GameObject.Instantiate ( tmpObj ) as GameObject ;

		SetPosition ( Position ) ;
		SetScale ( Scale ) ;
		SetRotation ( Rotation ) ;

		return ( innerObject != null ) ;
	}

	public void 			SetPosition ( Vector3 pos )
	{
		Position = pos ;
		if ( innerObject != null )
		{
			innerObject.transform.position = Position ;
		}
	}

	public void 			SetScale ( Vector3 scale )
	{
		Scale = scale ;
		if ( innerObject != null )
		{
			innerObject.transform.localScale = Scale ;
		}
	}

	public void 			SetRotation ( Quaternion quat )
	{
		Rotation = quat ;
		if ( innerObject != null )
		{
			innerObject.transform.localRotation = Rotation ;
		}
	}
}
