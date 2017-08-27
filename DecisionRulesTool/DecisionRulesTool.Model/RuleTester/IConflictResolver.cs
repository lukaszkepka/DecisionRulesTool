using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IConflictResolver
{
    ConflictResolvingMethod resolvingMethod { get; set; }

    void Resolve();

}

