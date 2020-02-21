// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Security.Authentication;
using System.Runtime.CompilerServices;

namespace System.DirectoryServices.Protocols
{
    public partial class LdapSessionOptions
    {
        private int PALldap_get_option_secInfo(ConnectionHandle ldapHandle, LdapOption option, SecurityPackageContextConnectionInformation outValue) => Wldap32.ldap_get_option_secInfo(ldapHandle, option, outValue);

        private int PALldap_get_option_sechandle(ConnectionHandle ldapHandle, LdapOption option, ref SecurityHandle outValue) => Wldap32.ldap_get_option_sechandle(ldapHandle, option, ref outValue);

        private int PALldap_set_option_clientcert(ConnectionHandle ldapHandle, LdapOption option, QUERYCLIENTCERT outValue) => Wldap32.ldap_set_option_clientcert(ldapHandle, option, outValue);

        private int PALldap_set_option_servercert(ConnectionHandle ldapHandle, LdapOption option, VERIFYSERVERCERT outValue) => Wldap32.ldap_set_option_servercert(ldapHandle, option, outValue);

        private int PALldap_set_option_int(ConnectionHandle ld, LdapOption option, ref int inValue) => Wldap32.ldap_set_option_int(ld, option, ref inValue);

        private unsafe int PALldap_start_tls(ConnectionHandle ldapHandle, ref int ServerReturnValue, ref IntPtr Message, IntPtr ServerControls, IntPtr ClientControls) => Wldap32.ldap_start_tls(ldapHandle, ref ServerReturnValue, ref Message, ServerControls, ClientControls);

        private unsafe int PALldap_parse_result_referral(ConnectionHandle ldapHandle, IntPtr result, IntPtr serverError, IntPtr dn, IntPtr message, ref IntPtr referral, IntPtr control, byte freeIt)
        {
            int resultError = Wldap32.ldap_parse_result_referral(ldapHandle, result, serverError, dn, message, ref referral, control, freeIt);
            return resultError;
        }

        private static unsafe void PALldap_value_free(IntPtr referral) => Wldap32.ldap_value_free(referral);

        private byte PALLdap_stop_tls() => Wldap32.ldap_stop_tls(_connection._ldapHandle);

        private int PALldap_get_option_int(ConnectionHandle ldapHandle, LdapOption option, ref int outValue) => Wldap32.ldap_get_option_int(ldapHandle, option, ref outValue);

        private int PALldap_get_option_ptr(ConnectionHandle ldapHandle, LdapOption option, ref IntPtr outValue) => Wldap32.ldap_get_option_ptr(ldapHandle, option, ref outValue);

        private static void PALldap_memfree(IntPtr outValue) => Wldap32.ldap_memfree(outValue);

        private int PALldap_set_option_ptr(ConnectionHandle ldapHandle, LdapOption option, ref IntPtr inValue) => Wldap32.ldap_set_option_ptr(ldapHandle, option, ref inValue);

        private int PALldap_set_option_referral(ConnectionHandle ldapHandle, LdapOption option, ref LdapReferralCallback outValue) => Wldap32.ldap_set_option_referral(ldapHandle, option, ref outValue);

        private static void PALCertFreeCRLContext(IntPtr certPtr) => Wldap32.CertFreeCRLContext(certPtr);



        internal bool FQDN
        {
            set
            {
                // set the value to true
                SetIntValueHelper(LdapOption.LDAP_OPT_AREC_EXCLUSIVE, 1);
            }
        }

        public bool SecureSocketLayer
        {
            get
            {
                int outValue = GetIntValueHelper(LdapOption.LDAP_OPT_SSL);
                return outValue == 1;
            }
            set
            {
                int temp = value ? 1 : 0;
                SetIntValueHelper(LdapOption.LDAP_OPT_SSL, temp);
            }
        }

        private static string PtrToString(IntPtr pointer) => Marshal.PtrToStringUni(pointer);

        private static IntPtr StringToPtr(string value) => Marshal.StringToHGlobalUni(value);
    }
}
