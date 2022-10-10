///Copyright(c) 2015,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2015-08-10
///Description:
///Modification:

using System;
using System.Collections.Generic;

namespace Irlovan.Structure
{
    public class FileLayer : Layer, IFileLayer
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="clockwork"></param>
        /// <param name="index"></param>
        public FileLayer(string name, Clockwork clockwork, int index, ILayer parent)
            : base(name, clockwork, index, parent) { }

        #endregion Structure

        #region Function

        /// <summary>
        /// Init
        /// </summary>
        public override void Init() {
            base.Init();
            Path = Path + Clockwork.FileNameExtention;
        }

        /// <summary>
        /// If the layer exists
        /// </summary>
        /// <returns></returns>
        public override bool Exist() {
            return System.IO.File.Exists(Path);
        }

        /// <summary>
        /// Delete the layer
        /// </summary>
        /// <returns></returns>
        public override bool Delete() {
            try {
                System.IO.File.Delete(Path);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Append All Lines
        /// </summary>
        /// <returns></returns>
        public override bool AppendAllLines(DateTime timeStamp, IEnumerable<string> lines) {
            try {
                System.IO.File.AppendAllLines(Path, lines);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Append All Text
        /// </summary>
        /// <returns></returns>
        public override bool AppendAllText(DateTime timeStamp, string text) {
            try {
                System.IO.File.AppendAllText(Path, text);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Read All Lines
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public override IEnumerable<string> ReadAllLines(DateTime timeStamp) {
            try {
                return System.IO.File.ReadAllLines(Path);
            }
            catch (Exception) {
                return null;
            }
        }

        /// <summary>
        /// Read All Text
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public override string ReadAllText(DateTime timeStamp) {
            try {
                return System.IO.File.ReadAllText(Path);
            }
            catch (Exception) {
                return null;
            }
        }

        #endregion Function

    }
}
