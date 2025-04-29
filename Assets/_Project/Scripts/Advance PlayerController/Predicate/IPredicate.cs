using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityUtils
{
    public interface IPredicate
    {
        bool Evaluate();
    }

    public class And : IPredicate
    {
        [SerializeField] List<IPredicate> rules = new List<IPredicate>();
        public bool Evaluate() => rules.All(r => r.Evaluate());
    }

    public class Or : IPredicate
    {
        [SerializeField] List<IPredicate> rules = new List<IPredicate>();
        public bool Evaluate() => rules.Any(r => r.Evaluate());
    }

    //public class Not : IPredicate
    //{
    //    [SerializeField, FormerlySerializedAs("rule")] IPredicate rule;
    //    public bool Evaluate() => !rule.Evaluate();
    //}

    public class Not : IPredicate
    {
        [SerializeField] IPredicate rule;
        public bool Evaluate() => !rule.Evaluate();
    }
}