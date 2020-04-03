// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.DirectoryServices.Protocols
{
    internal static class NativePal
    {
        public static void BerBvecfree(IntPtr ptrResult) => OpenLDAP.ber_bvecfree(ptrResult);

        public static void BerBvfree(IntPtr flattenptr) => OpenLDAP.ber_bvfree(flattenptr);

        public static int BerFlatten(BerSafeHandle berElement, ref IntPtr flattenptr) => OpenLDAP.ber_flatten(berElement, ref flattenptr);

        public static int BerPrintfBerarray(BerSafeHandle berElement, string format, IntPtr value) => OpenLDAP.ber_printf_berarray(berElement, format, value);

        public static int BerPrintfBytearray(BerSafeHandle berElement, string format, HGlobalMemHandle value, int length) => OpenLDAP.ber_printf_bytearray(berElement, format, value, length);

        public static int BerPrintfEmptyarg(BerSafeHandle berElement, string format) => OpenLDAP.ber_printf_emptyarg(berElement, format);

        public static int BerPrintfInt(BerSafeHandle berElement, string format, int value) => OpenLDAP.ber_printf_int(berElement, format, value);

        public static int BerScanf(BerSafeHandle berElement, string format) => OpenLDAP.ber_scanf(berElement, format);

        public static int BerScanfBitstring(BerSafeHandle berElement, string format, ref IntPtr ptrResult, ref int length) => OpenLDAP.ber_scanf_bitstring(berElement, format, ref ptrResult, ref length);

        public static int BerScanfInt(BerSafeHandle berElement, string format, ref int result) => OpenLDAP.ber_scanf_int(berElement, format, ref result);

        public static int BerScanfPtr(BerSafeHandle berElement, string format, ref IntPtr value) => OpenLDAP.ber_scanf_ptr(berElement, format, ref value);

        public static void LdapAbandon(ConnectionHandle ldapHandle, int messagId) => OpenLDAP.ldap_abandon(ldapHandle, messagId);

        public static void LdapControlFree(IntPtr control) => OpenLDAP.ldap_control_free(control);

        public static int LdapCreateSortControl(ConnectionHandle handle, IntPtr keys, byte critical, ref IntPtr control) => OpenLDAP.ldap_create_sort_control(handle, keys, critical, ref control);

        public static int LdapGetOptionInt(ConnectionHandle ldapHandle, LdapOption option, ref int outValue) => OpenLDAP.ldap_get_option_int(ldapHandle, option, ref outValue);

        public static int LdapGetOptionPtr(ConnectionHandle ldapHandle, LdapOption option, ref IntPtr outValue) => OpenLDAP.ldap_get_option_ptr(ldapHandle, option, ref outValue);

        public static int LdapGetOptionSechandle(ConnectionHandle ldapHandle, LdapOption option, ref SecurityHandle outValue) => OpenLDAP.ldap_get_option_sechandle(ldapHandle, option, ref outValue);

        // This option is not supported on Linux, so it would most likely throw.
        public static int LdapGetOptionSecInfo(ConnectionHandle ldapHandle, LdapOption option, SecurityPackageContextConnectionInformation outValue) => OpenLDAP.ldap_get_option_secInfo(ldapHandle, option, outValue);

        public static void LdapMemfree(IntPtr outValue) => OpenLDAP.ldap_memfree(outValue);

        public static int LdapParseResultReferral(ConnectionHandle ldapHandle, IntPtr result, IntPtr serverError, IntPtr dn, IntPtr message, ref IntPtr referral, IntPtr control, byte freeIt)
            => OpenLDAP.ldap_parse_result_referral(ldapHandle, result, serverError, dn, message, ref referral, control, freeIt);

        // This option is not supported in Linux, so it would most likely throw.
        public static int LdapSetOptionClientcert(ConnectionHandle ldapHandle, LdapOption option, QUERYCLIENTCERT outValue) => OpenLDAP.ldap_set_option_clientcert(ldapHandle, option, outValue);

        public static int LdapSetOptionInt(ConnectionHandle ld, LdapOption option, ref int inValue) => OpenLDAP.ldap_set_option_int(ld, option, ref inValue);

        public static int LdapSetOptionPtr(ConnectionHandle ldapHandle, LdapOption option, ref IntPtr inValue) => OpenLDAP.ldap_set_option_ptr(ldapHandle, option, ref inValue);

        public static int LdapSetOptionReferral(ConnectionHandle ldapHandle, LdapOption option, ref LdapReferralCallback outValue) => OpenLDAP.ldap_set_option_referral(ldapHandle, option, ref outValue);

        // This option is not supported in Linux, so it would most likely throw.
        public static int LdapSetOptionServercert(ConnectionHandle ldapHandle, LdapOption option, VERIFYSERVERCERT outValue) => OpenLDAP.ldap_set_option_servercert(ldapHandle, option, outValue);

        public static int LdapStartTls(ConnectionHandle ldapHandle, ref int ServerReturnValue, ref IntPtr Message, IntPtr ServerControls, IntPtr ClientControls) => OpenLDAP.ldap_start_tls(ldapHandle, ref ServerReturnValue, ref Message, ServerControls, ClientControls);

        // openldap doesn't have a ldap_stop_tls function. Returning true as no-op for Linux.
        public static byte LdapStopTls(ConnectionHandle ldapHandle) => 1;

        public static void LdapValueFree(IntPtr referral) => OpenLDAP.ldap_value_free(referral);
    }
}
