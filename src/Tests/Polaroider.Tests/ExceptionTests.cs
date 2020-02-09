﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Polaroider.Tests
{
    public class ExceptionTests
    {
        [Test]
        public void MismatchException()
        {
            try
            {
                "this\r\nis\r\na invalid string".MatchSnapshot();
            }
            catch (SnapshotMatchException e)
            {
                e.Message.Should().Be(string.Join(Environment.NewLine, 
                    "",
                    "Snapshots do not match at Line 3", 
                    " - a valid string", 
                    " + a invalid string",
                    "",
                    "Strings differ at Index 2",
                    " - a valid string",
                    " + a invalid string"));

                return;
            }

            Assert.Fail("this should not be reached");
        }
    }
}
