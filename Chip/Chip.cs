///Copyright(c) 2015,HIT All rights reserved.
///Summary:Dll Chip
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Irlovan.Chip
{
    public class Chip : IChip
    {

        #region Structure

        /// <summary>
        /// Chips for local libraries
        /// </summary>
        public Chip(string dllPath, string namespaceName) {
            DllPath = dllPath;
            NameSpace = namespaceName;
            Init();
            LoadDll();
        }

        #endregion Structure

        #region Field

        // file type for chips
        internal const string FileType = "*.dll";

        #endregion Field

        #region Property

        /// <summary>
        /// NameSpace for chip
        /// </summary>
        public string NameSpace { get; internal set; }

        /// <summary>
        /// AssemblyList of chips
        /// </summary>
        public Dictionary<string, Assembly> AssemblyList { get; internal set; }

        /// <summary>
        /// DllPath for chips
        /// </summary>
        public string DllPath { get; internal set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Init
        /// </summary>
        public virtual void Init() {
            AssemblyList = new Dictionary<string, Assembly>();
        }

        /// <summary>
        /// load chips
        /// </summary>
        public virtual void LoadDll() {
            string dllPath = System.Environment.CurrentDirectory + DllPath;
            if (!Directory.Exists(dllPath)) { return; }
            DirectoryInfo chips = new DirectoryInfo(dllPath);
            FileInfo[] files = chips.GetFiles(FileType);
            foreach (FileInfo file in files) {
                LoadChip(file);
            }
        }

        /// <summary>
        /// Load Chip
        /// </summary>
        private void LoadChip(FileInfo file) {
            try {
                Assembly assem = Assembly.LoadFile(file.FullName);
                AppDomain.CurrentDomain.Load(assem.GetName());
                AssemblyList.Add(file.Name.Split('.')[0], assem);
            } catch (Exception) {
                return;
            }
        }

        #endregion Function

    }
}
