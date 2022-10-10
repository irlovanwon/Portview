///Copyright(c) 2015,HIT All rights reserved.
///Summary:Register
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using System;
using System.IO;

namespace Irlovan.LocalInterface
{
    public class Register : IDisposable
    {

        #region Structure

        /// <summary>
        /// Register
        /// </summary>
        public Register() {
            Init();
        }

        #endregion Structure

        #region Field

        private const string RecorderFilePath = "\\Register\\";
        private const string FileName = "RAM";

        #endregion Field

        #region Property

        /// <summary>
        /// System Register
        /// </summary>
        internal Irlovan.Register.Register SysRegister { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Init
        /// </summary>
        private void Init() {
            string registerPath = System.Environment.CurrentDirectory + RecorderFilePath + FileName;
            if (!File.Exists(registerPath)) { return; }
            SysRegister = new Irlovan.Register.Register(System.Environment.CurrentDirectory + RecorderFilePath, FileName);
            if (!SysRegister.InitState) { SysRegister.Dispose(); }
            if (SysRegister.InitFromHD()) { return; }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() {
            if (SysRegister == null) { return; }
            SysRegister.Dispose();
        }

        #endregion Function

    }
}
