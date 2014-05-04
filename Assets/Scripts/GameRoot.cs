using UnityEngine ;
using UnityEditor ;
using System.Collections;

public class GameRoot : MonoBehaviour
{
	public Hero 			fighter ;
	public AnimObject 		zombie ;
	public BaseObject 		floor ;

	public Camera 			mainCamera ;
	public float 			distanceFromPlayer ;
	public float 			yawAngle ;

	public Vector3 			destPoint ;

	void Awake ()
	{
		fighter = new Hero () ;
		fighter.LoadObject ( "Avatars/Hero/femaleWarrior/Prefabs/femaleWarriorPants" ) ;
		fighter.SetPosition ( Vector3.zero ) ;

		zombie = new AnimObject () ;
		zombie.LoadObject ( "Avatars/Monster/Troll_Assets/Prefabs/troll" ) ;
		zombie.SetPosition ( new Vector3 ( 0.0f , 0.0f , 5.0f ) ) ;

		floor = new BaseObject () ;
		floor.LoadObject ( "Scene/Floor/floor" ) ;
		floor.SetPosition ( new Vector3 ( 0.0f , -0.5f , 0.0f ) ) ;

		mainCamera = GameObject.Find ( "Main Camera" ).GetComponent<Camera> () ;
		mainCamera.transform.position = new Vector3 ( 0.0f , 5.0f , -5.0f ) ;
		mainCamera.transform.LookAt ( fighter.GetTransform () ) ;

		distanceFromPlayer 	= 14.0f ;
		yawAngle 			= 45.0f ;

		destPoint 			= Vector3.zero ;
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		float delta = Time.deltaTime ;

		UpdateInput ( delta ) ;
		UpdatePlayer ( delta ) ;
		UpdateCamera ( delta ) ;
	}

	void UpdateCamera ( float deltaTime )
	{
		float x 	= fighter.Position.x ;

		float y 	= fighter.Position.y ;
		float yAdd 	= distanceFromPlayer * Mathf.Sin ( yawAngle * Mathf.PI / 180.0f ) ;

		float z 	= fighter.Position.z ;
		float zAdd	= distanceFromPlayer * Mathf.Cos ( yawAngle * Mathf.PI / 180.0f ) ;

		mainCamera.transform.position = new Vector3 ( x , y + yAdd , z + zAdd ) ;
		mainCamera.transform.LookAt ( fighter.GetTransform () ) ;
	}

	Vector3 GetPointOnGroundFromRay ( Vector3 rayVector , Vector3 rayOrigin )
	{
		float x = rayVector.x / rayVector.y * ( 0.0f - rayOrigin.y ) + rayOrigin.x ;
		float z = rayVector.z / rayVector.y * ( 0.0f - rayOrigin.y ) + rayOrigin.z ;
		return new Vector3 ( x , 0.0f , z ) ;
	}

	void UpdateInput ( float deltaTime )
	{
		if ( Input.GetMouseButton ( 0 ) )
		{
			Vector3 mouseDownPos = Input.mousePosition ;
			Ray ray = mainCamera.ScreenPointToRay ( mouseDownPos ) ;

			Vector3 ptInGround = GetPointOnGroundFromRay ( ray.direction , ray.origin ) ;
			destPoint = ptInGround ;

			fighter.RunTo ( destPoint ) ;
		}
	}

	void UpdatePlayer ( float deltaTime )
	{
		fighter.Tick ( deltaTime ) ;
	}
}
