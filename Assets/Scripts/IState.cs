using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void EnterState();

    void UpdateState();

    void Impact();

    void OnTriggerEnter(Collider col);

    void OnTriggerStay(Collider col);

    void OnTriggerExit(Collider col);
}
