/*работа с планшетом без использования формы
 * 
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wgssSTU;
using System.Drawing;

namespace AddIn
{

    class SignatureObj
    {
        private wgssSTU.Tablet m_tablet;
        private wgssSTU.ICapability m_capability;
        private wgssSTU.IInformation m_information;
        private List<wgssSTU.IPenData> m_penData;
        private List<wgssSTU.IPenDataTimeCountSequence> m_penTimeData;
        private Bitmap m_bitmap;

        private delegate void ButtonClick();
        private struct Button
        {
            public System.Drawing.Rectangle Bounds;
            public String Text;
            public EventHandler Click;

            public void PerformClick()
            {
                Click(this, null);
            }
        };
        private Button[] m_btns;




        private void btnOk_Click(object sender, EventArgs e)
        {
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
        }

        private void onPenData(wgssSTU.IPenData penData)
        {
            //          Point pt = tabletToScreen(penData);
        }


        private void onGetReportException(wgssSTU.ITabletEventsException tabletEventsException)
        {
            try
            {
                tabletEventsException.getException();
            }
            catch (Exception e)
            {
                m_tablet.disconnect();
                m_tablet = null;
                m_penData = null;
                m_penTimeData = null;

            }
        }



        public SignatureObj(wgssSTU.IUsbDevice usbDevice)
        {


        }

        public void StartSigning() 
        {
        
        }


        public List<wgssSTU.IPenData> getPenData()
        {
            return m_penData;
        }


        public void StartSigning(wgssSTU.IUsbDevice usbDevice)
        {
            m_tablet = new wgssSTU.Tablet();
            wgssSTU.ProtocolHelper protocolHelper = new wgssSTU.ProtocolHelper();
            wgssSTU.IErrorCode ec = m_tablet.usbConnect(usbDevice, true);
            if (ec.value == 0)
            {
                m_capability = m_tablet.getCapability();
                m_information = m_tablet.getInformation();
            }
            else
            {
                throw new Exception(ec.message);
            }


            //разместить кнопки в нижней части экрана ширина 1/3 высота 1/7

            m_btns = new Button[3];
            int w2 = m_capability.screenWidth / 3;
            int w3 = m_capability.screenWidth / 3;
            int w1 = m_capability.screenWidth - w2 - w3;
            int y = m_capability.screenHeight * 6 / 7;
            int h = m_capability.screenHeight - y;
            m_btns[0].Bounds = new System.Drawing.Rectangle(0, y, w1, h);
            m_btns[1].Bounds = new System.Drawing.Rectangle(w1, y, w2, h);
            m_btns[2].Bounds = new System.Drawing.Rectangle(w1 + w2, y, w3, h);
            m_btns[0].Text = "OK";
            m_btns[1].Text = "Clear";
            m_btns[2].Text = "Cancel";
            m_btns[0].Click = new EventHandler(btnOk_Click);
            m_btns[1].Click = new EventHandler(btnClear_Click);
            m_btns[2].Click = new EventHandler(btnCancel_Click);



            m_tablet.onGetReportException += new wgssSTU.ITabletEvents2_onGetReportExceptionEventHandler(onGetReportException);
            m_tablet.onPenData += new wgssSTU.ITabletEvents2_onPenDataEventHandler(onPenData);
        }




    }
}
