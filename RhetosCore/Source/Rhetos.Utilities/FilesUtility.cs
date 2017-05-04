﻿/*
    Copyright (C) 2014 Omega software d.o.o.

    This file is part of Rhetos.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Rhetos.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Rhetos.Utilities
{
    public class FilesUtility
    {
        private readonly ILogger _logger;

        public FilesUtility(ILogProvider logProvider)
        {
            _logger = logProvider.GetLogger(GetType().Name);
        }

        private void Retry(Action action, Func<string> actionName)
        {
            const int maxTries = 10;
            for (int tries = maxTries; tries > 0; tries--)
            {
                try
                {
                    action();
                    break;
                }
                catch
                {
                    if (tries <= 1)
                        throw;

                    if (tries == maxTries - 1) // Logging the second retry instead of the first one, because first retries are too common.
                        _logger.Trace(() => "Waiting to " + actionName.Invoke() + ".");

                    if (Environment.UserInteractive)
                        System.Threading.Thread.Sleep(500);
                    continue;
                }
            }
        }

        public void SafeCreateDirectory(string path)
        {
            try
            {
                // When using TortoiseHg and the Rhetos folder is opened in Windows Explorer,
                // Directory.CreateDirectory() will stochastically fail with UnauthorizedAccessException or DirectoryNotFoundException.
                Retry(() => Directory.CreateDirectory(path), () => "create directory " + path);
            }
            catch (Exception ex)
            {
                throw new FrameworkException(String.Format("Can't create directory '{0}'. Check that it's not locked.", path), ex);
            }
        }

        public void SafeDeleteDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    return;

                File.SetAttributes(path, FileAttributes.Normal);

                foreach (var dir in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
                    File.SetAttributes(dir, FileAttributes.Normal);

                foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
                    File.SetAttributes(file, FileAttributes.Normal);

                Retry(() => Directory.Delete(path, true), () => "delete directory " + path);

                Retry(() => { if (Directory.Exists(path)) throw new FrameworkException("Failed to delete directory " + path); }, () => "check if directory deleted " + path);
            }
            catch (Exception ex)
            {
                throw new FrameworkException(String.Format("Can't delete directory '{0}'. Check that it's not locked.", path), ex);
            }
        }

        /// <summary>
        /// Creates the directory if it doesn't exists and deletes its content.
        /// This method will not delete the directory and create a new one; the existing directory is kept, in order to reduce locking issues if the folder is opened in command prompt or other application.
        /// </summary>
        public void EmptyDirectory(string path)
        {
            SafeCreateDirectory(path);

            foreach (var file in Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly))
            {
                File.SetAttributes(file, FileAttributes.Normal);
                Retry(() => File.Delete(file), () => "delete file " + file);
            }
            foreach (var folder in Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly))
                SafeDeleteDirectory(folder);
        }

        public void SafeMoveFile(string source, string destination)
        {
            try
            {
                SafeCreateDirectory(Path.GetDirectoryName(destination)); // Less problems with locked folders if the directory is created before moving the file. Locking may occur when using TortoiseHg and the Rhetos folder is opened in Windows Explorer.
                Retry(() => File.Move(source, destination), () => "move file " + source);
            }
            catch (Exception ex)
            {
                throw new FrameworkException(String.Format("Can't move file '{0}' to '{1}'. Check that destination file or folder is not locked.", source, destination), ex);
            }
        }

        public void SafeCopyFile(string source, string destination)
        {
            try
            {
                SafeCreateDirectory(Path.GetDirectoryName(destination));
                Retry(() => File.Copy(source, destination), () => "copy file " + source);
            }
            catch (Exception ex)
            {
                throw new FrameworkException(String.Format("Can't copy file '{0}' to '{1}'. Check that destination folder is not locked.", source, destination), ex);
            }
        }
    }
}
