using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RefuseConflict : IConflictResolver
{
	public virtual ConflictResolvingMethod resolvingMethod
	{
		get;
		set;
	}

	public virtual void Resolve()
	{
		throw new System.NotImplementedException();
	}

}

