// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.DirectoryServices.Protocols
{
    internal partial class LdapPartialResultsProcessor
    {
        private static void PALLdap_Abandon(ConnectionHandle ldapHandle, int messagId) => Wldap32.ldap_abandon(ldapHandle, messagId);
    }
}
