using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Drawing;

namespace SoundCatcher
{
    class DmxController
    {
      
        private enum FT_STATUS
        {
            FT_OK = 0,
            FT_INVALID_HANDLE,
            FT_DEVICE_NOT_FOUND,
            FT_DEVICE_NOT_OPENED,
            FT_IO_ERROR,
            FT_INSUFFICIENT_RESOURCES,
            FT_INVALID_PARAMETER,
            FT_INVALID_BAUD_RATE,
            FT_DEVICE_NOT_OPENED_FOR_ERASE,
            FT_DEVICE_NOT_OPENED_FOR_WRITE,
            FT_FAILED_TO_WRITE_DEVICE,
            FT_EEPROM_READ_FAILED,
            FT_EEPROM_WRITE_FAILED,
            FT_EEPROM_ERASE_FAILED,
            FT_EEPROM_NOT_PRESENT,
            FT_EEPROM_NOT_PROGRAMMED,
            FT_INVALID_ARGS,
            FT_OTHER_ERROR
        };
        
        private static byte[] buffer;
        private static System.IntPtr handle;
        private static int bytesWritten = 0;
        private static FT_STATUS status;


        public static byte[] buffer2 = new byte[511];
        public static System.IntPtr handle2;
        public static bool done = false;
        public static int bytesWritten2 = 0;
        //public static FT_STATUS status2;



        private const byte BITS_8 = 8;
        private const byte STOP_BITS_2 = 2;
        private const byte PARITY_NONE = 0;
        private const UInt16 FLOW_NONE = 0;
        private const byte PURGE_RX = 1;
        private const byte PURGE_TX = 2;

        private const byte SET_DMX_TX_MODE = 6;
        private const byte DMX_START_CODE = 0x7E;
        private const byte DMX_END_CODE = 0xE7;
        private const byte DMX_HEADER_LENGTH = 4;
        private const byte OFFSET = 0xFF;
        private const byte BYTE_LENGTH = 8;

        private const byte FT_PURGE_RX = 1;
        private const byte FT_PURGE_TX  =      2;

        // NOTE: This value must be one greater than the number of channels for your
        // device.  SO if you are communicating to a 26 channel device, value should 
        // be 27.
        private static int   DMX_DATA_LENGTH = 105;

        [DllImport("FTD2XX.dll")]
        private static extern FT_STATUS FT_Open(UInt32 uiPort, ref System.IntPtr handle);
        [DllImport("FTD2XX.dll")]
        private static extern FT_STATUS FT_Close(IntPtr ftHandle);
        [DllImport("FTD2XX.dll")]
        private static extern FT_STATUS FT_Read(IntPtr ftHandle, IntPtr lpBuffer, UInt32 dwBytesToRead, ref UInt32 lpdwBytesReturned);
        [DllImport("FTD2XX.dll")]
        private static extern FT_STATUS FT_Write(IntPtr ftHandle, IntPtr lpBuffer, UInt32 dwBytesToRead, ref UInt32 lpdwBytesWritten);
        [DllImport("FTD2XX.dll")]
        private static extern FT_STATUS FT_SetDataCharacteristics(IntPtr ftHandle, byte uWordLength, byte uStopBits, byte uParity);
        [DllImport("FTD2XX.dll")]
        private static extern FT_STATUS FT_SetFlowControl(IntPtr ftHandle, char usFlowControl, byte uXon, byte uXoff);
        [DllImport("FTD2XX.dll")]
        private static extern FT_STATUS FT_GetModemStatus(IntPtr ftHandle, ref UInt32 lpdwModemStatus);
        [DllImport("FTD2XX.dll")]
        private static extern FT_STATUS FT_Purge(IntPtr ftHandle, UInt32 dwMask);
        [DllImport("FTD2XX.dll")]
        private static extern FT_STATUS FT_SetBreakOn(IntPtr ftHandle);
        [DllImport("FTD2XX.dll")]
        private static extern FT_STATUS FT_SetBreakOff(IntPtr ftHandle);
        [DllImport("FTD2XX.dll")]
        private static extern FT_STATUS FT_GetStatus(IntPtr ftHandle, ref UInt32 lpdwAmountInRxQueue, ref UInt32 lpdwAmountInTxQueue, ref UInt32 lpdwEventStatus);
        [DllImport("FTD2XX.dll")]
        private static extern FT_STATUS FT_ResetDevice(IntPtr ftHandle);
        [DllImport("FTD2XX.dll")]
        private static extern FT_STATUS FT_SetDivisor(IntPtr ftHandle, char usDivisor);
        [DllImport("FTD2XX.dll")]
        private static extern FT_STATUS FT_ClrRts(IntPtr ftHandle);


