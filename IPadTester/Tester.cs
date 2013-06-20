using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using IPADLib;
using IPADTest;

namespace IPADDemo
{
    public partial class Tester : Form
    {
        enum TransctionToRun
        {
            None,
            Keyed,
            Swiped,
        }

        #region Mercury properties

        private TransctionToRun transactionToRun = TransctionToRun.None;
        private static string merchantID = "118725340908147";
        private static string password = "xyz";
        private static string invoiceNo = string.Empty;
        private static string memo = "Testing MagTek.IPAD.CSharp";
        private static string cvvData = "123";
        private static decimal purchase = 2.25m;
        private static string operatorID = "test";

        #endregion Mercury properties

        private IPAD pp = null;
        private DeviceMonitor devmon;
        private DeviceManager dv;
        static AutoResetEvent autoEvent = new AutoResetEvent(false);
        static AutoResetEvent cancelTestEvent = new AutoResetEvent(false);
        static PINRequestCompleteEventArgs pinResponse;

        private int MsgID = 1;
        private int MsgNo = 4;
        private MemoryStream ms; 

        public Tester()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dv = new DeviceManager();

            string p = dv.FindIPAD();
            if (p != null) DeviceConnect(p);

            devmon = new DeviceMonitor();
            devmon.RegisterForEvents(this.Handle);
            devmon.DeviceAttachedEvent += new DeviceAttachedEventHandler(devmon_DeviceAttachedEvent);
            devmon.DeviceRemovedEvent += new DeviceRemovedEventHandler(devmon_DeviceRemovedEvent);

            ddlCommands.SelectedIndex = 0;
            HideAllParams();
            DisplayMSGParamInitialize();
        }

