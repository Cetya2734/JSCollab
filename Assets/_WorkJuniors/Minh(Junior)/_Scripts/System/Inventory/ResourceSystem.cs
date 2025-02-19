using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class ResourceSystem : StaticInstance<ResourceSystem> {
        public List<GameObject> ExampleHeroes { get; private set; }
        private Dictionary<GameObject, GameObject> _ExampleHeroesDict;

        protected override void Awake() {
            base.Awake();
            AssembleResources();
        }

        private void AssembleResources() {
            ExampleHeroes = Resources.LoadAll<GameObject>("ExampleHeroes").ToList();
            //_ExampleHeroesDict = ExampleHeroes.ToDictionary(r => r.HeroType, r => r);
        }

        // public ScriptableExampleHero GetExampleHero(ExampleHeroType t) => _ExampleHeroesDict[t];
        // public ScriptableExampleHero GetRandomHero() => ExampleHeroes[Random.Range(0, ExampleHeroes.Count)];
    }   