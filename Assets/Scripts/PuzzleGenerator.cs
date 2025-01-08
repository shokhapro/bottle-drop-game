using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    [Serializable]
    public class PuzzleLevel
    {
        [Serializable]
        public class PuzzleLevelBottle
        {
            public int[] ballMatIndices = new int[0];
        }

        public PuzzleLevelBottle[] bottles = new PuzzleLevelBottle[0];
        public int firstDropBallMatIndex = 0;
    }

#if UNITY_EDITOR
    [SerializeField] private int testLevelIndex = -1;
    [Space]
#endif
    [SerializeField] private PuzzleLevel[] levels = new PuzzleLevel[0];
    [Space]
    [SerializeField] private Material[] ballMaterials = new Material[0];
    [Space]
    [SerializeField] private PuzzleBottle bottlePrefab;
    [SerializeField] private PuzzleBall ballPrefab;
    [Space]
    [SerializeField] private Transform bottleParent;
    [SerializeField] private PuzzleCylinder cylinder;
    [Space]
    [SerializeField] private TextMeshProUGUI levelText;

    private int _activeLevel = -1;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("level-index")) PlayerPrefs.SetInt("level-index", 0);
    }

    private void Start()
    {
        var levelIndex = PlayerPrefs.GetInt("level-index");

#if UNITY_EDITOR
        if (testLevelIndex >= 0)
            levelIndex = testLevelIndex;
#endif

        Generate(levelIndex);
    }

    private void Generate(int levelIndex)
    {
        for (int i = 0; i < bottleParent.childCount; i++)
            Destroy(bottleParent.GetChild(i).gameObject);

        var l = levels[levelIndex % levels.Length];

        for (int i = 0; i < l.bottles.Length; i++)
        {
            var u = Instantiate(bottlePrefab, bottleParent);

            var omis = l.bottles[i].ballMatIndices;

            u.SetSize(omis.Length);

            for (int n = 0; n < omis.Length; n++)
            {
                var o = Instantiate(ballPrefab, bottleParent);

                var omi = omis[n];

                o.SetMaterial(ballMaterials[omi]);
                o.SetId(omi);

                u.SetBall(o);
            }
        }

        this.DelayedAction(0.1f, () =>
        {
            cylinder.Retransform(1);
        });


        var d = Instantiate(ballPrefab, null);

        var dmi = l.firstDropBallMatIndex;

        d.SetMaterial(ballMaterials[dmi]);
        d.SetId(dmi);

        d.transform.position = new Vector3(0f, 10f, 0f);

        GlobalVar.Set<PuzzleBall>("drop-ball", d);

        levelText.text = "Level " + (levelIndex + 1);

        _activeLevel = levelIndex;
    }

    public void OnLevelWin()
    {
        PlayerPrefs.SetInt("level-index", _activeLevel + 1);
    }
}
