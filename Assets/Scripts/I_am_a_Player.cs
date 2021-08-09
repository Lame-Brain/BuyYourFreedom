using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_am_a_Player : MonoBehaviour
{
    public enum Directions { North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest }
    public enum Weapons { Sword, Arrow, Bomb }
    public GameObject myFace;
    public bool canMove;
    public float speed, dashModifier;
    public Weapons selectedWeapon;

    bool _move_up, _move_right, _move_down, _move_left, _button1, _button2, _button3, _Esc, _changedDirection;
    Directions _facing, _old_facing, _new_facing;
    Rigidbody2D _rigidBody;

    public void Init_Play()
    {
        _facing = Directions.North;
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        selectedWeapon = Weapons.Sword;
    }

    private void Update()
    {
        _move_up = false; _move_left = false; _move_down = false; _move_right = false;
        _button1 = false; _button2 = false; _button3 = false; _Esc = false;
        _old_facing = _facing; _changedDirection = false;

        //CONTROLS
        if (!GameManager.PAUSED && canMove)
        {
            //Get axis Input
            if (Input.GetAxisRaw("Vertical") > 0) _move_up = true;
            if (Input.GetAxisRaw("Horizontal") > 0) _move_left = true;
            if (Input.GetAxisRaw("Vertical") < 0) _move_down = true;
            if (Input.GetAxisRaw("Horizontal") < 0) _move_right = true;

            //Get Button Input
            if (Input.GetButtonUp("Cancel")) _Esc = true;
            if (Input.GetButtonUp("Fire1")) _button1 = true;
            if (Input.GetButtonUp("Fire2")) _button2 = true;
            if (Input.GetButton("Fire3")) _button3 = true;
        }

        //Facing
        if (_move_up && !_move_left && !_move_right && !_move_down) _new_facing = Directions.North;
        if (_move_up && _move_left && !_move_right && !_move_down) _new_facing = Directions.NorthEast;
        if (!_move_up && _move_left && !_move_right && !_move_down) _new_facing = Directions.East;
        if (!_move_up && _move_left && !_move_right && _move_down) _new_facing = Directions.SouthEast;
        if (!_move_up && !_move_left && !_move_right && _move_down) _new_facing = Directions.South;
        if (!_move_up && !_move_left && _move_right && _move_down) _new_facing = Directions.SouthWest;
        if (!_move_up && !_move_left && _move_right && !_move_down) _new_facing = Directions.West;
        if (_move_up && !_move_left && _move_right && !_move_down) _new_facing = Directions.NorthWest;
        if (_old_facing != _new_facing) _changedDirection = true;
        _facing = _new_facing;

        //Movement
        if (_facing == Directions.North) transform.eulerAngles = new Vector3(0, 0, 0);
        if (_facing == Directions.NorthEast) transform.eulerAngles = new Vector3(0, 0, 315);
        if (_facing == Directions.East) transform.eulerAngles = new Vector3(0, 0, 270);
        if (_facing == Directions.SouthEast) transform.eulerAngles = new Vector3(0, 0, 225);
        if (_facing == Directions.South) transform.eulerAngles = new Vector3(0, 0, 180);
        if (_facing == Directions.SouthWest) transform.eulerAngles = new Vector3(0, 0, 135);
        if (_facing == Directions.West) transform.eulerAngles = new Vector3(0, 0, 90);
        if (_facing == Directions.NorthWest) transform.eulerAngles = new Vector3(0, 0, 45);
        float _totalSpeed = speed;
        if (_button3)
        {
            _totalSpeed += dashModifier;            
            //Play Dash AudioClip
        }
        if (_move_up || _move_left || _move_down || _move_right) transform.Translate(Vector2.up * _totalSpeed * Time.deltaTime);

        //Slashing Sound
        //if(selectedWeapon == Weapons.Sword && _changedDirection) Play Slashing AudioClip

        //Weapon Selection
        if(selectedWeapon == Weapons.Sword)
        {
            transform.Find("Sword").gameObject.SetActive(true);
            transform.Find("Arrow").gameObject.SetActive(false);
            transform.Find("Bomb").gameObject.SetActive(false);
        }
        if(selectedWeapon == Weapons.Arrow)
        {
            transform.Find("Sword").gameObject.SetActive(false);
            transform.Find("Arrow").gameObject.SetActive(true);
            transform.Find("Bomb").gameObject.SetActive(false);
        }
        if(selectedWeapon == Weapons.Bomb)
        {
            transform.Find("Sword").gameObject.SetActive(false);
            transform.Find("Arrow").gameObject.SetActive(false);
            transform.Find("Bomb").gameObject.SetActive(true);
        }
    }
}
