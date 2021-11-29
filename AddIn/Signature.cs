using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using wgssSTU;
using System.Windows.Forms;
using System.IO;


enum PenDataOptionMode
{
    PenDataOptionMode_None,
    PenDataOptionMode_TimeCount,
    PenDataOptionMode_SequenceNumber,
    PenDataOptionMode_TimeCountSequence
};


namespace AddIn
{


    public class Signature
    {
        private SignatureForm signatureForm;
        private Bitmap m_bitmap;
        private int m_penDataType;
        private List<wgssSTU.IPenDataTimeCountSequence> m_penTimeData;
        private List<wgssSTU.IPenData> m_penData;

        public string getBitmapBase64()
        {
            //ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            //EncoderParameters myEncoderParameters = new EncoderParameters(1);

            //EncoderParameter myEncoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Quality);
            //myEncoderParameters.Param[0] = myEncoderParameter;
            //m_bitmap.Save(outStream, jgpEncoder, myEncoderParameters);
            if (m_bitmap != null)
            {
                MemoryStream outStream = new MemoryStream();
                m_bitmap.Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return Convert.ToBase64String(outStream.GetBuffer());
            }
            else
            {
                return null;
            }
        }
  

        public void StartSignatureForm()
        {
            m_bitmap = null;
            m_penDataType = 0;
            m_penData = null;
            m_penTimeData = null;

            wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();
            if (usbDevices.Count != 0)
            {
                try
                {
                    wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                    SignatureForm demo = new SignatureForm(usbDevice, false);
                    demo.ShowDialog();

                    m_penDataType = demo.penDataType;

                    m_bitmap = demo.GetResultBitmap();

                    if (m_penDataType == (int)PenDataOptionMode.PenDataOptionMode_TimeCountSequence)
                        m_penTimeData = demo.getPenTimeData();
                    else
                        m_penData = demo.getPenData();

                    if (m_penData != null || m_penTimeData != null)
                    {
                        // process penData here!

                        wgssSTU.IInformation information = demo.getInformation();
                        wgssSTU.ICapability capability = demo.getCapability();
                    }
                    demo.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No STU devices attached");
            }
        }


        public void StartSignatureObj()
        {
            int penDataType;
            List<wgssSTU.IPenDataTimeCountSequence> penTimeData = null;
            List<wgssSTU.IPenData> penData = null;

            wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();
            if (usbDevices.Count != 0)
            {
                try
                {
                    wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                    SignatureObj demo = new SignatureObj(usbDevice);
                    demo.StartSigning();
                    penData = demo.getPenData();

                    if (penData != null || penTimeData != null)
                    {
                        // process penData here!

//                        wgssSTU.IInformation information = demo.getInformation();
//                       wgssSTU.ICapability capability = demo.getCapability();
                    }
 //                   demo.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No STU devices attached");
            }
        }
    }



}

