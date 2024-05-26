using System.Collections;
using System.Collections.Generic;
using EasyTab;
using EasyTab.Internals;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RangeEnumeratorTests
{
    [Test]
    [TestCase(5, 0, 5, false, ExpectedResult = new[] { 0, 1, 2, 3, 4 })]
    [TestCase(5, 1, 5, false, ExpectedResult = new[] { 1, 2, 3, 4, 0 })]
    [TestCase(5, 4, 5, false, ExpectedResult = new[] { 4, 0, 1, 2, 3 })]
    // reverse
    [TestCase(5, 0, 5, true, ExpectedResult = new[] { 0, 4, 3, 2, 1 })]
    [TestCase(5, 1, 5, true, ExpectedResult = new[] { 1, 0, 4, 3, 2 })]
    [TestCase(5, 4, 5, true, ExpectedResult = new[] { 4, 3, 2, 1, 0 })]
    public int[] Test(int leng, int start, int count, bool reverse)
    {
        var l = new List<int>(count);
        var e = new RangeEnumerator(leng, start, count, reverse);

        while (e.MoveNext())
            l.Add(e.Current);

        return l.ToArray();
    }

    [Test]
    public void Test()
    {
        
    }
}