        private static void test()
        {

            openDMX(27);
 
            //Thread thread = new Thread(new ThreadStart(writeData));
            //thread.Start();
            setDmxValue(0, 0); // set DMX channel 1 to maximum value
            setDmxValue(1, 128); // set DMX channel 1 to maximum value
            setDmxValue(2, 255); // set DMX channel 1 to maximum value
            setDmxValue(3, 255); // set DMX channel 1 to maximum value
            setDmxValue(4, 255); // set DMX channel 1 to maximum value
            setDmxValue(11, 255); // set DMX channel 1 to maximum value
            setDmxValue(12, 255); // set DMX channel 1 to maximum value
            setDmxValue(13, 255); // set DMX channel 1 to maximum value
            setDmxValue(22, 255); // set DMX channel 1 to maximum value
            //setDmxValue(5, 255); // set DMX channel 1 to maximum value
            write();
        }




        public static bool openDMX(int numChannels)
        {
            if(buffer==null) buffer = new byte[DMX_DATA_LENGTH ]; // can be any length up to 512. The shorter the faster.
          
            
            //Thread.Sleep(750);
            
            //int RTimeout = 120;
            //int WTimeout = 100;
            //FT_SetTimeouts(handle,RTimeout,WTimeout);

            // Piurges the buffer
            //FT_Purge(handle,FT_PURGE_RX);

            status = FT_Open(0, ref handle2);
            //Thread thread = new Thread(new ThreadStart(writeData2));
            //thread.Start();
            setDmxValue(1000, 0);  //Set DMX Start Code

            status = FT_Open(1, ref handle);
            if (FT_STATUS.FT_OK != status)
            {
               // System.Windows.Forms.MessageBox.Show("Unable to open DMX Controller 1");
                //return false;
            }

           

            return true;
       }

        

        public static void setDmxValue(int channel, byte value)
        {
            if (channel >= 1000)
            {
                channel -= 1000;
                if (buffer != null)
                {
                    buffer2[channel] = value;
                }
            }
            else
                buffer[channel] = value;
        }
        public static byte getDmxValue(int channel)
        {
            if (channel >= 1000)
            {
                channel -= 1000;
                return  buffer2[channel];
            }
            return buffer[channel];
        }

        public static void dark()
        {
            for (int r = 0; r < buffer.Length; ++r)
            {
                buffer[r] = 0;
                buffer2[r] = 0;
            }

            write();
        }

        public static void write()
        {
            if (handle == null) return;
            buffer[0] = 0;
            byte[] header = new byte[4];

            header[0] = DMX_START_CODE;
            header[1] = SET_DMX_TX_MODE;
            header[2] = (byte)(buffer.Length & OFFSET);
            header[3] = (byte)(buffer.Length >> BYTE_LENGTH);
            bytesWritten = writePacket(handle, header, header.Length);

            bytesWritten = writePacket(handle, buffer, buffer.Length);

            header[0] = DMX_END_CODE;
            bytesWritten = writePacket(handle, header, 1);



            initOpenDMX2();
            FT_SetBreakOn(handle2);
            FT_SetBreakOff(handle2);
            bytesWritten = writePacket(handle2, buffer2, buffer2.Length);

        }

        private static int writePacket(System.IntPtr handle, byte[] data, int length)
        {
            IntPtr ptr = Marshal.AllocHGlobal((int)length);
            Marshal.Copy(data, 0, ptr, (int)length);
            uint bytesWritten = 0;
            FT_Write(handle, ptr, (uint)length, ref bytesWritten);
            return (int)bytesWritten;
        }


        public static void writeData2()
        {
            while (!done)
            {
                initOpenDMX2();
                FT_SetBreakOn(handle2);
                FT_SetBreakOff(handle2);
                bytesWritten = writePacket(handle2,buffer2, buffer2.Length);
                Thread.Sleep(25);
            }

        }

      
        public static void initOpenDMX2()
        {
            status = FT_ResetDevice(handle2);
            status = FT_SetDivisor(handle2, (char)12);  // set baud rate
            status = FT_SetDataCharacteristics(handle2, BITS_8, STOP_BITS_2, PARITY_NONE);
            status = FT_SetFlowControl(handle2, (char)FLOW_NONE, 0, 0);
            status = FT_ClrRts(handle2);
            status = FT_Purge(handle2, PURGE_TX);
            status = FT_Purge(handle2, PURGE_RX);
        }

        public static void closeOpenDMX2()
        {
            done = true;
        }


    }


    
    
}
