using UnityEngine;
using System.Collections;

public class PhysicsObject : BaseObject
{
	public Collider 		innerCollider ;

	public PhysicsObject ()
	{
		innerCollider = null ;
	}

	public override bool 	LoadObject ( string objectPath )
	{
		base.LoadObject ( objectPath ) ;

		if ( innerObject != null )
		{
			innerCollider = innerObject.GetComponent ( typeof ( Collider ) ) as Collider ;
		}

		return ( innerObject != null ) ;
	}

	public bool 			IntersectsWith ( Ray ray , out RaycastHit hit )
	{
		hit = new RaycastHit () ;

		if ( innerCollider != null )
		{
			return innerCollider.Raycast ( ray , out hit , Mathf.Infinity ) ;
		}

		return false ;
	}
}
