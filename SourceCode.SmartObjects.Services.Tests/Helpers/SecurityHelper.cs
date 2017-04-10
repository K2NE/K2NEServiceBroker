using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;

namespace SourceCode.SmartObjects.Services.Tests.Helpers
{
    /// <summary>
    /// Helper class for security specific code.
    /// </summary>
    public static class SecurityHelper
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        public static void InvokeAsUser(Action action, string userName, string password)
        {
            // Strip-off Domain Name
            userName = userName.Split('\\').Last();

            var currentUserName = WindowsIdentity.GetCurrent().Name;
            var userDomainName = $"{Environment.UserDomainName}\\{userName}";

            if (userDomainName.Equals(currentUserName, StringComparison.InvariantCultureIgnoreCase))
            {
                action();
            }
            else
            {
                SafeTokenHandle safeTokenHandle;

                const int LOGON32_PROVIDER_DEFAULT = 0;

                //This parameter causes LogonUser to create a primary token.
                const int LOGON32_LOGON_INTERACTIVE = 2;

                // Call LogonUser to obtain a handle to an access token.
                bool returnValue = LogonUser(userName, Environment.UserDomainName, password,
                    LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                    out safeTokenHandle);

                if (false == returnValue)
                {
                    int ret = Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(ret);
                }

                // Use the token handle returned by LogonUser.
                using (safeTokenHandle)
                using (WindowsIdentity newId = new WindowsIdentity(safeTokenHandle.DangerousGetHandle()))
                using (WindowsImpersonationContext impersonatedUser = newId.Impersonate())
                {
                    Debug.WriteLine("User: " + WindowsIdentity.GetCurrent().Name);

                    action();
                }
            }
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);
    }

    public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeTokenHandle()
            : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            return CloseHandle(handle);
        }

        [DllImport("kernel32.dll")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr handle);
    }
}