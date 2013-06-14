using System.Collections.Generic;

namespace WallpaperGenerator.Formulas.FormulaTreeGeneration
{        
    public class OperatorGuard
    {
        public FormulaTreeNodeWrapper RootWrapper { get; private set; }

        public IDictionary<int, FormulaTreeNodeWrapper> ChildrenWrappers { get; private set; }

        public OperatorGuard(FormulaTreeNodeWrapper rootWrapper)
            : this (rootWrapper, null)
        {
        }

        public OperatorGuard(FormulaTreeNodeWrapper rootWrapper,
                             IDictionary<int, FormulaTreeNodeWrapper> childrenWrappers)
        {
            RootWrapper = rootWrapper;
            ChildrenWrappers = childrenWrappers;
        }
    }
}
