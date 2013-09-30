using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.ProgressReporting
{
    public sealed class ProgressFullInfo : IEnumerable<ScopeProgress>
    {
        private readonly List<ScopeProgress> _scopeProgresses;

        public double Progress
        {
            get { return _scopeProgresses[0].Progress; }
        }

        public string Name
        {
            get { return _scopeProgresses[0].Name; }
        }

        public ProgressFullInfo(IEnumerable<ScopeProgress> scopeProgresses)
        {
            _scopeProgresses = scopeProgresses.ToList();

            if (_scopeProgresses.Count == 0)
                throw new ArgumentException("Should be at least one scope progress.", "scopeProgresses");
        }

        public IEnumerator<ScopeProgress> GetEnumerator()
        {
            return _scopeProgresses.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}