// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.DirectoryServices.Protocols
{
    public static partial class BerConverter
    {
        private static int PALBer_Printf_EmptyArg(BerSafeHandle berElement, string format) => Wldap32.ber_printf_emptyarg(berElement, format);

        private static int PALBer_Printf_Int(BerSafeHandle berElement, string format, int value) => Wldap32.ber_printf_int(berElement, format, value);

        private static int PALBer_Flatten(BerSafeHandle berElement, ref IntPtr flattenptr) => Wldap32.ber_flatten(berElement, ref flattenptr);

        private static void PALBer_Bvfree(IntPtr flattenptr) => Wldap32.ber_bvfree(flattenptr);

        private static int PALBer_Scanf(BerSafeHandle berElement, string format) => Wldap32.ber_scanf(berElement, format);

        private static int PALBer_Scanf_Int(BerSafeHandle berElement, string format, ref int result) => Wldap32.ber_scanf_int(berElement, format, ref result);

        private static int PALBer_Scanf_Bitstring(BerSafeHandle berElement, string format, ref IntPtr ptrResult, ref int length) => Wldap32.ber_scanf_bitstring(berElement, format, ref ptrResult, ref length);

        private static int PALBer_Printf_Bytearray(BerSafeHandle berElement, string format, HGlobalMemHandle value, int length) => Wldap32.ber_printf_bytearray(berElement, format, value, length);

        private static int PALBer_Scanf_Ptr(BerSafeHandle berElement, string format, ref IntPtr value) => Wldap32.ber_scanf_ptr(berElement, format, ref value);

        private static int PALBer_Printf_Berarray(BerSafeHandle berElement, string format, IntPtr value) => Wldap32.ber_printf_berarray(berElement, format, value);

        private static void PALBer_Bvecfree(IntPtr ptrResult) => Wldap32.ber_bvecfree(ptrResult);
    }
}
