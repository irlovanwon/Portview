///Copyright(c) 2015,HIT All rights reserved.
///Summary:For windows command line
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using System;
using System.Threading;

namespace Irlovan.Lib.CMD
{
    public static class CMD
    {

        /// <span class="code-SummaryComment"><summary></span>
        /// Execute the command Asynchronously.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="command">string command.</param></span>
        public static void ExecuteCommandAsync(string command) {
            try {
                //Asynchronously start the Thread to process the Execute command request.
                System.Threading.Thread objThread = new System.Threading.Thread(new ParameterizedThreadStart(ExecuteCommandSync));
                //Make the thread as background thread.
                objThread.IsBackground = true;
                //Set the Priority of the thread.
                objThread.Priority = ThreadPriority.AboveNormal;
                //Start the thread.
                objThread.Start(command);
            }
            catch (ThreadStartException objException) {
                ThreadStartException e = objException;
                // Log the exception
            }
            catch (ThreadAbortException objException) {
                ThreadAbortException e = objException;
                // Log the exception
            }
            catch (Exception objException) {
                Exception e = objException;
                // Log the exception
            }
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Executes a shell command synchronously.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="command">string command</param></span>
        /// <span class="code-SummaryComment"><returns>string, as output of the command.</returns></span>
        public static void ExecuteCommandSync(object command) {
            try {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                //Console.WriteLine(result);
            }
            catch (Exception objException) {
                Exception e = objException;
                // Log the exception
            }
        }

    }
}
