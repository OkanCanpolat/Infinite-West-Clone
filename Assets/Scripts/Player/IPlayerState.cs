using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState 
{
    public void OnEnter();
    public void OnExit();
    public void OnPlayerClickDown();
    public void OnPlayerClickUp();
    public void OnEnemyClickDown(Enemy enemy);
}
