namespace Client
{
    using Microsoft.Win32;
    using System;
    using System.IO;
    using System.Windows.Forms;

    public class FileManager
    {
        private string m_BasePath = "";
        private bool m_Error;
        private string m_FilePath = "";

        public FileManager()
        {
            this.m_BasePath = Application.StartupPath;
            this.m_FilePath = Engine.m_OverrideDataPath;
            if (this.m_FilePath == null)
            {
                this.m_FilePath = this.GetExePath("Ultima Online");
                if (this.m_FilePath == null)
                {
                    this.m_FilePath = this.GetExePath("Ultima Online Third Dawn");
                    if (this.m_FilePath == null)
                    {
                        OpenFileDialog dialog = new OpenFileDialog {
                            CheckPathExists = true,
                            CheckFileExists = false,
                            FileName = "Client.exe",
                            Filter = "Client.exe|Client.exe",
                            Title = "Find your UO directory",
                            InitialDirectory = Path.GetPathRoot(this.m_BasePath)
                        };
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            this.m_FilePath = Path.GetDirectoryName(dialog.FileName);
                        }
                        else
                        {
                            MessageBox.Show("Couldn't find UO directory.", "Client");
                        }
                        dialog.Dispose();
                    }
                }
                this.m_Error = this.m_FilePath == null;
            }
        }

        public string BasePath(string Path)
        {
            return Path.Combine(this.m_BasePath, Path);
        }

        public Stream CreateBaseMUL(string Path)
        {
            string path = Path.Combine(this.m_BasePath, Path);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            new FileInfo(path).Directory.Create();
            return File.Create(path);
        }

        public FileStream CreateUnique(string basePath, string extension)
        {
            string path = this.BasePath(string.Format("{0}{1}", basePath, extension));
            int num = 0;
            do
            {
                try
                {
                    return new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                }
                catch
                {
                    path = string.Format("{0}{1}{2}", basePath, ++num, extension);
                }
            }
            while (num < 0x3e8);
            throw new Exception(string.Format("Unable to create unique file (basePath='{0}', extension='{0}')", basePath, extension));
        }

        public void Dispose()
        {
        }

        private string GetExePath(string entry)
        {
            string directoryName;
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(string.Format(@"SOFTWARE\Origin Worlds Online\{0}\1.0", entry)))
                {
                    directoryName = Path.GetDirectoryName(key.GetValue("ExePath").ToString());
                }
            }
            catch
            {
                directoryName = null;
            }
            return directoryName;
        }

        public Stream OpenBaseMUL(string Path)
        {
            return this.OpenRead(Path.Combine(this.m_BasePath, Path));
        }

        public Stream OpenMUL(Files File)
        {
            return this.OpenRead(Path.Combine(this.m_FilePath, Config.GetFile((int) File)));
        }

        public Stream OpenMUL(string Path)
        {
            return this.OpenRead(Path.Combine(this.m_FilePath, Path));
        }

        protected Stream OpenRead(string Path)
        {
            return File.OpenRead(Path);
        }

        public string ResolveMUL(Files File)
        {
            return Path.Combine(this.m_FilePath, Config.GetFile((int) File));
        }

        public string ResolveMUL(string Path)
        {
            return Path.Combine(this.m_FilePath, Path);
        }

        public bool Error
        {
            get
            {
                return this.m_Error;
            }
        }

        public string FilePath
        {
            get
            {
                return this.m_FilePath;
            }
        }
    }
}

