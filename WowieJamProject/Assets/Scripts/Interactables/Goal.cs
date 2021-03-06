using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAtoms.BaseAtoms;
using UnityEngine.InputSystem;

public class Goal : MonoBehaviour
{

    [SerializeField] VoidBaseEventReference LevelCompleteEvent;
    [SerializeField] GameObject GoalSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Goal");
        collision.GetComponentInChildren<Animator>().SetTrigger("Win");
        collision.GetComponent<PlayerController>().FreezeMovement = true; ;
        collision.transform.position = transform.position;

        GameObject sound = Instantiate(GoalSound, transform.position, Quaternion.identity, transform);
        Destroy(sound, 1f);

        Invoke("ActivateEndScreen", 1f);
    }

    void ActivateEndScreen()
    {
        LevelCompleteEvent.Event.Raise();
    }
}
