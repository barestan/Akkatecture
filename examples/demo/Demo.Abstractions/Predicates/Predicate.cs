﻿using System;
using System.Collections.Generic;

namespace Demo.Predicates
{
    public abstract class Predicate { }

    public abstract class Predicate<TLeft, TRight, TOperator> : Predicate
        where TOperator : Enum
    {
        private readonly IDictionary<TOperator, Func<TLeft, TRight, Boolean>> _functions;

        protected Predicate()
        {
            _functions = new Dictionary<TOperator, Func<TLeft, TRight, Boolean>>();
        }

        public TOperator Operator { get; set; }
        public TRight Value { get; set; }

        public Boolean Check(TLeft left)
        {
            if (_functions.ContainsKey(Operator) == false)
                throw new NotImplementedException($"Operator {Operator.ToString()} does not implemented");
            return _functions[Operator](left, Value);
        }

        protected void Register(TOperator @operator, Func<TLeft, TRight, Boolean> function)
        {
            if (_functions.ContainsKey(@operator))
                throw new Exception($"Function  for operator {@operator} already exists");

            _functions.Add(@operator, function);
        }
    }
}
