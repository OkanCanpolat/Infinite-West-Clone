using System.Collections.Generic;
using Zenject;

public class TurnController
{
    public bool IsLevelFinished;
    private SignalBus signalBus;
    private List<Enemy> aliveEnemies;
    private int aliveEnemyCount;
    private int enemyCountTurn;
    public TurnController(SignalBus signalBus, List<Enemy> aliveEnemies)
    {
        this.signalBus = signalBus;
        this.aliveEnemies = aliveEnemies;
        aliveEnemyCount = aliveEnemies.Count;
        enemyCountTurn = aliveEnemyCount;
        signalBus.Subscribe<EnemyTurnEndSignal>(EnemyFinishedTurn);
        signalBus.Subscribe<EnemyDiedSignal>(OnEnemyDied);
    }
    public void EndPlayerTurn()
    {
        signalBus.Fire<PlayerTurnEndSignal>();
    }

    private void EnemyFinishedTurn()
    {
        enemyCountTurn--;

        if (enemyCountTurn <= 0)
        {
            enemyCountTurn = aliveEnemyCount;
            signalBus.Fire<PlayerTurnStartSignal>();
        }
    }
    private void OnEnemyDied(EnemyDiedSignal signal)
    {
        aliveEnemies.Remove(signal.Enemy);
        aliveEnemyCount--;
        enemyCountTurn = aliveEnemyCount;

        if(aliveEnemyCount <= 0)
        {
            IsLevelFinished = true;
            signalBus.Fire<LevelFinishedSignal>();
        }
    }
}

public class PlayerTurnEndSignal { }
public class EnemyTurnEndSignal { }
public class PlayerTurnStartSignal { }
public class EnemyDiedSignal { public Enemy Enemy; }
public class LevelFinishedSignal { }