        void devmon_DeviceRemovedEvent(object sender, DeviceMonitorEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new DeviceRemovedEventHandler(devmon_DeviceRemovedEvent), new object[] { sender, e });
                return;
            }
            if (pp != null)
            {
                if (pp.DevicePath == e.device)
                {
                    pp.Dispose();
                    pp = null;
                    lblStatus.Text = "Status: Device is disconnected.";

                    btnRun.Enabled = false;
                    btnValidate.Enabled = false;
                    cancelTestEvent.Set();
                }
            }

            string p = dv.FindIPAD();
            if (p != null) DeviceConnect(p);
        }

        void devmon_DeviceAttachedEvent(object sender, DeviceMonitorEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new DeviceAttachedEventHandler(devmon_DeviceAttachedEvent), new object[] { sender, e });
                return;
            }

            if (pp == null)
            {
                DeviceConnect(e.device);                
            }
        }

        void DeviceConnect(string path)
        {
            if (path != null)
            {
                pp = new IPAD();
                pp.CardRequestCompleteEvent += new CardRequestCompleteEventHandler(pp_GetMSRCompleteEvent);
                pp.PINRequestCompleteEvent += new PINRequestCompleteEventHandler(pp_GetPINCompleteEvent);
                pp.GetKeyCompleteEvent += new GetKeyCompleteEventHandler(pp_GetKeyCompleteEvent);
                pp.DisplayCompleteEvent += new DisplayCompleteEventHandler(pp_DisplayCompleteEvent);
                pp.StateChangedEvent += new DeviceStateChangedEventHandler(pp_StateChangedEvent);
                
                try
                {
                    pp.Connect(path);
                    lblStatus.Text = "Status: Device is connected.";
                    btnRun.Enabled = true;
                    btnValidate.Enabled = true;
                }
                catch
                {
                    pp = null;
                }
            }
        }

        void pp_DisplayCompleteEvent(object sender, DisplayCompleteEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new DisplayCompleteEventHandler(pp_DisplayCompleteEvent), new object[] { sender, e });
                return;
            }

            String str;
            str = String.Format("DisplayCompleteEvent:  IPADStatus.{0:s}", Enum.GetName(typeof(IPADStatus), (byte)e.OpStatus));

            ((AutoResetEvent)autoEvent).Set();
            DisplayStatusResult(str, Color.Indigo);
          }

        void pp_StateChangedEvent(object sender, IPADLib.DeviceStateChangeEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new DeviceStateChangedEventHandler(pp_StateChangedEvent), new object[] { sender, e });
                return;
            }
            string status;

            status = String.Format("StatusChangedEvent:  DeviceStatus.{0:s}", Enum.GetName(typeof(DeviceState), (byte)e.NewState));
            
            if (e.SessionSts != 0)
            {
                status += String.Format(";  SessionStatus: {0:s} ", e.SessionSts.ToString());
                
            }

            DisplayResult(status);
        }
        string ConvertOpStatusToString (IPADStatus status)
        {
            if (status.ToString() != null)
                return String.Format("IPADStatus.{0:s} ", status.ToString());
            else
                return String.Format("{0:d} ", status);
        }
        void pp_GetKeyCompleteEvent(object sender, GetKeyCompleteEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new GetKeyCompleteEventHandler(pp_GetKeyCompleteEvent), new object[] { sender, e });
                return;
            }
            if (e.OpStatus == IPADStatus.OK)
            {
                DisplayResult (String.Format("GetKeyCompleteEvent: {0:s} key selected.", Enum.GetName(typeof(FunctionKey), e.key)));
            }

            else
                DisplayResult(String.Format ("GetKeyCompleteEvent: Error {0:x} = ", e.OpStatus) + ConvertOpStatusToString (e.OpStatus));
        }

        void pp_GetPINCompleteEvent(object sender, PINRequestCompleteEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new PINRequestCompleteEventHandler(pp_GetPINCompleteEvent), new object[] { sender, e });
                return;
            }
            ((AutoResetEvent)autoEvent).Set();
            pinResponse = e;
            if (e.OpStatus == IPADStatus.OK)
            {
                DisplayResult("GetPINCompleteEvent");
                DisplayResult("KSN: " + MakeHex(e.KSN, 0, e.KSN.Length) + ".");
                DisplayResult("EPB: " + MakeHex(e.EPB, 0, e.EPB.Length) + ".");
            }
            else
                DisplayResult(String.Format("GetPINCompleteEvent: Error {0:x} = ", e.OpStatus) + ConvertOpStatusToString(e.OpStatus));
        }
        private string MakeHex(byte[] b)
        {
            int len = b.Length;
            StringBuilder s = new StringBuilder(len * 2);
            for (int i = 0; i < len; i++)
            {
                s.Append(b[i].ToString("X2"));
            }
            return s.ToString();
        }
        private string MakeHex(byte[] b, int start, int len)
        {
            StringBuilder s = new StringBuilder(len * 2);
            for (int i = 0; i < len; i++)
            {
                s.Append(b[i + start].ToString("X2"));
            }
            return s.ToString();
        }

        string ConvertCardStatusToString(CardStatus status)
        {
            if (Enum.GetName(typeof(CardStatus), status) != null)
                return String.Format("{0:s} ", Enum.GetName(typeof(CardStatus), status));
            else
                return String.Format("{0:d} ", status);
        }

        string ConvertCardTypeToString(CardType type)
        {
            if (Enum.GetName(typeof(CardType), type) != null)
                return String.Format("{0:s} ", Enum.GetName(typeof(CardType), type));
            else
                return String.Format("{0:d} ", type);
        }

        void pp_GetMSRCompleteEvent(object sender, CardRequestCompleteEventArgs e)
        {
            string encTrack2 = string.Empty;
            string ksn = string.Empty;

            if (InvokeRequired)
            {
                Invoke(new CardRequestCompleteEventHandler(pp_GetMSRCompleteEvent), new object[] { sender, e });
                return;
            }
            
            ((AutoResetEvent)autoEvent).Set();
            DisplayResult("GetMSRCompleteEvent");

            if (e.OpStatus == IPADStatus.OK && e.CardStatus == CardStatus.OK)
            {
                DisplayResult(String.Format("OpStatus: {0:s}, CardStatus: {1:s}, CardType: {2:s}", ConvertOpStatusToString(e.OpStatus), ConvertCardStatusToString(e.CardStatus), ConvertCardTypeToString(e.card.CardType)));
                if ((e.card.Track1Status == 0) && (e.card.Track1 != null))
                    DisplayResult(String.Format("Track1    : {0:s}", Encoding.ASCII.GetString(e.card.Track1)));
                if ((e.card.Track2Status == 0) && (e.card.Track2 != null))
                    DisplayResult(String.Format("Track2    : {0:s}", Encoding.ASCII.GetString(e.card.Track2)));
                if ((e.card.Track3Status == 0) && (e.card.Track3 != null))
                    DisplayResult(String.Format("Track3    : {0:s}", Encoding.ASCII.GetString(e.card.Track3)));
                if ((e.card.EncTrack1Status == 0) && (e.card.EncTrack1 != null))
                    DisplayResult(String.Format("EncTrac1  : {0:s}", MakeHex(e.card.EncTrack1)));
                if ((e.card.EncTrack2Status == 0) && (e.card.EncTrack2 != null))
                {
                    DisplayResult(String.Format("EncTrac2  : {0:s}", MakeHex(e.card.EncTrack2)));
                    encTrack2 = MakeHex(e.card.EncTrack2);
                }
                if ((e.card.EncTrack3Status == 0) && (e.card.EncTrack3 != null))
                    DisplayResult(String.Format("EncTrac3  : {0:s}", MakeHex(e.card.EncTrack3)));
                if ((e.card.EncMPStatus == 0) && (e.card.EncMP != null))
                    DisplayResult(String.Format("EncMP     : {0:s}", MakeHex(e.card.EncMP)));
                if (e.card.KSNStatus == 0)
                {
                    DisplayResult(String.Format("KSN       : {0:s}", MakeHex(e.card.KSN)));
                    ksn = MakeHex(e.card.KSN);
                    DisplayResult(String.Format("MPSts     : {0:s}", MakeHex(e.card.MPSts)));
                }

                switch (this.transactionToRun)
                {
                    case TransctionToRun.Keyed:
                        this.RunKeyedTransaction(encTrack2, ksn);
                        break;
                    case TransctionToRun.Swiped:
                        this.RunSwipedTransaction(encTrack2, ksn);
                        break;
                    default:
                        // Don't run anything
                        break;
                }
            }
            else
            {
                DisplayResult(String.Format("GetMSRCompleteEvent: Error {0:s}", ConvertOpStatusToString(e.OpStatus)));
            }

        }        

        private void DisplayResult(string strRet)
        {
            DateTime dt = DateTime.Now;
            String str;

            str = dt.ToString("HH:mm:ss");
            richTextBoxEventResult.SelectionStart = richTextBoxEventResult.TextLength;
            richTextBoxEventResult.SelectionColor = Color.Navy;
            richTextBoxEventResult.SelectedText = str + ":  " + strRet + Environment.NewLine;
            richTextBoxEventResult.ScrollToCaret();

        }
        private void DisplayStatusResult(string strRet, Color color)
        {
            DateTime dt = DateTime.Now;
            String str;

            str = dt.ToString("HH:mm:ss");


            richTextBoxStatus.SelectionStart = richTextBoxStatus.TextLength;
            richTextBoxStatus.SelectionColor = color;
            richTextBoxStatus.SelectedText = str + ":  " + strRet + Environment.NewLine;
            richTextBoxStatus.ScrollToCaret();
        }
        private void DisplayStatusResult(string strRet, Color color, FontStyle bold)
        {
            DateTime dt = DateTime.Now;
            String str;

            str = dt.ToString("HH:mm:ss");

            richTextBoxStatus.SelectionStart = richTextBoxStatus.TextLength;
            richTextBoxStatus.SelectionColor = color;
            richTextBoxStatus.SelectionFont = new Font("Arial", 10, bold);
            richTextBoxStatus.SelectedText = str + ":  " + strRet + Environment.NewLine;
            richTextBoxStatus.ScrollToCaret();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string str;
            this.transactionToRun = TransctionToRun.None;

            if (checkBoxClearEventWindow.Checked == true)
                richTextBoxEventResult.Clear();
            str = ddlCommands.SelectedItem.ToString();
            switch (str) //ddlCommands.SelectedIndex)
            {
                case "Display Msg": //"Display Msg"
                    IssueDisplay(System.Convert.ToByte(textBoxParam1.Text, 10), (IPADLib.DisplayMsg)System.Convert.ToByte(textBoxParam2.Text, 10), true);
                    break;

                case "Request Manual Card Entry":  //HS 6/22/2011
                    this.transactionToRun = TransctionToRun.Keyed;
                    RequestManualEntry();
                    break;

                case "Request PIN": //"Request PIN"
                    RequestPIN();
                    break;

                case "Request Card":  //"Request Card"
                    this.transactionToRun = TransctionToRun.Swiped;
                    RequestCard();
                    break;

                case "Get Response": //"Get Selection"
                    GetSelection();
                    break;

                case "Confirm Amount": //"Confirm Amount"
                    ConfirmAmount();
                    break;

                case "Select Credit Debit": //"Select Credit Debit"
                    SelectCreditDebit();
                    break;

                case "Halt Operation":  //"Halt Operation"
                    HaltOperation();
                    break;

                case "End Session": //"End Session"
                    EndSession();
                    break;

                case "Send Amount": //"Send amount"
                    SendAmount();
                    break;

                case "Get Status": //"Get Status"
                    GetStatus();
                    break;
               
                default:

                    break;
            }
        }
        private byte GetTypeData()
        {
            int b = (PANinPIN.Checked) ? 0x08 : 0;
            b |= (QwickCodes.Checked) ? 0x04 : 0;
            b |= System.Convert.ToByte(textBoxParam2.Text, 10);
            return (byte)b;           
        }
        private void RequestManualEntry()
        {
            if (pp == null)
            {
                MessageBox.Show("Device is not connected");
                return;
            }

            string str;
            FieldOptions field = (IPADLib.FieldOptions)System.Convert.ToByte(textBoxParam2.Text, 10);
            byte bQwickCodes = (QwickCodes.Checked) ? (byte)1 : (byte)0;
            byte bPANinPIN = (PANinPIN.Checked) ? (byte)1 : (byte)0;
            byte waitTime = System.Convert.ToByte(textBoxParam1.Text, 10);
            Buzzer tone = (IPADLib.Buzzer)System.Convert.ToByte(textBoxParam3.Text, 10);
            byte typeData = GetTypeData();

            autoEvent.Reset();

            DisplayStatusResult("------------------------------------------------------", Color.Brown);
            DisplayStatusResult("Sending command to request Manual Card Entry.", Color.Black);

            if (Enum.GetName(typeof(FieldOptions), (byte)field) != null)
                str = String.Format("Testing IPAD.RequestCard({0:d}, FieldOptions.{1:s}, ",
                                    waitTime, Enum.GetName(typeof(FieldOptions), (byte)field));
            else
                str = String.Format("Testing IPAD.RequestCard({0:d}, {1:d}, ", waitTime, (byte)field);

            str += String.Format("UsePAN.{0:s}, SetQwickCodes.{1:s}, ",
                                 Enum.GetName(typeof(OptionStatus), bPANinPIN),
                                 Enum.GetName(typeof(OptionStatus), bQwickCodes));
            // Enum.GetName(typeof(UsePAN), (byte)((QwickCodes.Checked) ? (byte)1 : (byte)0)));



            if (Enum.GetName(typeof(Buzzer), (byte)tone) != null)
                str += String.Format("Buzzer.{0:s})", Enum.GetName(typeof(Buzzer), (byte)tone));
            else
                str += String.Format("{0:d})", tone);

            DisplayStatusResult(str, Color.Black);


            try
            {
                pp.RequestManualCardData(waitTime, typeData, tone);
            }
            catch (Exception ex)
            {
                String str1;
                str1 = String.Format("RequestCardError:  {0}", ex.Message);
                DisplayStatusResult(str1, Color.Red);
                return;
            }

            while (true)
            {
                Application.DoEvents();

                if (autoEvent.WaitOne(1000, false))
                {
                    Console.WriteLine("Work method signaled.");
                    break;
                }
            }
            
        }

        private void HideAllParams()
        {
            btnRun.Enabled = true;//HS  6/28/11

            labelParam1.Visible = false;
            labelParam2.Visible = false;
            labelParam3.Visible = false;
            labelParam4.Visible = false;
            labelParam5.Visible = false;
            labelParam6.Visible = false;
            lMsg.Visible = false; //HS  6/28/11
            
            textBoxParam1.Visible = false;
            textBoxParam2.Visible = false;
            textBoxParam3.Visible = false;
            textBoxParam4.Visible = false;
            textBoxParam5.Visible = false;
            textBoxParam6.Visible = false;
            textBoxMsg.Visible = false; //HS  6/28/11

            labelRangeParam1.Visible = false;
            labelRangeParam2.Visible = false;
            labelRangeParam3.Visible = false;
            labelRangeParam4.Visible = false;
            labelRangeParam5.Visible = false;
            labelRangeParam6.Visible = false;

            //HS  6/28/11 check boxes for sendmultidata selection
            QwickCodes.Visible = false;
            PANinPIN.Visible = false;
            BackgroundClr.Visible = false;
            Underline.Visible = false;            

            // HS  6/28/11 buttons for sendmultidata
            btnAcceptMsg.Visible = false;
            rBtnDispMsg.Visible = false;
            rBtnDispMsg.Visible = false;
            gBoxSelectReportType.Visible = false;

        }
        private void DisplayMSGParamInitialize()
        {
            HideAllParams();
            labelParam1.Text = "Wait Time (Seconds):";
            labelParam2.Text = "Message ID:";

            labelRangeParam1.Text = "(Valid Values: 0..255)";
            labelRangeParam2.Text = "(Valid Values: 0..8)";
            
            labelRangeParam1.Visible = true;
            labelRangeParam2.Visible = true;
            
            textBoxParam1.Text = "5";
            textBoxParam2.Text = "1";

            labelParam1.Visible = true;
            labelParam2.Visible = true;
            textBoxParam1.Visible = true;
            textBoxParam2.Visible = true;
        }

        

        private void RequestPINParamInitialize()
        {
            HideAllParams();
            labelParam1.Text = "Wait Time (Seconds):";
            labelParam2.Text = "PIN Message ID:";
            labelParam3.Text = "Min PIN Length:";
            labelParam4.Text = "Max PIN Length:";
            labelParam5.Text = "Tone:";
            labelParam6.Text = "Options:";


            labelRangeParam1.Text = "(Valid Values: 0..255)";
            labelRangeParam2.Text = "(Valid Values: 0..3)";
            labelRangeParam3.Text = "(Valid Values: 4..12)";
            labelRangeParam4.Text = "(Valid Values: 4..12)";
            labelRangeParam5.Text = "(Valid Values: 0..2)";
            labelRangeParam6.Text = "(Valid Values: 0..3)";

            labelRangeParam1.Visible = true;
            labelRangeParam2.Visible = true;
            labelRangeParam3.Visible = true;
            labelRangeParam4.Visible = true;
            labelRangeParam5.Visible = true;
            labelRangeParam6.Visible = true;

            textBoxParam1.Text = "10";
            textBoxParam2.Text = "0";
            textBoxParam3.Text = "4";
            textBoxParam4.Text = "8";
            textBoxParam5.Text = "1";
            textBoxParam6.Text = "0";


            labelParam1.Visible = true;
            labelParam2.Visible = true;
            labelParam3.Visible = true;
            labelParam4.Visible = true;
            labelParam5.Visible = true;
            labelParam6.Visible = true;
            textBoxParam1.Visible = true;
            textBoxParam2.Visible = true;
            textBoxParam3.Visible = true;
            textBoxParam4.Visible = true;
            textBoxParam5.Visible = true;
            textBoxParam6.Visible = true;
        }


        private void RequestCardParamInitialize()
        {
            HideAllParams();
            labelParam1.Text = "Wait Time (Seconds):";
            labelParam2.Text = "Request Card Message ID:";
            labelParam3.Text = "Tone:";

            textBoxParam1.Text = "200";
            textBoxParam2.Text = "1";
            textBoxParam3.Text = "0";

            labelRangeParam1.Text = "(Valid Values: 0..255)";
            labelRangeParam2.Text = "(Valid Values: 0..3)";
            labelRangeParam3.Text = "(Valid Values: 0..2)";
            labelRangeParam1.Visible = true;
            labelRangeParam2.Visible = true;
            labelRangeParam3.Visible = true;

            labelParam1.Visible = true;
            labelParam2.Visible = true;
            labelParam3.Visible = true;
            textBoxParam1.Visible = true;
            textBoxParam2.Visible = true;
            textBoxParam3.Visible = true;
        }
        private void RequestCard()
        {
            if (pp == null)
            {
                MessageBox.Show("Device is not connected");
                return;
            }

            string str;
            CardMsg msg = (IPADLib.CardMsg)System.Convert.ToByte(textBoxParam2.Text, 10);
            byte waitTime = System.Convert.ToByte(textBoxParam1.Text, 10);
            Buzzer tone = (IPADLib.Buzzer)System.Convert.ToByte(textBoxParam3.Text, 10);



            autoEvent.Reset();

            DisplayStatusResult("------------------------------------------------------", Color.Brown);
            DisplayStatusResult("Sending command to request Card Swipe.", Color.Black);

            if (Enum.GetName(typeof(DisplayMsg), (byte)msg) != null)
                str = String.Format("Testing IPAD.RequestCard({0:d}, DisplayMsg.{1:s}, ", waitTime, Enum.GetName(typeof(CardMsg), (byte)msg), tone);
            else
                str = String.Format("Testing IPAD.RequestCard({0:d}, {1:d}, ", waitTime, (byte)msg, tone);

            if (Enum.GetName(typeof(Buzzer), (byte)tone) != null) 
                str += String.Format("Buzzer.{0:s})", Enum.GetName(typeof(Buzzer), (byte)tone));
            else
                str += String.Format("{0:d})", tone);

            DisplayStatusResult(str, Color.Black);


            try
            {
                pp.RequestCard(waitTime, msg, tone);
            }
            catch (Exception ex)
            {
                String str1;
                str1 = String.Format("RequestCardError:  {0}", ex.Message);
                DisplayStatusResult(str1, Color.Red);
                return ;
            }
            
            while (true)
            {
                Application.DoEvents();

                if (autoEvent.WaitOne(1000, false))
                {
                    Console.WriteLine("Work method signaled.");
                    break;
                }
            }

        }


        private void SelectCreditDebitParamInitialize()
        {
            HideAllParams();
            labelParam1.Text = "Wait Time (Seconds):";
            labelParam2.Text = "Tone:";

            textBoxParam1.Text = "20";
            textBoxParam2.Text = "0";

            labelRangeParam1.Text = "(Valid Values: 0..255)";

            labelRangeParam2.Text = "(Valid Values: 0..2)";
            labelRangeParam1.Visible = true;
            labelRangeParam2.Visible = true;

            labelParam1.Visible = true;
            labelParam2.Visible = true;
            textBoxParam1.Visible = true;
            textBoxParam2.Visible = true;
        }

        private void ConfirmAmountParamInitialize()
        {
            HideAllParams();
            labelParam1.Text = "Wait Time (Seconds):";
            labelParam2.Text = "Tone:";

            textBoxParam1.Text = "20";
            textBoxParam2.Text = "0";

            labelRangeParam1.Text = "(Valid Values: 0..255)";

            labelRangeParam2.Text = "(Valid Values: 0..2)";
            labelRangeParam1.Visible = true;
            labelRangeParam2.Visible = true;

            labelParam1.Visible = true;
            labelParam2.Visible = true;
            textBoxParam1.Visible = true;
            textBoxParam2.Visible = true;
        }

        private void GetSelectionParamInitialize()
        {
            HideAllParams();
            labelParam1.Text = "Wait Time (Seconds):";
            labelParam2.Text = "Response Message ID:";
            labelParam3.Text = "Key Mask:";
            labelParam4.Text = "Tone:";

            textBoxParam1.Text = "20";
            textBoxParam2.Text = "0";
            textBoxParam3.Text = "5";
            textBoxParam4.Text = "0";

            labelRangeParam1.Text = "(Valid Values: 0..255)";
            labelRangeParam2.Text = "(Valid Values: 0..1)";
            labelRangeParam3.Text = "(Valid Bits: Left=0, Middle=1, Right=2, Enter=4)";
            labelRangeParam4.Text = "(Valid Values: 0..2)";
            labelRangeParam1.Visible = true;
            labelRangeParam2.Visible = true;
            labelRangeParam3.Visible = true;
            labelRangeParam4.Visible = true;

            labelParam1.Visible = true;
            labelParam2.Visible = true;
            labelParam3.Visible = true;
            labelParam4.Visible = true;
            textBoxParam1.Visible = true;
            textBoxParam2.Visible = true;
            textBoxParam3.Visible = true;
            textBoxParam4.Visible = true;
        }
      


        private void ConfirmAmount()
        {
            if (pp == null)
            {
                MessageBox.Show("Device is not connected");
                return;
            }

            String str;
            byte waitTime = System.Convert.ToByte(textBoxParam1.Text, 10);
            Buzzer tone = (Buzzer)System.Convert.ToByte(textBoxParam2.Text, 10);

            DisplayStatusResult("------------------------------------------------------", Color.Brown);
            DisplayStatusResult("Sending command to confirm amount.", Color.Black);


            if (tone.ToString() != null)
                str = String.Format("Testing IPAD.ConfirmAmount({0:d}, ResponseMsg.{1:s})", waitTime, tone.ToString());
            else
                str = String.Format("Testing IPAD.ConfirmAmount({0:d}, {1:d})", waitTime, (byte)tone);


            DisplayStatusResult(str, Color.Black);
            try
            {
              //  pp.SendAmount(AmountType.Debit, "12345.67");
                pp.ConfirmAmount(waitTime, tone);
            }
            catch (Exception ex)
            {
                String str1;
                str1 = String.Format("ConfirmAmtError:  {0}", ex.Message);
                DisplayStatusResult(str1, Color.Red);
            }
        }

        private void SelectCreditDebit()
        {
            if (pp == null)
            {
                MessageBox.Show("Device is not connected");
                return;
            }

            String str;
            byte waitTime = System.Convert.ToByte(textBoxParam1.Text, 10);
            Buzzer tone = (Buzzer)System.Convert.ToByte(textBoxParam2.Text, 10);

            DisplayStatusResult("------------------------------------------------------", Color.Brown);
            DisplayStatusResult("Sending command to get credit/debit selection.", Color.Black);


            if (tone.ToString() != null)
                str = String.Format("Testing IPAD.SelectCreditDebit({0:d}, ResponseMsg.{1:s})", waitTime, tone.ToString());
            else
                str = String.Format("Testing IPAD.SelectCreditDebit({0:d}, {1:d})", waitTime, (byte)tone);


            DisplayStatusResult(str, Color.Black);
            try
            {
                pp.SelectCreditDebit(waitTime, tone);
            }
            catch (Exception ex)
            {
                String str1;
                str1 = String.Format("Select C/D:  {0}", ex.Message);
                DisplayStatusResult(str1, Color.Red);
            }
        }

        private void GetSelection()
        {
            if (pp == null)
            {
                MessageBox.Show("Device is not connected");
                return;
            }

            String str;
            byte waitTime = System.Convert.ToByte(textBoxParam1.Text, 10);
            ResponseMsg msg = (IPADLib.ResponseMsg)System.Convert.ToByte(textBoxParam2.Text, 10);
            KeyMask mask = (IPADLib.KeyMask)System.Convert.ToByte(textBoxParam3.Text, 10);
            Buzzer tone = (Buzzer) System.Convert.ToByte(textBoxParam4.Text, 10);

            DisplayStatusResult("------------------------------------------------------", Color.Brown);
            DisplayStatusResult("Sending command to request key selection.", Color.Black);


            if (msg.ToString() != null)
                str = String.Format("Testing IPAD.GetResponse({0:d}, ResponseMsg.{1:s}, ", waitTime, msg.ToString());
            else
                str = String.Format("Testing IPAD.GetResponse({0:d}, {1:d}, ", waitTime, (byte)msg);

            if (mask.ToString () != null)
                str += String.Format("KeyMask.{0:s}, ", mask.ToString());
            else
                str += String.Format("{0:d}, ", (byte)mask);

            if (tone.ToString () != null)
                str += String.Format("Buzzer.{0:s})", tone.ToString());
            else
                str += String.Format("{0:d})", tone);


            DisplayStatusResult(str, Color.Black);
            try
            {
                if (msg == ResponseMsg.AmountOk)
                {
                    pp.SendAmount(AmountType.Credit, "1245.67");
                    pp.GetResponse(waitTime, msg, mask, tone);
                }
                else
                    pp.GetResponse(waitTime, msg, mask, tone);
            }
            catch (Exception ex)
            {
                String str1;
                str1 = String.Format("GetResponseError:  {0}", ex.Message);
                DisplayStatusResult(str1, Color.Red);
            }

        }


        private void HaltOperation()
        {
            if (pp == null)
            {
                MessageBox.Show("Device is not connected");
                return;
            }

            DisplayStatusResult("------------------------------------------------------", Color.Brown);
            DisplayStatusResult("Sending command to cancel previous command.", Color.Black);
            pp.CancelOperation();
        }

        private void GetStatus()
        {
            if (pp == null)
            {
                MessageBox.Show("Device is not connected");
                return;
            }

            DisplayStatusResult("------------------------------------------------------", Color.Brown);
            DisplayStatusResult("Sending command to get device status.", Color.Black);
            pp.GetStatus();
        }

        private void EndSession()
        {
            if (pp == null)
            {
                MessageBox.Show("Device is not connected");
                return;
            }

            DisplayStatusResult("------------------------------------------------------", Color.Brown);
            DisplayStatusResult("Sending command to end session.", Color.Black);

            pp.EndSession();
        }

        //private string GetAllPossibleValues(Type en)
        //{
        //    string str = "";
        //    Type type = en.GetType();
        //    int len = Enum.GetValues(type).Length;
        //    foreach (Enum i in Enum.GetValues(type))
        //    {
        //        len--;
        //        if (len != 0)
        //            str += string.Format("{0:d}, ", i);
        //        else
        //            str += string.Format("{0:d} ", i);
        //    }
        //    return str;


        //}
        private void SendAmountParamInitialize()
        {
            string str = "";
            HideAllParams();
            labelParam1.Text = "Amount Type:";
            labelParam2.Text = "Amount:";
            //AmountType Amttype;

            //str = GetAllPossibleValues((System.Type)Amttype);
            

            int len = Enum.GetValues(typeof(AmountType)).Length;
            foreach (AmountType i in Enum.GetValues(typeof(AmountType)))
            {
                len--;
                if (len != 0)
                    str += string.Format("{0:d}, ", i);
                else
                    str += string.Format("{0:d} ", i);
            }

            

            labelRangeParam1.Text = "(Valid Values: " + str + ")";
            labelRangeParam2.Text = "(Valid Values: Max. length 11 characters including '.', which is not required if there is no cents)";

            labelRangeParam1.Visible = true;
            labelRangeParam2.Visible = true;

            textBoxParam1.Text = "0";
            
            textBoxParam2.Text = "1234.56";

            labelParam1.Visible = true;
            labelParam2.Visible = true;
            textBoxParam1.Visible = true;
            textBoxParam2.Visible = true;
        }
        private void SendAmount()
        {
            if (pp == null)
            {
                MessageBox.Show("Device is not connected");
                return;
            }

            string str;

            AmountType type = (IPADLib.AmountType)System.Convert.ToByte(textBoxParam1.Text, 10);
            DisplayStatusResult("------------------------------------------------------", Color.Brown);
            DisplayStatusResult("Sending command to set amount.", Color.Black);

            if (type.ToString() != null)
                str = String.Format("Testing IPAD.SendAmount(AmountType.{0:s}, {1:s})", type.ToString(), textBoxParam2.Text);
            else
                str = String.Format("Testing IPAD.SendAmount({0:d}, {1:d})", type, textBoxParam2.Text);

            DisplayStatusResult(str, Color.Black);

            try
            {
                pp.SendAmount(type, textBoxParam2.Text);
            }
            catch (Exception ex)
            {
                String str1;
                str1 = String.Format("SndAmtError:  {0}", ex.Message);
                DisplayStatusResult(str1, Color.Red);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            richTextBoxEventResult.Clear();
            richTextBoxStatus.Clear();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ddlCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            String str;

            str = ddlCommands.SelectedItem.ToString();
            switch (str) //ddlCommands.SelectedIndex
            {
                case "Display Msg": //"Display Msg"
                DisplayMSGParamInitialize();
                btnValidate.Enabled = true;
                break;

            case "Send MultiData": // HS 6/22/11
                SendMultiDataInitialize();
                btnRun.Enabled = false;
                break;

            case "Request PIN": //"Request PIN"
                RequestPINParamInitialize();
                btnValidate.Enabled = true;
                break;

            case "Request Card":  //"Request Card"
                RequestCardParamInitialize();
                btnValidate.Enabled = true;
                break;

            case "Request Manual Card Entry": // HS 6/22/11
                RequestManualParamInitialize();
                btnValidate.Enabled = true;
                break;

            case "Get Response": //"Get Key Input"
                GetSelectionParamInitialize();
                btnValidate.Enabled = true;
                break;

            case "Confirm Amount": //"Confirm Amount"
                ConfirmAmountParamInitialize();
                btnValidate.Enabled = false;
                break;

            case "Select Credit Debit": //"Select Credit Debit"
                SelectCreditDebitParamInitialize();
                btnValidate.Enabled = false;
                break;

            case "Halt Operation":  //"Halt Operation"
                HideAllParams();
                btnValidate.Enabled = false;
                break;

            case "End Session": //"End Session"
                HideAllParams();
                btnValidate.Enabled = false;
                break;

            case "Send Amount": //"Send amount"
                SendAmountParamInitialize();
                btnValidate.Enabled = true;
                break;

            case "Get Status": //"Get Status"
                HideAllParams();
                btnValidate.Enabled = false;
                break;
            default:
                break;
        }
    }

        private void RequestManualParamInitialize()
        {
            HideAllParams();
            labelParam1.Text = "Wait Time(Sec):";
            labelParam2.Text = "Field Options:";
            labelParam3.Text = "Tone:";

            textBoxParam1.Text = "200";
            textBoxParam2.Text = "0";

            textBoxParam3.Text = "0";

            labelRangeParam1.Text = "(Valid Values: 0..255)";
            labelRangeParam2.Text = "(Valid Values: 0..3)";
            //labelRangeParam2.Text = "(Valid Values: 0-Acct,Date,CVC; 1-Acct,Date; 2-Acct,CVC; 3-Acct)";
            labelRangeParam3.Text = "(Valid Values: 0..2)";
            labelRangeParam1.Visible = true;
            labelRangeParam2.Visible = true;
            labelRangeParam3.Visible = true;

            labelParam1.Visible = true;
            labelParam2.Visible = true;
            labelParam3.Visible = true;
            textBoxParam1.Visible = true;
            textBoxParam2.Visible = true;
            textBoxParam3.Visible = true;
            QwickCodes.Visible = true;
            PANinPIN.Visible = true;
        }

        private void SendMultiDataInitialize()
        {
            HideAllParams();
            labelParam1.Text = "Total Msg(s) #:";
            labelParam2.Text = "X axis location:";
            labelParam3.Text = "Y axis location:";
            labelParam4.Text = "Spacing:";
            labelParam5.Text = "Alignment:";
            labelParam6.Text = "Font Size:";


            labelRangeParam1.Text = "(Valid Values: 0..255)";
            labelRangeParam2.Text = "(Valid Values: 0..127)";
            labelRangeParam3.Text = "(Valid Values: 0..63)";
            labelRangeParam4.Text = "(Valid Values: 0..2)";
            labelRangeParam5.Text = "(Valid Values: 0..2)";
            labelRangeParam6.Text = "(Valid Values: 0..2)";

            labelRangeParam1.Visible = true;
            labelRangeParam2.Visible = true;
            labelRangeParam3.Visible = true;
            labelRangeParam4.Visible = true;
            labelRangeParam5.Visible = true;
            labelRangeParam6.Visible = true;

            textBoxParam1.Text = "1";
            textBoxParam2.Text = "64";
            textBoxParam3.Text = "56";
            textBoxParam4.Text = "0";
            textBoxParam5.Text = "1";
            textBoxParam6.Text = "1";


            labelParam1.Visible = true;
            labelParam2.Visible = true;
            labelParam3.Visible = true;
            labelParam4.Visible = true;
            labelParam5.Visible = true;
            labelParam6.Visible = true;
            lMsg.Visible = true;
            lMsg.Text = "Display Text Message # " + MsgID;

            textBoxParam1.Visible = true;
            textBoxParam2.Visible = true;
            textBoxParam3.Visible = true;
            textBoxParam4.Visible = true;
            textBoxParam5.Visible = true;
            textBoxParam6.Visible = true;
            textBoxMsg.Visible = true;

            BackgroundClr.Visible = true;
            Underline.Visible = true;
            btnAcceptMsg.Visible = true;
            btnRun.Enabled = false;
            rBtnDispMsg.Visible = true;
            rBtnDispMsg.Visible = true;
            gBoxSelectReportType.Visible = true;

            rBtnReqUsrSelect.Checked = true;
        }

        private bool WaitForCommandToFinish()
        {
            while (true)
            {
                Application.DoEvents();

                if (autoEvent.WaitOne(1000, false))
                {
                    Console.WriteLine("Work method signaled.");
                    break;
                }

                if (cancelTestEvent.WaitOne(1, false))
                {
                    if (pp != null)
                        DisplayResult("**********Process cancelled by user.");
                    else
                        DisplayResult("**********IPAD has been disconnected.");
                    return false;
                }

            }

            return true;

        }
        private bool IssueRequestCard(byte waitTime, CardMsg msg, Buzzer tone, bool Wait)
        {
            if (pp == null)
            {
                MessageBox.Show("Device is not connected");
                return false;
            }

            string str;
           
            //CardMsg msg = (IPADLib.CardMsg)System.Convert.ToByte(textBoxParam2.Text, 10);
            //byte waitTime = System.Convert.ToByte(textBoxParam1.Text, 10);
            //Buzzer tone = (IPADLib.Buzzer)System.Convert.ToByte(textBoxParam3.Text, 10);



            autoEvent.Reset();

            DisplayStatusResult("------------------------------------------------------", Color.Brown);
            DisplayStatusResult("Sending command to request Card Swipe.", Color.Black);

            if (Enum.GetName(typeof(DisplayMsg), (byte)msg) != null)
                str = String.Format("Testing IPAD.RequestCard({0:d}, DisplayMsg.{1:s}, ", waitTime, Enum.GetName(typeof(CardMsg), (byte)msg), tone);
            else
                str = String.Format("Testing IPAD.RequestCard({0:d}, {1:d}, ", waitTime, (byte)msg, tone);

            if (Enum.GetName(typeof(Buzzer), (byte)tone) != null)
                str += String.Format("Buzzer.{0:s})", Enum.GetName(typeof(Buzzer), (byte)tone));
            else
                str += String.Format("{0:d})", tone);

            DisplayStatusResult(str, Color.Black);


            try
            {
                pp.RequestCard(waitTime, msg, tone);
            }
            catch (Exception ex)
            {
                String str1;
                str1 = String.Format("RequestCardError:  {0}", ex.Message);
                DisplayStatusResult(str1, Color.Red);
                return false;
            }

            if (Wait)
            {
                while (true)
                {
                    Application.DoEvents();

                    if (autoEvent.WaitOne(1000, false))
                    {
                        Console.WriteLine("Work method signaled.");
                        break;
                    }
                }
                if (cancelTestEvent.WaitOne(1, false))
                {
                    if (pp != null)
                        DisplayResult("**********Process cancelled by user.");
                    else
                        DisplayResult("**********IPAD has been disconnected.");
                    return false;
                }
            }
            return true;
        }
        private bool IssueRequestPIN(byte waitTime, PinMsg pinMsg, byte min, byte max, Buzzer tones, byte options, bool Wait)
        {
            if (pp == null)
            {
                MessageBox.Show("Device is not connected");
                return false;
            }

            string str;

            autoEvent.Reset();
            DisplayStatusResult("------------------------------------------------------", Color.Brown);

            if (pinMsg.ToString() != null)
                str = String.Format("Testing IPAD.RequestPIN({0:d}, DisplayMsg.{1:s}, {2:d}, {3:d}", waitTime, pinMsg.ToString(), min, max, Enum.GetName(typeof(Buzzer), (byte)tones));

            else
                str = String.Format("Testing IPAD.RequestPIN({0:d}, {1:d}, {2:d}, {3:d}", waitTime, pinMsg.ToString(), min, max, Enum.GetName(typeof(Buzzer), (byte)tones));
            if (tones.ToString() != null)
                str += String.Format(", Buzzer.{0:s})", tones.ToString());
            else
                str += String.Format(", Buzzer.{0:d})", tones);

            DisplayStatusResult(str, Color.Black);
            try
            {
              pp.RequestPIN(waitTime, pinMsg, min, max, tones, options);
            }
            catch (Exception ex)
            {
                String str1;
                str1 = String.Format("RequestPINError:  {0}", ex.Message);
                DisplayStatusResult(str1, Color.Red);
                return false;
            }
            if (Wait == true)
                return WaitForCommandToFinish();
            else
                return true;

        }

        private void RequestPIN()
        {
            IssueRequestPIN(System.Convert.ToByte(textBoxParam1.Text, 10), (IPADLib.PinMsg)System.Convert.ToByte(textBoxParam2.Text, 10), System.Convert.ToByte(textBoxParam3.Text, 10), System.Convert.ToByte(textBoxParam4.Text, 10), (IPADLib.Buzzer)System.Convert.ToByte(textBoxParam5.Text, 10),System.Convert.ToByte(textBoxParam6.Text, 10), false);
        }

        private bool IssueDisplay(byte waitTime, DisplayMsg msg, bool Wait)
        {
            if (pp == null)
            {
                MessageBox.Show("Device is not connected");
                return false;
            }

            string str;
            autoEvent.Reset();                        
            DisplayStatusResult("------------------------------------------------------", Color.Brown);
            
            if (Enum.GetName(typeof (DisplayMsg), (byte) msg) != null)
                str = String.Format("Testing IPAD.Display({0:x}, DisplayMsg.{1:s})", waitTime, Enum.GetName(typeof (DisplayMsg), (byte) msg));
            else
                str = String.Format("Testing IPAD.Display({0:x}, {1:x})", waitTime, (byte)msg);
            DisplayStatusResult(str, Color.Black);
            try
            {
                pp.Display(waitTime, msg);
            }
            catch (Exception ex)
            {
                String str1;
                str1 = String.Format("DisplayError:  {0}", ex.Message);
                DisplayStatusResult(str1, Color.Red);
                return false;
            }
            if (Wait == true)
                return WaitForCommandToFinish();
            else
                return true;

        }

         private void DisplayMSGTests()
        {
            if (IssueDisplay(1, DisplayMsg.Approved, true) == false)
                return;

            if (IssueDisplay(1, DisplayMsg.Blank, true) == false)
                return;

            if (IssueDisplay(1, DisplayMsg.Cancelled, true) == false)
                return;
            if (IssueDisplay(1, DisplayMsg.Declined, true) == false)
                return;
            if (IssueDisplay(1, DisplayMsg.PINInvalid, true) == false)
                return;
            if (IssueDisplay(1, DisplayMsg.PleaseWait, true) == false)
                return;
            if (IssueDisplay(1, DisplayMsg.Processing, true) == false)
                return;
            if (IssueDisplay(1, DisplayMsg.ThankYou, true) == false)
                return;
            if (IssueDisplay(1, DisplayMsg.HandsOff, true) == false)
                return;

            DisplayStatusResult("Sending invalid DisplayMsg id to Dislplay()", Color.Black);
            if (IssueDisplay(5, (DisplayMsg)9, true) != false)
                return;
            
            if (IssueDisplay(5, DisplayMsg.Approved, false) == false)
                return;

            DisplayStatusResult("Sending RequestPIN() while device is not idle.", Color.Black);
            IssueRequestPIN(20, IPADLib.PinMsg.EnterPin, 4, 4, Buzzer.None, 0, false);
            //pp.RequestPIN(10, PinMsg.EnterPin, 4, 4, Buzzer.None, 0);

            IssueDisplay(5, DisplayMsg.Declined, false);
            //IssueDisplayWithoutEvent(10, DisplayMsg.Declined, false);
            DisplayStatusResult("------------------------------------------------------", Color.Brown);
            DisplayStatusResult("Finished testing Display()", Color.Black);
        }
        private void RequestPINTests()
        {
            IssueRequestPIN(20, IPADLib.PinMsg.EnterPin, 4, 4, Buzzer.SingleBeep, 0, true);
            IssueRequestPIN(20, IPADLib.PinMsg.EnterPinAmt, 4, 5, Buzzer.DoubleBeep, 0, true);
            IssueRequestPIN(20, IPADLib.PinMsg.ReenterPIN, 4, 11, Buzzer.SingleBeep, 0, true);
            IssueRequestPIN(20, IPADLib.PinMsg.ReenterPINAmt, 4, 12, Buzzer.DoubleBeep, 0, true);
            IssueRequestPIN(20, IPADLib.PinMsg.VerifyPIN, 4, 5, Buzzer.SingleBeep, 0, true);
        }
        private void RequestCardTests()
        {
           
             IssueRequestCard(20, IPADLib.CardMsg.SwipeCard, Buzzer.SingleBeep, true);
             IssueRequestCard(20, IPADLib.CardMsg.PleaseSwipeCard, Buzzer.DoubleBeep, true);
             
             IssueRequestCard(20, IPADLib.CardMsg.PleaseSwipeAgain, Buzzer.None, false);
             DisplayStatusResult("Sending RequestCard() while device is not idle.", Color.Red);
             IssueRequestCard(20, IPADLib.CardMsg.SwipeCard, Buzzer.None, false);

        }
        private void btnValidate_Click(object sender, EventArgs e)
        {

     
            if (btnValidate.Text == "Validate")
            {
                btnValidate.Text = "Cancel Process";
                cancelTestEvent.Reset();
                switch (ddlCommands.SelectedItem.ToString())
                {
                    case "Display Msg": //"Display Msg"
                        DisplayMSGTests();
                        break;

                    case "Request PIN": //"Request PIN"
                        RequestPINTests();
                        break;

                    case "Request Card":  //"Request Card"
                        RequestCardTests();
                        //RequestCardParamInitialize();
                        break;

                    case "Get Response": //"Get Key Inpu"
                        //GetSelectionParamInitialize();
                        break;

                    case "Halt Operation":  //"Halt Operation"
                        //HideAllParams();
                        break;

                    case "End Session": //"End Session"
                        //HideAllParams();
                        break;

                    case "Send Amount": //"Send amount"
                        //SendAmountParamInitialize();
                        break;

                    case "Get Status": //"Get Status"
                        //HideAllParams();
                        break;
                    default:
                        break;
                }
                //pp.RequestCard (20, IPADLib.CardMsg.SwipeCard, 0);
                btnValidate.Text = "Validate";

            }
            else
            {
                cancelTestEvent.Set();
            }
        
        }
        private byte GetMsgType()
        {
            int b = (BackgroundClr.Checked) ? 0x80 : 0;        // backround cleared or unchanged
            b |= (Underline.Checked) ? 0x40 : 0;                 // Underlined
            b |= Convert.ToByte(textBoxParam4.Text, 10) * 16;  // Spacing 1,2, 3
            b |= Convert.ToByte(textBoxParam5.Text, 10) * 4;   // Alignment 1,2,3
            b |= Convert.ToByte(textBoxParam6.Text, 10);
            return (byte)b;
        }
        private void ClearMsgData()
        {
            lMsg.Text = "Display Text Message # " + (MsgID + 1);
            textBoxMsg.Text = "";
        }
        private void btnAcceptMsg_Click(object sender, EventArgs e)
        {
            MsgNo = System.Convert.ToByte(textBoxParam1.Text, 10);
            byte msgType;
            if (rBtnReqUsrSelect.Checked) msgType = 0x06;
            else msgType = 0x07;
            if (MsgNo > 0)
            {
                if (MsgID == 1)
                {
                    textBoxParam1.Enabled = false;  //prevent from modifying MsgNO

                    // generate MemoryStream m;
                    //MemoryStream ms = new MemoryStream();   global value
                    ms = new MemoryStream();
                    ms.WriteByte((byte)MsgNo);

                }


                // convert dialog box parameters to bytes
                // add parameters to array of MsgData type;

                //  IPAD.MsgData[] MsgBlock  = new IPAD.MsgData[10] ;
                // MsgBlock[0] = new IPAD.MsgData();
                IPAD.MsgData MsgBlock = new IPAD.MsgData();


                MsgBlock.X = Convert.ToByte(textBoxParam2.Text, 10);
                MsgBlock.Y = Convert.ToByte(textBoxParam3.Text, 10);
                MsgBlock.MsgType = GetMsgType();  //compose byte[3] background | underline | spacing & alignment & font
                MsgBlock.Msg = textBoxMsg.Text;

                // Display Selection 
                string str;
                byte bUnderline = (Underline.Checked) ? (byte)1 : (byte)0;
                byte bBackground = (BackgroundClr.Checked) ? (byte)1 : (byte)0;
                Spacing SpOpt = (IPADLib.Spacing)Convert.ToByte(textBoxParam4.Text, 10);
                Alignment AlOpt = (IPADLib.Alignment)Convert.ToByte(textBoxParam5.Text, 10);
                FontSize FtOpt = (IPADLib.FontSize)Convert.ToByte(textBoxParam6.Text, 10);

                autoEvent.Reset();

                DisplayStatusResult("------------------------------------------------------", Color.Brown);
                DisplayStatusResult("Composing Message Block Data", Color.Black);

                str = String.Format("Message Block #{0:d} ({1:d}, X={2:d}, Y={3:d}, (MsgType.{4:d}: BackroundClr.{5:s}, Underline.{6:s}, ",
                                    MsgID, (byte)(textBoxMsg.Text.Length + 4),
                                    MsgBlock.X, MsgBlock.Y, MsgBlock.MsgType,
                                    Enum.GetName(typeof(OptionStatus), bBackground),
                                    Enum.GetName(typeof(OptionStatus), bUnderline));

                str += String.Format("Spacing.{0:s}, Alignment.{1:s}, FontSize.{2:s}),0,{3:s})",
                                  Enum.GetName(typeof(Spacing), SpOpt),
                                  Enum.GetName(typeof(Alignment), AlOpt),
                                  Enum.GetName(typeof(FontSize), FtOpt),
                                  MsgBlock.Msg);


                DisplayStatusResult(str, Color.Black);
                // End of Display Selection


                // generate string of data - call  addUserString (MemoryStream m, MsgData MsgData);
                MsgBlock.addUserString(ms);
                // if need a next message to compose, clean screen, show dialog boxs for next message
                // and start from the begining;
                ClearMsgData();

                if (MsgID < MsgNo)
                    MsgID++;   // next Message to compose
                else             // if (MsgID == MsgNo)
                {
                    // if last MsgID, send command to device,
                    // deactivate AcceptMsg btn and activate RUN or
                    //  replace AcceptMsg with Send BigBlockData/RUN message on a button
                    DisplayStatusResult("Sending MultiBlock Data for Report = " + msgType.ToString(), Color.Black, FontStyle.Bold);
                    pp.SendMultiData(msgType, ms.ToArray());  //6 -for Request User Select, 7- for Disp Message
                    MsgID = 1;
                    textBoxParam1.Enabled = true;
                    ms.Dispose();

                }


            }


        }

        #region Mercury Methods for E2E and MToken

        private void RunKeyedTransaction(string encTrack2, string ksn)
        {
            invoiceNo = DateTime.Now.ToString("yyMMddhhmmssfff");

            // Create Request KeyValuePairs
            Dictionary<string, object> requestDictionary = new Dictionary<string, object>();
            requestDictionary.Add("MerchantID", merchantID);
            requestDictionary.Add("TranType", "Credit");
            requestDictionary.Add("TranCode", "Sale");
            requestDictionary.Add("InvoiceNo", invoiceNo);
            requestDictionary.Add("RefNo", invoiceNo);
            requestDictionary.Add("Memo", memo);
            requestDictionary.Add("Frequency", "OneTime");
            requestDictionary.Add("RecordNo", "RecordNumberRequested");
            requestDictionary.Add("PartialAuth", "Allow");
            requestDictionary.Add("EncryptedFormat", "MagneSafe");
            requestDictionary.Add("AccountSource", "Keyed");
            requestDictionary.Add("EncryptedBlock", encTrack2);
            requestDictionary.Add("EncryptedKey", ksn);
            requestDictionary.Add("Purchase", purchase);
            requestDictionary.Add("CVVData", cvvData);
            requestDictionary.Add("OperatorID", operatorID);

            // Process Transaction and get Response KeyValuePairs           
            Dictionary<string, string> responseDictionary = ProcessCreditTransaction(requestDictionary, password);

            DisplayResponseKeyValuePairs(responseDictionary, "Credit", "Sale (Keyed)");
        }

        private void RunSwipedTransaction(string encTrack2, string ksn)
        {
            invoiceNo = DateTime.Now.ToString("yyMMddhhmmssfff");

            // Create Request KeyValuePairs
            Dictionary<string, object> requestDictionary = new Dictionary<string, object>();
            requestDictionary.Add("MerchantID", merchantID);
            requestDictionary.Add("TranType", "Credit");
            requestDictionary.Add("TranCode", "Sale");
            requestDictionary.Add("InvoiceNo", invoiceNo);
            requestDictionary.Add("RefNo", invoiceNo);
            requestDictionary.Add("Memo", memo);
            requestDictionary.Add("Frequency", "OneTime");
            requestDictionary.Add("RecordNo", "RecordNumberRequested");
            requestDictionary.Add("PartialAuth", "Allow");
            requestDictionary.Add("EncryptedFormat", "MagneSafe");
            requestDictionary.Add("AccountSource", "Swiped");
            requestDictionary.Add("EncryptedBlock", encTrack2);
            requestDictionary.Add("EncryptedKey", ksn);
            requestDictionary.Add("Purchase", purchase);
            requestDictionary.Add("CVVData", cvvData);
            requestDictionary.Add("OperatorID", operatorID);

            // Process Transaction and get Response KeyValuePairs           
            Dictionary<string, string> responseDictionary = ProcessCreditTransaction(requestDictionary, password);

            DisplayResponseKeyValuePairs(responseDictionary, "Credit", "Sale (Swiped)");
        }

        private Dictionary<string, string> ProcessCreditTransaction(Dictionary<string, object> requestDictionary, string password)
        {
            // Build XML Request from KeyValuePairs
            string xmlRequest = XMLHelper.BuildXMLRequest(requestDictionary).ToString();
            string xmlResponse = string.Empty;

            using (IPADTest.MercuryWebServices.wsSoapClient client = new IPADTest.MercuryWebServices.wsSoapClient())
            {
                Console.WriteLine("Processing Credit Transaction...");
                xmlResponse = client.CreditTransaction(xmlRequest, password);
            }

            // Parse XML Response into KeyValuePairs
            return XMLHelper.ParseXMLResponse(xmlResponse);
        }

        private void DisplayResponseKeyValuePairs(Dictionary<string, string> responseDictionary, string tranType, string tranCode)
        {
            DisplayStatusResult(string.Format("--- Response Key Value Pairs for {0} {1} ---", tranType, tranCode), Color.Blue, FontStyle.Bold);

            foreach (KeyValuePair<string, string> kvp in responseDictionary)
            {
                DisplayStatusResult(string.Format("{0}:{1};", kvp.Key, kvp.Value), Color.Blue);
            }
        }

        #endregion Mercury Methods for E2E and MToken
    }
}