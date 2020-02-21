// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.DirectoryServices.Protocols
{
    public partial class SortRequestControl : DirectoryControl
    {
        private static int PALLdap_Create_Sort_Control(ConnectionHandle handle, IntPtr keys, byte critical, ref IntPtr control) => Wldap32.ldap_create_sort_control(handle, keys, critical, ref control);

        private static void PALLdap_Control_Free(IntPtr control) => Wldap32.ldap_control_free(control);
    }
}
