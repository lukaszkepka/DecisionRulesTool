﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DecisionRulesTool.Model.Utils
{
    public sealed class ReferenceEqualityComparer : IEqualityComparer, IEqualityComparer<object>
    {
        public static readonly ReferenceEqualityComparer Default = new ReferenceEqualityComparer(); 

        public new bool Equals(object x, object y)
        {
            return x == y;
        }

        public int GetHashCode(object obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}
