using System.Linq;
using System.Collections.Generic;
using DecisionRulesTool.Model.RuleFilters.Appliers;
using DecisionRulesTool.Model.RuleFilters;
using PropertyChanged;

namespace DecisionRulesTool.Model.Model
{
    [AddINotifyPropertyChangedInterface]
    public class RuleSetSubset : RuleSet
    {
        protected RuleSetSubset rootRuleSet;
        protected List<IRuleFilter> ruleFilters;

        public string FiltersInfo { get; set; }
        public string FiltersShortInfo { get; set; }
        public RuleSetSubset RootRuleSet
        {
            get
            {
                return rootRuleSet ?? (this);
            }
            private set
            {
                rootRuleSet = value;
            }
        }
        public RuleSetSubset InitialRuleSet { get; protected set; }
        public IList<RuleSetSubset> Subsets { get; protected set; }
        public IEnumerable<IRuleFilter> Filters => ruleFilters;

        //
        public IList<IRuleFilterApplier> FilterAppliers;

        public override string Name { get; set; }

        public string DisplayAs
        {
            get
            {
                return $"{Name}: {Filters.LastOrDefault()?.ToString()} [{Rules.Count}]";
            }
        }

        public void Initialize()
        {
            if (ruleFilters == null)
            {
                ruleFilters = new List<IRuleFilter>();
            }

            if (Subsets == null)
            {
                Subsets = new List<RuleSetSubset>();
            }

            if (FilterAppliers == null)
            {
                FilterAppliers = new List<IRuleFilterApplier>();
            }
        }

        public RuleSetSubset(string name) : base(name)
        {
            Initialize();
        }

        /// <summary>
        /// Constructor used for filling basic RuleSet class fields
        /// </summary>
        /// <param name="ruleSet">RuleSet used for initializing fields</param>
        public RuleSetSubset(RuleSet ruleSet) : base(ruleSet.Name, ruleSet.Attributes, ruleSet.Rules, ruleSet.DecisionAttribute)
        {
            Initialize();
        }

        public string GetShortFiltersInfo()
        {
            if (ruleFilters.Any())
            {
                return ruleFilters.Select(x => x.GetShortName()).Aggregate((x, y) => $"{x}_{y}");
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Constructor that creates new subset as child of initialRuleSet.
        /// Field InitialSubset is set to initialRuleSet and RootRuleSet is
        /// set to initialRuleSet rootRuleSet
        /// </summary>
        /// <param name="initialRuleSet"></param>
        public RuleSetSubset(RuleSetSubset initialRuleSet) : this(initialRuleSet, initialRuleSet.RootRuleSet)
        {
            Initialize();
            InitialRuleSet = initialRuleSet;
            RootRuleSet = initialRuleSet.RootRuleSet;

            this.ruleFilters.AddRange(initialRuleSet.Filters);
        }

        public RuleSetSubset(RuleSetSubset initialRuleSet, RuleSetSubset rootRuleSet) : base(initialRuleSet.Name, initialRuleSet.Attributes, initialRuleSet.Rules, initialRuleSet.DecisionAttribute)
        {
            Initialize();
            InitialRuleSet = initialRuleSet;
            RootRuleSet = rootRuleSet;
        }

        public void RemoveFilter(int index)
        {
            ruleFilters.RemoveAt(index);
        }

        public void AddFilter(IRuleFilter ruleFilter)
        {
            ruleFilters.Add(ruleFilter);
        }

        public void ApplyFilters()
        {
            RuleSet filteredRuleSet = null;

            foreach (IRuleFilter filter in ruleFilters)
            {
                filteredRuleSet = filter.FilterRules(this);
                if (filteredRuleSet.Rules.Count == 0)
                {
                    break;
                }
            }

            this.FiltersShortInfo = GetShortFiltersInfo();
            this.FiltersInfo = ruleFilters.Select(x => x.ToString()).Aggregate((x, y) => $"{x}, {y}");
            this.Attributes = filteredRuleSet.Attributes;
            this.Rules = filteredRuleSet.Rules;
        }

        public override object Clone()
        {
            return new RuleSetSubset(InitialRuleSet, RootRuleSet)
            {
                Rules = this.Rules,
                Subsets = this.Subsets,
                ruleFilters = this.ruleFilters,
            };
        }
    }
}
