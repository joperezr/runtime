// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Net;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace System.DirectoryServices.Protocols
{
    public partial class LdapConnection
    {
        private void InternalInitConnectionHandle(string hostname)
        {
            OpenLDAP.ldap_initialize(out IntPtr ldapServerHandle, $"ldap://{hostname}:{((LdapDirectoryIdentifier)_directoryIdentifier).PortNumber}/");
            _ldapHandle = new ConnectionHandle(ldapServerHandle, _needDispose);
        }

        private int InternalConnectToServer()
        {
            Debug.Assert(!_ldapHandle.IsInvalid);
            // In Linux you don't have to call Connect after calling init. You directly call bind.
            return 0;
        }

        private int InternalBind(NetworkCredential tempCredential, SEC_WINNT_AUTH_IDENTITY_EX cred, BindMethod method)
        {
            int error;
            if (tempCredential == null && AuthType == AuthType.External)
            {
                error = OpenLDAP.ldap_simple_bind(_ldapHandle, null, null);
            }
            else
            {
                error = OpenLDAP.ldap_simple_bind(_ldapHandle, cred.user, cred.password);
                if (error != 0x00)
                {
                    IntPtr mssg = IntPtr.Zero;
                    OpenLDAP.ldap_get_option_ptr(_ldapHandle, LdapOption.LDAP_OPT_ERROR_STRING, ref mssg);
                    string message = Marshal.PtrToStringAnsi(mssg);
                    throw new LdapException(message);
                }
            }

            return error;
        }

        private void PALldap_abandon(ConnectionHandle ldapHandle, int messagId) => OpenLDAP.ldap_abandon(ldapHandle, messagId);

        private int PALldap_delete_ext(ConnectionHandle ldapHandle, string dn, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber) => OpenLDAP.ldap_delete_ext(ldapHandle, dn, servercontrol, clientcontrol, ref messageNumber);

        private int PALldap_rename(ConnectionHandle ldapHandle, string dn, string newRdn, string newParentDn, int deleteOldRdn, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber) =>
                                OpenLDAP.ldap_rename(ldapHandle, dn, newRdn, newParentDn, deleteOldRdn, servercontrol, clientcontrol, ref messageNumber);

        private int PALldap_compare(ConnectionHandle ldapHandle, string dn, string attributeName, string strValue, berval binaryValue, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber) =>
                                OpenLDAP.ldap_compare(ldapHandle, dn, attributeName, binaryValue, servercontrol, clientcontrol, ref messageNumber);

        private int PALldap_add(ConnectionHandle ldapHandle, string dn, IntPtr attrs, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber) =>
                                OpenLDAP.ldap_add(ldapHandle, dn, attrs, servercontrol, clientcontrol, ref messageNumber);

        private int PALldap_modify(ConnectionHandle ldapHandle, string dn, IntPtr attrs, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber) =>
                                OpenLDAP.ldap_modify(ldapHandle, dn, attrs, servercontrol, clientcontrol, ref messageNumber);

        private int PALldap_extended_operation(ConnectionHandle ldapHandle, string oid, berval data, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber) =>
                                OpenLDAP.ldap_extended_operation(ldapHandle, oid, data, servercontrol, clientcontrol, ref messageNumber);

        private int PALldap_search(ConnectionHandle ldapHandle, string dn, int scope, string filter, IntPtr attributes, bool attributeOnly, IntPtr servercontrol, IntPtr clientcontrol, int timelimit, int sizelimit, ref int messageNumber) =>
                                OpenLDAP.ldap_search(ldapHandle, dn, scope, filter, attributes, attributeOnly, servercontrol, clientcontrol, timelimit, sizelimit, ref messageNumber);

        private int PALldap_set_option_clientcert(ConnectionHandle ldapHandle, LdapOption option, QUERYCLIENTCERT outValue) => OpenLDAP.ldap_set_option_clientcert(ldapHandle, option, outValue);

        private int PALldap_simple_bind_s(ConnectionHandle ld, string who, string passwd) => OpenLDAP.ldap_simple_bind(ld, who, passwd);

        private int PALldap_result(ConnectionHandle ldapHandle, int messageId, int all, LDAP_TIMEVAL timeout, ref IntPtr Message) => OpenLDAP.ldap_result(ldapHandle, messageId, all, timeout, ref Message);

        private int PALldap_parse_extended_result(ConnectionHandle ldapHandle, IntPtr result, ref IntPtr oid, ref IntPtr data, byte freeIt) => OpenLDAP.ldap_parse_extended_result(ldapHandle, result, ref oid, ref data, freeIt);

        private IntPtr PALldap_first_entry(ConnectionHandle ldapHandle, IntPtr result) => OpenLDAP.ldap_first_entry(ldapHandle, result);

        private IntPtr PALldap_next_entry(ConnectionHandle ldapHandle, IntPtr result) => OpenLDAP.ldap_next_entry(ldapHandle, result);

        private IntPtr PALldap_first_reference(ConnectionHandle ldapHandle, IntPtr result) => OpenLDAP.ldap_first_reference(ldapHandle, result);

        private IntPtr PALldap_next_reference(ConnectionHandle ldapHandle, IntPtr result) => OpenLDAP.ldap_next_reference(ldapHandle, result);

        private static void PALldap_memfree(IntPtr value) => OpenLDAP.ldap_memfree(value);

        private unsafe int PALldap_parse_result(ConnectionHandle ldapHandle, IntPtr result, ref int serverError, ref IntPtr dn, ref IntPtr message, ref IntPtr referral, ref IntPtr control, byte freeIt) =>
                                OpenLDAP.ldap_parse_result(ldapHandle, result, ref serverError, ref dn, ref message, ref referral, ref control, freeIt);

        private unsafe int PALldap_result2error(ConnectionHandle ldapHandle, IntPtr result, int freeIt) => OpenLDAP.ldap_result2error(ldapHandle, result, freeIt);

        private static unsafe void PALldap_value_free(IntPtr referral) => OpenLDAP.ldap_value_free(referral);

        private static unsafe void PALldap_controls_free(IntPtr value) => OpenLDAP.ldap_controls_free(value);

        private IntPtr PALldap_get_dn(ConnectionHandle ldapHandle, IntPtr result) => OpenLDAP.ldap_get_dn(ldapHandle, result);

        private IntPtr PALldap_first_attribute(ConnectionHandle ldapHandle, IntPtr result, ref IntPtr address) => OpenLDAP.ldap_first_attribute(ldapHandle, result, ref address);

        private IntPtr PALldap_next_attribute(ConnectionHandle ldapHandle, IntPtr result, IntPtr address) => OpenLDAP.ldap_next_attribute(ldapHandle, result, address);

        private static void PALldap_ber_free(IntPtr berelement, int option) => OpenLDAP.ber_free(berelement, option);

        private IntPtr PALldap_get_values_len(ConnectionHandle ldapHandle, IntPtr result, string name) => OpenLDAP.ldap_get_values_len(ldapHandle, result, name);

        private static void PALldap_value_free_len(IntPtr berelement) => OpenLDAP.ldap_value_free_len(berelement);

        private int PALldap_parse_reference(ConnectionHandle ldapHandle, IntPtr result, ref IntPtr referrals)
        {
            return OpenLDAP.ldap_parse_reference(ldapHandle, result, ref referrals, IntPtr.Zero, 0);
        }

        private int PALLdapGetLastError()
        {
            int result = 0;
            OpenLDAP.ldap_get_option_int(_ldapHandle, LdapOption.LDAP_OPT_ERROR_NUMBER, ref result);
            return result;
        }

        private static string PtrToString(IntPtr requestName) => Marshal.PtrToStringAnsi(requestName);

        private IntPtr StringToPtr(string s) => Marshal.StringToHGlobalAnsi(s);
    }
}
