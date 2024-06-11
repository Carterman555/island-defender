using IslandDefender;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// One repository for all scriptable objects. Create your query methods here to keep your business logic clean.
/// I make this a MonoBehaviour as sometimes I add some debug/development references in the editor.
/// If you don't feel free to make this a standard class
/// </summary>
public class ResourceSystem : StaticInstance<ResourceSystem>
{
    public List<ScriptableEnemy> Enemies { get; private set; }
    private Dictionary<EnemyType, ScriptableEnemy> EnemiesDict;

    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }

    private void AssembleResources() {
        Enemies = Resources.LoadAll<ScriptableEnemy>("").ToList();
        EnemiesDict = Enemies.ToDictionary(r => r.EnemyType, r => r);
    }

    public ScriptableEnemy GetEnemy(EnemyType t) => EnemiesDict[t];
}   