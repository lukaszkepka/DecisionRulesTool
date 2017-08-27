using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TestResult
{
	public virtual string[] decisionValues
	{
		get;
		set;
	}

	public virtual ConfusionMatrix ConfusionMatrix
	{
		get;
		set;
	}

	public virtual TestRequest TestRequest
	{
		get;
		set;
	}

}

