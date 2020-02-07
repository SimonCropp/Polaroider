﻿using System.IO;

namespace Polaroider
{
    /// <summary>
    /// write snapshots to file
    /// </summary>
    public class SnapshotWriter : ISnapshotWriter
    {
        /// <summary>
        /// write the snapshot to file
        /// </summary>
        /// <param name="snapshot"></param>
        /// <param name="snapshotId"></param>
        public void Write(Snapshot snapshot, SnapshotId snapshotId)
        {
            var collection = new SnapshotCollection();
            collection.Add(snapshot);

            var file = snapshotId.GetFilePath();
            if (File.Exists(file))
            {
                var reader = new SnapshotReader();
                var tmp = reader.Read(snapshotId);

                foreach (var token in tmp)
                {
                    if (!token.SnapshotContainsMetadata(snapshot.Metadata))
                    {
                        collection.Add(token);
                    }
                }
            }

            Directory.CreateDirectory(Path.GetDirectoryName(snapshotId.GetFilePath()));

            using (var writer = new StreamWriter(snapshotId.GetFilePath(), false))
            {
                foreach(var token in collection)
                {
                    if (snapshot.HasMetadata())
                    {
                        writer.WriteLine("---metadata");
                    }

                    foreach (var info in token.Metadata)
                    {
                        writer.WriteLine($"{info.Key}: {info.Value}");
                    }

                    writer.WriteLine("---data");

                    foreach (var line in token)
                    {
                        writer.WriteLine(line.Value);
                    }
                }
            }
        }
    }
}
