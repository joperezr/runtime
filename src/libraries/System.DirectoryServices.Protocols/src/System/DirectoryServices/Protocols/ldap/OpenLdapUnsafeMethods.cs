// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
    internal class OpenLDAP
    {
        private const string _openldapdll = "libldap";

        [DllImport(_openldapdll, EntryPoint = "ldap_initialize", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int ldap_initialize(out IntPtr ld, string hostname);

        [DllImport(_openldapdll, EntryPoint = "ldap_unbind_ext_s", CharSet = CharSet.Ansi)]
        public static extern int ldap_unbind_ext_s(IntPtr ld, ref IntPtr serverctrls, ref IntPtr clientctrls);

        [DllImport(_openldapdll, EntryPoint = "ldap_get_dn", CharSet = CharSet.Ansi)]
        public static extern IntPtr ldap_get_dn([In] ConnectionHandle ldapHandle, [In] IntPtr result);

        [DllImport(_openldapdll, EntryPoint = "ldap_get_option", CharSet = CharSet.Ansi)]
        public static extern int ldap_get_option_secInfo([In] ConnectionHandle ldapHandle, [In] LdapOption option, [In, Out] SecurityPackageContextConnectionInformation outValue);

        [DllImport(_openldapdll, EntryPoint = "ldap_get_option", CharSet = CharSet.Ansi)]
        public static extern int ldap_get_option_sechandle([In] ConnectionHandle ldapHandle, [In] LdapOption option, ref SecurityHandle outValue);

        [DllImport(_openldapdll, EntryPoint = "ldap_get_option", CharSet = CharSet.Ansi)]
        public static extern int ldap_get_option_int([In] ConnectionHandle ldapHandle, [In] LdapOption option, ref int outValue);

        [DllImport(_openldapdll, EntryPoint = "ldap_get_option", CharSet = CharSet.Ansi)]
        public static extern int ldap_get_option_ptr([In] ConnectionHandle ldapHandle, [In] LdapOption option, ref IntPtr outValue);

        [DllImport(_openldapdll, EntryPoint = "ldap_get_values_len", CharSet = CharSet.Ansi)]
        public static extern IntPtr ldap_get_values_len([In] ConnectionHandle ldapHandle, [In] IntPtr result, string name);

        [DllImport(_openldapdll, EntryPoint = "ldap_result", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int ldap_result([In] ConnectionHandle ldapHandle, int messageId, int all, LDAP_TIMEVAL timeout, ref IntPtr Mesage);

        [DllImport(_openldapdll, EntryPoint = "ldap_result2error", CharSet = CharSet.Ansi)]
        public static extern int ldap_result2error([In] ConnectionHandle ldapHandle, [In] IntPtr result, int freeIt);

        [DllImport(_openldapdll, EntryPoint = "ldap_search_ext", CharSet = CharSet.Ansi)]
        public static extern int ldap_search([In] ConnectionHandle ldapHandle, string dn, int scope, string filter, IntPtr attributes, bool attributeOnly, IntPtr servercontrol, IntPtr clientcontrol, int timelimit, int sizelimit, ref int messageNumber);

        [DllImport(_openldapdll, EntryPoint = "ldap_set_option", CharSet = CharSet.Ansi)]
        public static extern int ldap_set_option_clientcert([In] ConnectionHandle ldapHandle, [In] LdapOption option, QUERYCLIENTCERT outValue);

        [DllImport(_openldapdll, EntryPoint = "ldap_set_option", CharSet = CharSet.Ansi)]
        public static extern int ldap_set_option_servercert([In] ConnectionHandle ldapHandle, [In] LdapOption option, VERIFYSERVERCERT outValue);

        [DllImport(_openldapdll, EntryPoint = "ldap_set_option", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int ldap_set_option_int([In] ConnectionHandle ld, [In] LdapOption option, ref int inValue);

        [DllImport(_openldapdll, EntryPoint = "ldap_set_option", CharSet = CharSet.Ansi)]
        public static extern int ldap_set_option_ptr([In] ConnectionHandle ldapHandle, [In] LdapOption option, ref IntPtr inValue);

        [DllImport(_openldapdll, EntryPoint = "ldap_set_option", CharSet = CharSet.Ansi)]
        public static extern int ldap_set_option_referral([In] ConnectionHandle ldapHandle, [In] LdapOption option, ref LdapReferralCallback outValue);

        [DllImport(_openldapdll, EntryPoint = "ldap_start_tls_s", CharSet = CharSet.Ansi)]
        public static extern int ldap_start_tls(ConnectionHandle ldapHandle, ref int ServerReturnValue, ref IntPtr Message, IntPtr ServerControls, IntPtr ClientControls);

        [DllImport(_openldapdll, EntryPoint = "ldap_parse_result", CharSet = CharSet.Ansi)]
        public static extern int ldap_parse_result([In] ConnectionHandle ldapHandle, [In] IntPtr result, ref int serverError, ref IntPtr dn, ref IntPtr message, ref IntPtr referral, ref IntPtr control, byte freeIt);

        [DllImport(_openldapdll, EntryPoint = "ldap_parse_result", CharSet = CharSet.Ansi)]
        public static extern int ldap_parse_result_referral([In] ConnectionHandle ldapHandle, [In] IntPtr result, IntPtr serverError, IntPtr dn, IntPtr message, ref IntPtr referral, IntPtr control, byte freeIt);

        [DllImport(_openldapdll, EntryPoint = "ldap_parse_extended_result", CharSet = CharSet.Ansi)]
        public static extern int ldap_parse_extended_result([In] ConnectionHandle ldapHandle, [In] IntPtr result, ref IntPtr oid, ref IntPtr data, byte freeIt);

        [DllImport(_openldapdll, EntryPoint = "ldap_parse_reference", CharSet = CharSet.Ansi)]
        public static extern int ldap_parse_reference([In] ConnectionHandle ldapHandle, [In] IntPtr result, ref IntPtr referrals, IntPtr ServerControls, byte freeIt);

        [DllImport(_openldapdll, EntryPoint = "ldap_simple_bind_s", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int ldap_simple_bind([In] ConnectionHandle ld, string who, string passwd);

        [DllImport(_openldapdll, EntryPoint = "ldap_bind_s", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int ldap_bind_s([In] ConnectionHandle ld, string who, string passwd, int method);

        [DllImport(_openldapdll, EntryPoint = "ldap_err2string", CharSet = CharSet.Ansi)]
        public static extern IntPtr ldap_err2string(int err);

        [DllImport(_openldapdll, EntryPoint = "ldap_extended_operation", CharSet = CharSet.Ansi)]
        public static extern int ldap_extended_operation([In] ConnectionHandle ldapHandle, string oid, berval data, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber);

        [DllImport(_openldapdll, EntryPoint = "ldap_first_attribute", CharSet = CharSet.Ansi)]
        public static extern IntPtr ldap_first_attribute([In] ConnectionHandle ldapHandle, [In] IntPtr result, ref IntPtr address);

        [DllImport(_openldapdll, EntryPoint = "ldap_first_entry", CharSet = CharSet.Ansi)]
        public static extern IntPtr ldap_first_entry([In] ConnectionHandle ldapHandle, [In] IntPtr result);

        [DllImport(_openldapdll, EntryPoint = "ldap_first_reference", CharSet = CharSet.Ansi)]
        public static extern IntPtr ldap_first_reference([In] ConnectionHandle ldapHandle, [In] IntPtr result);

        [DllImport(_openldapdll, EntryPoint = "ldap_create_sort_control", CharSet = CharSet.Ansi)]
        public static extern int ldap_create_sort_control(ConnectionHandle handle, IntPtr keys, byte critical, ref IntPtr control);

        [DllImport(_openldapdll, EntryPoint = "ldap_control_free", CharSet = CharSet.Ansi)]
        public static extern int ldap_control_free(IntPtr control);

        [DllImport(_openldapdll, EntryPoint = "ldap_controls_free", CharSet = CharSet.Ansi)]
        public static extern int ldap_controls_free([In] IntPtr value);

        [DllImport(_openldapdll, EntryPoint = "ldap_value_free", CharSet = CharSet.Ansi)]
        public static extern int ldap_value_free([In] IntPtr value);

        [DllImport(_openldapdll, EntryPoint = "ldap_value_free_len", CharSet = CharSet.Ansi)]
        public static extern IntPtr ldap_value_free_len([In] IntPtr berelement);

        [DllImport(_openldapdll, EntryPoint = "ldap_memfree", CharSet = CharSet.Ansi)]
        public static extern void ldap_memfree([In] IntPtr value);

        [DllImport(_openldapdll, EntryPoint = "ldap_modify_ext", CharSet = CharSet.Ansi)]
        public static extern int ldap_modify([In] ConnectionHandle ldapHandle, string dn, IntPtr attrs, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber);

        [DllImport(_openldapdll, EntryPoint = "ldap_next_attribute", CharSet = CharSet.Ansi)]
        public static extern IntPtr ldap_next_attribute([In] ConnectionHandle ldapHandle, [In] IntPtr result, [In, Out] IntPtr address);

        [DllImport(_openldapdll, EntryPoint = "ldap_next_entry", CharSet = CharSet.Ansi)]
        public static extern IntPtr ldap_next_entry([In] ConnectionHandle ldapHandle, [In] IntPtr result);

        [DllImport(_openldapdll, EntryPoint = "ldap_next_reference", CharSet = CharSet.Ansi)]
        public static extern IntPtr ldap_next_reference([In] ConnectionHandle ldapHandle, [In] IntPtr result);

        [DllImport(_openldapdll, EntryPoint = "ldap_abandon", CharSet = CharSet.Ansi)]
        public static extern int ldap_abandon([In] ConnectionHandle ldapHandle, [In] int messagId);

        [DllImport(_openldapdll, EntryPoint = "ldap_add_ext", CharSet = CharSet.Ansi)]
        public static extern int ldap_add([In] ConnectionHandle ldapHandle, string dn, IntPtr attrs, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber);

        [DllImport(_openldapdll, EntryPoint = "ldap_delete_ext", CharSet = CharSet.Ansi)]
        public static extern int ldap_delete_ext([In] ConnectionHandle ldapHandle, string dn, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber);

        [DllImport(_openldapdll, EntryPoint = "ldap_rename", CharSet = CharSet.Ansi)]
        public static extern int ldap_rename([In] ConnectionHandle ldapHandle, string dn, string newRdn, string newParentDn, int deleteOldRdn, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber);

        [DllImport(_openldapdll, EntryPoint = "ldap_compare_ext", CharSet = CharSet.Ansi)]
        public static extern int ldap_compare([In] ConnectionHandle ldapHandle, string dn, string attributeName, berval binaryValue, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber);

        // Basic Encoding Rules (BER) native functions
        [DllImport(_openldapdll, EntryPoint = "ber_alloc_t", CharSet = CharSet.Ansi)]
        public static extern IntPtr ber_alloc(int option);

        [DllImport(_openldapdll, EntryPoint = "ber_init", CharSet = CharSet.Ansi)]
        public static extern IntPtr ber_init(berval value);

        [DllImport(_openldapdll, EntryPoint = "ber_free", CharSet = CharSet.Ansi)]
        public static extern IntPtr ber_free([In] IntPtr berelement, int option);

        [DllImport(_openldapdll, EntryPoint = "ber_printf", CharSet = CharSet.Ansi)]
        public static extern int ber_printf_emptyarg(BerSafeHandle berElement, string format);

        [DllImport(_openldapdll, EntryPoint = "ber_printf", CharSet = CharSet.Ansi)]
        public static extern int ber_printf_int(BerSafeHandle berElement, string format, int value);

        [DllImport(_openldapdll, EntryPoint = "ber_printf", CharSet = CharSet.Ansi)]
        public static extern int ber_printf_bytearray(BerSafeHandle berElement, string format, HGlobalMemHandle value, int length);

        [DllImport(_openldapdll, EntryPoint = "ber_printf", CharSet = CharSet.Ansi)]
        public static extern int ber_printf_berarray(BerSafeHandle berElement, string format, IntPtr value);

        [DllImport(_openldapdll, EntryPoint = "ber_flatten", CharSet = CharSet.Ansi)]
        public static extern int ber_flatten(BerSafeHandle berElement, ref IntPtr value);

        [DllImport(_openldapdll, EntryPoint = "ber_bvfree", CharSet = CharSet.Ansi)]
        public static extern int ber_bvfree(IntPtr value);

        [DllImport(_openldapdll, EntryPoint = "ber_bvecfree", CharSet = CharSet.Ansi)]
        public static extern int ber_bvecfree(IntPtr value);

        [DllImport(_openldapdll, EntryPoint = "ber_scanf", CharSet = CharSet.Ansi)]
        public static extern int ber_scanf(BerSafeHandle berElement, string format);

        [DllImport(_openldapdll, EntryPoint = "ber_scanf", CharSet = CharSet.Ansi)]
        public static extern int ber_scanf_int(BerSafeHandle berElement, string format, ref int value);

        [DllImport(_openldapdll, EntryPoint = "ber_scanf", CharSet = CharSet.Ansi)]
        public static extern int ber_scanf_bitstring(BerSafeHandle berElement, string format, ref IntPtr value, ref int length);

        [DllImport(_openldapdll, EntryPoint = "ber_scanf", CharSet = CharSet.Ansi)]
        public static extern int ber_scanf_ptr(BerSafeHandle berElement, string format, ref IntPtr value);
    }
}
