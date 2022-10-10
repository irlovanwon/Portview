///Copyright(c) 2015,HIT All rights reserved.
///Summary:Dll chip loader
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:
      
namespace Irlovan.Chip
{
    public class ChipLoader
    {

        #region Structure

        /// <summary>
        /// Dll chips
        /// </summary>
        public ChipLoader() {
            Driver = new Chip(DllPathValue_Driver, NameSpaceValue_Driver);
            Handler = new Chip(DllPathValue_Handler, NameSpaceValue_Handler);
            Script = new ScriptChipLoader();
            Recorder = new Chip(DllPathValue_Recorder, NameSpaceValue_Recorder);
            Notification = new Chip(DllPathValue_Notification, NameSpaceValue_Notification);
            /**************************************appear in the next version**************************************/
            //Compression = new Chip(DllPathValue_Compression, NameSpaceValue_Compression);
        }

        #endregion Structure

        #region Field

        private const string DllPathValue_Driver = "\\Driver\\";
        public const string NameSpaceValue_Driver = "Irlovan.Driver.";

        //private const string DllPathValue_Compression = "\\Compression\\";
        //public const string NameSpaceValue_Compression = "Irlovan.Compression.";

        private const string DllPathValue_Handler = "\\Handlers\\";
        public const string NameSpaceValue_Handler = "Irlovan.Handlers.";

        private const string DllPathValue_Recorder = "\\Recorder\\";
        public const string NameSpaceValue_Recorder = "Irlovan.Recorder.";

        private const string DllPathValue_Notification = "\\Notification\\";
        public const string NameSpaceValue_Notification = "Irlovan.Notification.";

        #endregion Field

        #region Property

        /// <summary>
        /// Driver Chip
        /// </summary>
        public Chip Driver { get; private set; }

        /// <summary>
        /// Handler Chip
        /// </summary>
        public Chip Handler { get; private set; }

        /// <summary>
        /// Compression Chip
        /// </summary>
        //public Chip Compression { get; private set; }

        /// <summary>
        /// Script Chip
        /// </summary>
        public ScriptChipLoader Script { get; private set; }

        /// <summary>
        /// Recorder Chip
        /// </summary>
        public Chip Recorder { get; private set; }

        /// <summary>
        /// Notification Chip
        /// </summary>
        public Chip Notification { get; private set; }

        #endregion Property

    }
}
