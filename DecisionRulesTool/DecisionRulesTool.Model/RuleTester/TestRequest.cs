using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TestRequest
{
	public virtual RuleSet ruleSet
	{
		get;
		set;
	}

	public virtual DataSet testSet
	{
		get;
		set;
	}

	public virtual ConflictResolvingMethod resolvingMethod
	{
		get;
		set;
	}

}

