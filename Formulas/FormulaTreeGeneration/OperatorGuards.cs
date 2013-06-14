using System;
using System.Collections.Generic;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.Formulas.FormulaTreeGeneration
{
    public class OperatorGuards
    {
        public FormulaTreeNodeWrapper[] RootWrappers { get; private set; }

        public IDictionary<int, FormulaTreeNodeWrapper[]> ChildrenWrappers { get; private set; }

        public OperatorGuards(FormulaTreeNodeWrapper[] rootWrappers)
            : this(rootWrappers, null)
        {
        }

        public OperatorGuards(FormulaTreeNodeWrapper[] rootWrappers,
                              IDictionary<int, FormulaTreeNodeWrapper[]> childrenWrappers)
        {
            RootWrappers = rootWrappers;
            ChildrenWrappers = childrenWrappers;
        }

        public OperatorGuard GetRandom(Random random)
        {
            FormulaTreeNodeWrapper rootWrapper = RootWrappers != null && RootWrappers.Length > 0
                ? RootWrappers.TakeRandom(random)
                : null;

            IDictionary<int, FormulaTreeNodeWrapper> childrenWrappers = null;
            if (ChildrenWrappers != null)
            {
                childrenWrappers = new Dictionary<int, FormulaTreeNodeWrapper>();
                foreach (int childIndex in ChildrenWrappers.Keys)
                {
                    childrenWrappers.Add(childIndex, ChildrenWrappers[childIndex].TakeRandom(random));
                }
            }

            return new OperatorGuard(rootWrapper, childrenWrappers);
        }
    }
}
