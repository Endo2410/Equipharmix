using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaPresentacion.Utilidades
{
    public class imprimirserie
    {
        // Helper para enviar texto directo a la impresora (RAW)
        public class RawPrinterHelper
        {
            [System.Runtime.InteropServices.DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true)]
            public static extern bool OpenPrinter(string szPrinter, out IntPtr hPrinter, IntPtr pd);

            [System.Runtime.InteropServices.DllImport("winspool.Drv", EntryPoint = "ClosePrinter")]
            public static extern bool ClosePrinter(IntPtr hPrinter);

            [System.Runtime.InteropServices.DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA")]
            public static extern bool StartDocPrinter(IntPtr hPrinter, int level, [System.Runtime.InteropServices.In] ref DOCINFOA di);

            [System.Runtime.InteropServices.DllImport("winspool.Drv", EntryPoint = "EndDocPrinter")]
            public static extern bool EndDocPrinter(IntPtr hPrinter);

            [System.Runtime.InteropServices.DllImport("winspool.Drv", EntryPoint = "StartPagePrinter")]
            public static extern bool StartPagePrinter(IntPtr hPrinter);

            [System.Runtime.InteropServices.DllImport("winspool.Drv", EntryPoint = "EndPagePrinter")]
            public static extern bool EndPagePrinter(IntPtr hPrinter);

            [System.Runtime.InteropServices.DllImport("winspool.Drv", EntryPoint = "WritePrinter")]
            public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public struct DOCINFOA
            {
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStr)]
                public string pDocName;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStr)]
                public string pOutputFile;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStr)]
                public string pDataType;
            }

            public static bool SendStringToPrinter(string szPrinterName, string szString)
            {
                IntPtr pBytes;
                int dwCount = szString.Length;
                pBytes = System.Runtime.InteropServices.Marshal.StringToCoTaskMemAnsi(szString);
                bool bSuccess = SendBytesToPrinter(szPrinterName, pBytes, dwCount);
                System.Runtime.InteropServices.Marshal.FreeCoTaskMem(pBytes);
                return bSuccess;
            }

            private static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, int dwCount)
            {
                IntPtr hPrinter;
                DOCINFOA di = new DOCINFOA();
                di.pDocName = "Impresión Código de Barras";
                di.pDataType = "RAW";
                if (!OpenPrinter(szPrinterName, out hPrinter, IntPtr.Zero)) return false;
                if (!StartDocPrinter(hPrinter, 1, ref di)) return false;
                if (!StartPagePrinter(hPrinter)) return false;
                int dwWritten;
                bool bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                EndPagePrinter(hPrinter);
                EndDocPrinter(hPrinter);
                ClosePrinter(hPrinter);
                return bSuccess;
            }
        }
    }
}
