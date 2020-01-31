using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Polaroid.Tests
{
    public class SnapshotReaderTests
    {
        private SnapshotIdResolver _snapshotResolver;

        [SetUp]
        public void Setup()
        {
            _snapshotResolver = new SnapshotIdResolver();
        }

        [Test]
        public void SnapshotReader_Read()
        {
            var reader = new SnapshotReader();
            var snapshot = reader.Read(_snapshotResolver.ResloveId());

            snapshot.Should().NotBeNull();
            snapshot.Single().ToString().Should().Be("test\r\nvalue");
        }

        [Test]
        public void SnapshotReader_Metadata()
        {
            var reader = new SnapshotReader();
            var snapshot = reader.Read(_snapshotResolver.ResloveId());

            snapshot.Should().NotBeNull();
            snapshot.Single().Metadata.Count.Should().Be(2);
            snapshot.Single().Metadata["option"].Should().Be("1");
            snapshot.Single().Metadata["datatype"].Should().Be("string");

            snapshot.Single().ToString().Should().Be("test\r\nmetadata");
        }

        [Test]
        public void SnapshotReader_WithSettings()
        {
            var reader = new SnapshotReader();
            var snapshot = reader.Read(_snapshotResolver.ResloveId());

            snapshot.Should().NotBeNull();
            snapshot.Single().ToString().Should().Be("test\r\nsettings");
        }

        [Test]
        public void SnapshotReader_Multifile()
        {
            var reader = new SnapshotReader();
            var snapshot = reader.Read(_snapshotResolver.ResloveId());

            snapshot.Count().Should().Be(3);

            var shots = snapshot.ToArray();

            for (var i = 1; i <= 3; i++)
            {
                shots[i-1].Metadata.Count.Should().Be(1);
                shots[i-1].Metadata["snapshot"].Should().Be($"{i}");
                shots[i-1].ToString().TrimEnd().Should().Be($"test\r\n{i}");
            }
        }
    }
}