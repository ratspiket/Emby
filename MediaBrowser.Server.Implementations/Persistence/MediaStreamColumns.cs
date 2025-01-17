﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaBrowser.Model.Logging;

namespace MediaBrowser.Server.Implementations.Persistence
{
    public class MediaStreamColumns
    {
        private readonly IDbConnection _connection;
        private readonly ILogger _logger;

        public MediaStreamColumns(IDbConnection connection, ILogger logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public void AddColumns()
        {
            AddPixelFormatColumnCommand();
            AddBitDepthCommand();
            AddIsAnamorphicColumn();
            AddIsCabacColumn();
            AddKeyFramesColumn();
            AddRefFramesCommand();
            AddCodecTagColumn();
            AddCommentColumn();
        }

        private void AddCommentColumn()
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA table_info(mediastreams)";

                using (var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.SingleResult))
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(1))
                        {
                            var name = reader.GetString(1);

                            if (string.Equals(name, "Comment", StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                        }
                    }
                }
            }

            var builder = new StringBuilder();

            builder.AppendLine("alter table mediastreams");
            builder.AppendLine("add column Comment TEXT");

            _connection.RunQueries(new[] { builder.ToString() }, _logger);
        }

        private void AddCodecTagColumn()
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA table_info(mediastreams)";

                using (var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.SingleResult))
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(1))
                        {
                            var name = reader.GetString(1);

                            if (string.Equals(name, "CodecTag", StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                        }
                    }
                }
            }

            var builder = new StringBuilder();

            builder.AppendLine("alter table mediastreams");
            builder.AppendLine("add column CodecTag TEXT");

            _connection.RunQueries(new[] { builder.ToString() }, _logger);
        }

        private void AddPixelFormatColumnCommand()
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA table_info(mediastreams)";

                using (var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.SingleResult))
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(1))
                        {
                            var name = reader.GetString(1);

                            if (string.Equals(name, "PixelFormat", StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                        }
                    }
                }
            }

            var builder = new StringBuilder();

            builder.AppendLine("alter table mediastreams");
            builder.AppendLine("add column PixelFormat TEXT");

            _connection.RunQueries(new[] { builder.ToString() }, _logger);
        }

        private void AddBitDepthCommand()
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA table_info(mediastreams)";

                using (var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.SingleResult))
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(1))
                        {
                            var name = reader.GetString(1);

                            if (string.Equals(name, "BitDepth", StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                        }
                    }
                }
            }

            var builder = new StringBuilder();

            builder.AppendLine("alter table mediastreams");
            builder.AppendLine("add column BitDepth INT NULL");

            _connection.RunQueries(new[] { builder.ToString() }, _logger);
        }

        private void AddRefFramesCommand()
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA table_info(mediastreams)";

                using (var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.SingleResult))
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(1))
                        {
                            var name = reader.GetString(1);

                            if (string.Equals(name, "RefFrames", StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                        }
                    }
                }
            }

            var builder = new StringBuilder();

            builder.AppendLine("alter table mediastreams");
            builder.AppendLine("add column RefFrames INT NULL");

            _connection.RunQueries(new[] { builder.ToString() }, _logger);
        }

        private void AddIsCabacColumn()
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA table_info(mediastreams)";

                using (var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.SingleResult))
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(1))
                        {
                            var name = reader.GetString(1);

                            if (string.Equals(name, "IsCabac", StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                        }
                    }
                }
            }

            var builder = new StringBuilder();

            builder.AppendLine("alter table mediastreams");
            builder.AppendLine("add column IsCabac BIT NULL");

            _connection.RunQueries(new[] { builder.ToString() }, _logger);
        }

        private void AddKeyFramesColumn()
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA table_info(mediastreams)";

                using (var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.SingleResult))
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(1))
                        {
                            var name = reader.GetString(1);

                            if (string.Equals(name, "KeyFrames", StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                        }
                    }
                }
            }

            var builder = new StringBuilder();

            builder.AppendLine("alter table mediastreams");
            builder.AppendLine("add column KeyFrames TEXT NULL");

            _connection.RunQueries(new[] { builder.ToString() }, _logger);
        }

        private void AddIsAnamorphicColumn()
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA table_info(mediastreams)";

                using (var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.SingleResult))
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(1))
                        {
                            var name = reader.GetString(1);

                            if (string.Equals(name, "IsAnamorphic", StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                        }
                    }
                }
            }

            var builder = new StringBuilder();

            builder.AppendLine("alter table mediastreams");
            builder.AppendLine("add column IsAnamorphic BIT NULL");

            _connection.RunQueries(new[] { builder.ToString() }, _logger);
        }

    }
}
