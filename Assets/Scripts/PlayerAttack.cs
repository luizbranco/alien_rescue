﻿using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	public float powerUpTimer = 5f;
	public GameObject weapon;

	private bool holdingWeapon = false;
	
	Player player;
	
	private void Awake () {
		player =  gameObject.GetComponent<Player>();
	}
	
	private void OnCollisionEnter2D(Collision2D coll){
		string layer = LayerMask.LayerToName(coll.gameObject.layer);
		if (layer == "Enemies") {
			if (holdingWeapon) {
				KillEnemy(coll.gameObject);
			} else {
				KillPlayer();
			}
		}
	}
	
	private void OnTriggerEnter2D(Collider2D coll) {
		string name = coll.gameObject.name;
		if (name == "Sword") {
			EquipWeapon();
			Destroy(coll.gameObject);
		}
	}

	public bool HoldingWeapon () {
		return holdingWeapon;
	}

	private void EquipWeapon () {
		holdingWeapon = true;
		weapon.renderer.enabled = true;
		Invoke("UnequipWeapon", powerUpTimer);
		player.sounds.PlayPowerupSound();
	}
	
	private void UnequipWeapon () {
		holdingWeapon = false;
		weapon.renderer.enabled = false;
	}

	private void KillPlayer () {
		rigidbody2D.velocity = new Vector2(0, 5f); 	// give hero a small jump kick
		rigidbody2D.isKinematic = false;  			// add kinematic back in case of being climbing
		collider2D.enabled = false;    				// allow hero to fall through anything
		player.SetDead();

	}
	
	private void KillEnemy (GameObject enemy) {
		int points = enemy.GetComponent<BaseEnemy>().KillForPoints();
		player.score.UpdateScore(points);
		player.sounds.PlayAttackSound();
	}

}