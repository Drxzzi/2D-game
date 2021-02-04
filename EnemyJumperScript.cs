using UnityEngine;
using System.Collections;

public class EnemyJumperScript : MonoBehaviour
{

	private Rigidbody2D myRigidbody;  //allows us to apply physics to the jumper
	private Animator myAnimator;  //allows us to have control over different animation states
	public float forceY;

	void Awake()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
	}
	// Use this for initialization
	void Start()
	{
		StartCoroutine(Attack());
	}

	IEnumerator Attack()
	{
		yield return new WaitForSeconds(Random.Range(2, 4));  //Controls the pause between jumps
		forceY = Random.Range(250, 550);					  //control the magnitude of the jump variable
		myRigidbody.AddForce(new Vector2(0, forceY));         //applies the jump to the enemy 
		myAnimator.SetBool("attack", true);                   //allows the attack animation to play
		yield return new WaitForSeconds(1.5f);                //length of time to play the attack animation
		myAnimator.SetBool("attack", false);                  //allows enemy to go back to idle state
		StartCoroutine(Attack());							  //creates a loop to happen again
	}

	/*
	void OnTriggerEnter2D(Collider2D target)
	{

		if (target.tag == "bullet")
		{
			Destroy(gameObject);
			Destroy(target.gameObject);
		}
	}
*/

}