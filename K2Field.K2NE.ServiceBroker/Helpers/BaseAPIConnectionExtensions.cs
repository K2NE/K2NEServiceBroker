using System;
using System.Linq;
using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.Hosting.Server.Interfaces;

namespace K2Field.K2NE.ServiceBroker.Helpers
{
    /// <summary>
    /// BaseAPIConnection Extension methods
    /// </summary>
    internal static class BaseAPIConnectionExtensions
    {
        /// <summary>
        /// Aligns your expected user's context to the BaseAPIConnection
        /// </summary>
        /// <param name="connection">Your current BaseAPIConnection who's context will be alligned</param>
        /// <param name="k2Connection">The broker base.</param>
        internal static void ImpersonateSessionUser(this BaseAPIConnection connection, K2Connection k2Connection)
        {
            connection.ThrowIfNull(nameof(connection));
            k2Connection.ThrowIfNull(nameof(k2Connection));

            try
            {
                // Don't impersonate if the connection does not have a session cookie
                var sessionCookie = connection.GetPropertyValue("SessionCookie") as string;
                if (string.IsNullOrEmpty(sessionCookie))
                {
                    return;
                }

                // Don't impersonate if the session's FQN is the same as the expected user's FQN
                var activeSession = k2Connection.SessionManager.GetActiveSession(sessionCookie);
                if (string.Equals(k2Connection.UserName, activeSession?.Owner?.PrimaryCredential?.FullyQualifiedName?.FQN, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                // Don't impersonate if not integrated or connection has a password
                var builder = new SCConnectionStringBuilder(k2Connection.SessionConnectionString);
                if (!builder.Integrated ||
                    !string.IsNullOrEmpty(builder.Password))
                {
                    return;
                }

                //In A PTA scenario, you would have fallen back to the service account and we would have Authenticated you as the service account.
                //If we are in a PTA context we should call the ImpersonateSessionUserByName method to align this connetion's user to the expected user
                K2NEServiceBroker.SecurityManager.ImpersonateSessionUserByName(sessionCookie, k2Connection.UserName, ImpersonationType.PassThrough, true);
            }
            catch
            {
                connection?.Dispose();
                throw;
            }
        }
    }
}