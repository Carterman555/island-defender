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
    private Dictionary<EnemyType, ScriptableEnemy> enemiesDict;

    public List<ScriptableAlly> Allies { get; private set; }
    private Dictionary<AllyType, ScriptableAlly> alliesDict;

    public List<ScriptableBuilding> Buildings { get; private set; }
    private Dictionary<BuildingType, ScriptableBuilding> buildingsDict;

    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }

    private void AssembleResources() {
        Enemies = Resources.LoadAll<ScriptableEnemy>("").ToList();
        enemiesDict = Enemies.ToDictionary(r => r.EnemyType, r => r);

        Allies = Resources.LoadAll<ScriptableAlly>("").ToList();
        alliesDict = Allies.ToDictionary(r => r.AllyType, r => r);

        Buildings = Resources.LoadAll<ScriptableBuilding>("").ToList();
        buildingsDict = Buildings.ToDictionary(r => r.BuildingType, r => r);
    }

    public ScriptableEnemy GetEnemy(EnemyType t) => enemiesDict[t];
    public ScriptableAlly GetAlly(AllyType t) => alliesDict[t];
    public ScriptableBuilding GetBuilding(BuildingType t) => buildingsDict[t];
}