// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
    public partial class LdapSessionOptions
    {
        // This option is not supported on Linux, so it would most likely throw.
        private int PALldap_get_option_secInfo(ConnectionHandle ldapHandle, LdapOption option, SecurityPackageContextConnectionInformation outValue) => OpenLDAP.ldap_get_option_secInfo(ldapHandle, option, outValue);

        private int PALldap_get_option_sechandle(ConnectionHandle ldapHandle, LdapOption option, ref SecurityHandle outValue) => OpenLDAP.ldap_get_option_sechandle(ldapHandle, option, ref outValue);

        // This option is not supported in Linux, so it would most likely throw.
        private int PALldap_set_option_clientcert(ConnectionHandle ldapHandle, LdapOption option, QUERYCLIENTCERT outValue) => OpenLDAP.ldap_set_option_clientcert(ldapHandle, option, outValue);

        // This option is not supported in Linux, so it would most likely throw.
        private int PALldap_set_option_servercert(ConnectionHandle ldapHandle, LdapOption option, VERIFYSERVERCERT outValue) => OpenLDAP.ldap_set_option_servercert(ldapHandle, option, outValue);

        private int PALldap_set_option_int(ConnectionHandle ld, LdapOption option, ref int inValue) => OpenLDAP.ldap_set_option_int(ld, option, ref inValue);

        private unsafe int PALldap_start_tls(ConnectionHandle ldapHandle, ref int ServerReturnValue, ref IntPtr Message, IntPtr ServerControls, IntPtr ClientControls) => OpenLDAP.ldap_start_tls(ldapHandle, ref ServerReturnValue, ref Message, ServerControls, ClientControls);

        private unsafe int PALldap_parse_result_referral(ConnectionHandle ldapHandle, IntPtr result, IntPtr serverError, IntPtr dn, IntPtr message, ref IntPtr referral, IntPtr control, byte freeIt)
        {
            int resultError = OpenLDAP.ldap_parse_result_referral(ldapHandle, result, serverError, dn, message, ref referral, control, freeIt);
            return resultError;
        }

        private static unsafe void PALldap_value_free(IntPtr referral) => OpenLDAP.ldap_value_free(referral);

        // openldap doesn't have a ldap_stop_tls function. Returning true as no-op for Linux.
        private byte PALLdap_stop_tls() => 1;

        private int PALldap_get_option_int(ConnectionHandle ldapHandle, LdapOption option, ref int outValue) => OpenLDAP.ldap_get_option_int(ldapHandle, option, ref outValue);

        private int PALldap_get_option_ptr(ConnectionHandle ldapHandle, LdapOption option, ref IntPtr outValue) => OpenLDAP.ldap_get_option_ptr(ldapHandle, option, ref outValue);

        private static void PALldap_memfree(IntPtr outValue) => OpenLDAP.ldap_memfree(outValue);

        private int PALldap_set_option_ptr(ConnectionHandle ldapHandle, LdapOption option, ref IntPtr inValue) => OpenLDAP.ldap_set_option_ptr(ldapHandle, option, ref inValue);

        private int PALldap_set_option_referral(ConnectionHandle ldapHandle, LdapOption option, ref LdapReferralCallback outValue) => OpenLDAP.ldap_set_option_referral(ldapHandle, option, ref outValue);

        private static void PALCertFreeCRLContext(IntPtr certPtr) { /* No op */ }


        // Options that are not supported in Linux

        internal bool FQDN
        {
            set
            {
                // no op
            }
        }

        public bool SecureSocketLayer
        {
            get
            {
                return false;
            }
            set
            {
                // no op since it is not supported in Linux
            }
        }

        private static string PtrToString(IntPtr pointer) => Marshal.PtrToStringAnsi(pointer);

        private static IntPtr StringToPtr(string value) => Marshal.StringToHGlobalAnsi(value);
    }
}
