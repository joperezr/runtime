// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.DirectoryServices.Protocols
{
    internal static class NativePal
    {
        public static void BerBvecfree(IntPtr ptrResult) => Wldap32.ber_bvecfree(ptrResult);

        public static void BerBvfree(IntPtr flattenptr) => Wldap32.ber_bvfree(flattenptr);

        public static int BerFlatten(BerSafeHandle berElement, ref IntPtr flattenptr) => Wldap32.ber_flatten(berElement, ref flattenptr);

        public static int BerPrintfBerarray(BerSafeHandle berElement, string format, IntPtr value) => Wldap32.ber_printf_berarray(berElement, format, value);

        public static int BerPrintfBytearray(BerSafeHandle berElement, string format, HGlobalMemHandle value, int length) => Wldap32.ber_printf_bytearray(berElement, format, value, length);

        public static int BerPrintfEmptyarg(BerSafeHandle berElement, string format) => Wldap32.ber_printf_emptyarg(berElement, format);

        public static int BerPrintfInt(BerSafeHandle berElement, string format, int value) => Wldap32.ber_printf_int(berElement, format, value);

        public static int BerScanf(BerSafeHandle berElement, string format) => Wldap32.ber_scanf(berElement, format);

        public static int BerScanfBitstring(BerSafeHandle berElement, string format, ref IntPtr ptrResult, ref int length) => Wldap32.ber_scanf_bitstring(berElement, format, ref ptrResult, ref length);

        public static int BerScanfInt(BerSafeHandle berElement, string format, ref int result) => Wldap32.ber_scanf_int(berElement, format, ref result);

        public static int BerScanfPtr(BerSafeHandle berElement, string format, ref IntPtr value) => Wldap32.ber_scanf_ptr(berElement, format, ref value);

        public static void LdapAbandon(ConnectionHandle ldapHandle, int messagId) => Wldap32.ldap_abandon(ldapHandle, messagId);

        public static void LdapControlFree(IntPtr control) => Wldap32.ldap_control_free(control);

        public static int LdapCreateSortControl(ConnectionHandle handle, IntPtr keys, byte critical, ref IntPtr control) => Wldap32.ldap_create_sort_control(handle, keys, critical, ref control);

        public static int LdapGetOptionInt(ConnectionHandle ldapHandle, LdapOption option, ref int outValue) => Wldap32.ldap_get_option_int(ldapHandle, option, ref outValue);

        public static int LdapGetOptionPtr(ConnectionHandle ldapHandle, LdapOption option, ref IntPtr outValue) => Wldap32.ldap_get_option_ptr(ldapHandle, option, ref outValue);

        public static int LdapGetOptionSechandle(ConnectionHandle ldapHandle, LdapOption option, ref SecurityHandle outValue) => Wldap32.ldap_get_option_sechandle(ldapHandle, option, ref outValue);

        public static int LdapGetOptionSecInfo(ConnectionHandle ldapHandle, LdapOption option, SecurityPackageContextConnectionInformation outValue) => Wldap32.ldap_get_option_secInfo(ldapHandle, option, outValue);

        public static void LdapMemfree(IntPtr outValue) => Wldap32.ldap_memfree(outValue);

        public static int LdapParseResultReferral(ConnectionHandle ldapHandle, IntPtr result, IntPtr serverError, IntPtr dn, IntPtr message, ref IntPtr referral, IntPtr control, byte freeIt)
            => Wldap32.ldap_parse_result_referral(ldapHandle, result, serverError, dn, message, ref referral, control, freeIt);

        public static int LdapSetOptionClientcert(ConnectionHandle ldapHandle, LdapOption option, QUERYCLIENTCERT outValue) => Wldap32.ldap_set_option_clientcert(ldapHandle, option, outValue);

        public static int LdapSetOptionInt(ConnectionHandle ld, LdapOption option, ref int inValue) => Wldap32.ldap_set_option_int(ld, option, ref inValue);

        public static int LdapSetOptionPtr(ConnectionHandle ldapHandle, LdapOption option, ref IntPtr inValue) => Wldap32.ldap_set_option_ptr(ldapHandle, option, ref inValue);

        public static int LdapSetOptionReferral(ConnectionHandle ldapHandle, LdapOption option, ref LdapReferralCallback outValue) => Wldap32.ldap_set_option_referral(ldapHandle, option, ref outValue);

        public static int LdapSetOptionServercert(ConnectionHandle ldapHandle, LdapOption option, VERIFYSERVERCERT outValue) => Wldap32.ldap_set_option_servercert(ldapHandle, option, outValue);

        public static int LdapStartTls(ConnectionHandle ldapHandle, ref int ServerReturnValue, ref IntPtr Message, IntPtr ServerControls, IntPtr ClientControls) => Wldap32.ldap_start_tls(ldapHandle, ref ServerReturnValue, ref Message, ServerControls, ClientControls);

        public static byte LdapStopTls(ConnectionHandle ldapHandle) => Wldap32.ldap_stop_tls(ldapHandle);

        public static void LdapValueFree(IntPtr referral) => Wldap32.ldap_value_free(referral);
    }
}
