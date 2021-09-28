﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BenchmarkChoco
{
    public sealed class ConfidenceCollection : IEnumerable<ConfidenceRecord>
    {
        private readonly List<ConfidenceRecord> _items = new List<ConfidenceRecord>();

        internal ConfidenceCollection()
        {
        }

        public bool IsEmpty
        {
            get {
                return _items.Count == 0;
            }
        }

        public IEnumerable<ConfidenceRecord> ConfidenceParts
        {
            get {
                return _items;
            }
        }

        public ConfidenceLevel GetConfidence()
        {
            if (_items.Count < 1)
                return ConfidenceLevel.Unknown;

            var result = GetRawConfidence();

            if (result < 0)
                return ConfidenceLevel.Bad;
            if (result < 2)
                return ConfidenceLevel.Questionable;
            if (result < 5)
                return ConfidenceLevel.Good;
            return ConfidenceLevel.VeryGood;
        }

        // Returns a number representing the confidence. 0 is a mid-point.
        public int GetRawConfidence()
        {
            var result = 0;
            _items.ForEach(x => result += x.Change);
            return result;
        }

        public string GetWorstOffender()
        {
            var lowest = _items.Min(x => x.Change);
            var item = _items.Find(x => x.Change.Equals(lowest));

            if (item != null) return item.Reason ?? string.Empty;
            return string.Empty;
        }

        internal void Add(int value)
        {
            _items.Add(new ConfidenceRecord(value));
        }

        internal void Add(ConfidenceRecord value)
        {
            _items.Add(value);
        }

        internal void AddRange(IEnumerable<ConfidenceRecord> values)
        {
            _items.AddRange(values.Where(x => !_items.Contains(x)));
        }

        public IEnumerator<ConfidenceRecord> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_items).GetEnumerator();
        }
    }

    public enum ConfidenceLevel
    {
        Unknown = 0,
        Bad = 5,
        Questionable = 7,
        Good = 9,
        VeryGood = 12
    }
}