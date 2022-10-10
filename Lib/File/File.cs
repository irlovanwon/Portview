///Copyright(c) 2015,HIT All rights reserved.
///Summary:File Helper
///Author:Irlovan
///Date:2015-03-22
///Description:
///Modification:


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Irlovan.Lib.File
{
    public static class File
    {

        /// <summary>
        /// Get all files in a folder with search pattern including child folder
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        //public static string[] GetFiles(string path, string searchPattern) {
        //    string[] result = Directory.GetFiles(path, searchPattern);
        //    try {
        //        string[] folders = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
        //        foreach (var item in folders) {
        //            string[] filePaths = Directory.GetFiles(item, searchPattern);
        //            result = Array.Helper.Combin<string>(result, filePaths);
        //        }
        //    } catch (Exception e) {
        //        Global.Info.LogRecorder.Log(LogLevelEnum.Error, e.ToString());
        //        return null;
        //    }
        //    return result;
        //}
    }
}
