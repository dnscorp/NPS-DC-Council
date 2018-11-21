using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common
{
    public partial class PopupCtrl : System.Web.UI.UserControl
    {
        #region Event Handlers
        public event EventHandler OkayButtonClick;
        public event EventHandler CancelButtonClick;
        #endregion

        public string HeadingText
        {
            get
            {
                return hfHeadingText.Value;
            }
            set
            {
                hfHeadingText.Value = value;
            }
        }
        public string Message
        {
            get
            {
                return hfMessage.Value;
            }
            set
            {
                hfMessage.Value = value;
            }
        }
        public string IdToProcess
        {
            get
            {
                return hfIdToProcess.Value;
            }
            set
            {
                hfIdToProcess.Value = value;
            }
        }
        public string Mode
        {
            get
            {
                return hfMode.Value;
            }
            set
            {
                hfMode.Value = value;
            }
        }

        public string OkayButtonText
        {
            get
            {
                return hfOkayButtonText.Value;
            }
            set
            {
                hfOkayButtonText.Value = value;
            }
        }
        public string CancelButtonText
        {
            get
            {
                return hfCancelButtonText.Value;
            }
            set
            {
                hfCancelButtonText.Value = value;
            }
        }

        public bool ShowOkayButton
        {
            get
            {
                if (string.IsNullOrEmpty(hfShowOkayButton.Value))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                if (value)
                {
                    hfShowOkayButton.Value = "1";
                }
                else
                {
                    hfShowOkayButton.Value = string.Empty;
                }
            }
        }
        protected void OnLnkOkayClick(object sender, EventArgs e)
        {
            if (OkayButtonClick != null)
                OkayButtonClick(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            litHeading.Text = HeadingText;
            litConfirmationMessage.Text = Message;
            lnkOkay.Attributes.Add("IdToProcess", IdToProcess);
            lnkOkay.Attributes.Add("Mode", Mode);
            litOkayButtonText.Text = OkayButtonText;
            litCancelButtonText.Text = CancelButtonText;
            if (ShowOkayButton)
            {
                lnkOkay.Visible = true;
            }
            else
            {
                lnkOkay.Visible = false;
            }
        }

        protected void lnkCancel_ServerClick(object sender, EventArgs e)
        {
            if (CancelButtonClick != null)
                CancelButtonClick(this, e);
        }

        internal void SetProperties(string strIdToProcess,bool blnShowOkayButton,string strOkayButtonText,string strCanceButtonText,string strHeadingText,string strMessage, string strMode)
        {
            this.IdToProcess = strIdToProcess;
            this.ShowOkayButton = blnShowOkayButton;
            this.OkayButtonText = strOkayButtonText;
            this.CancelButtonText = strCanceButtonText;
            this.HeadingText = strHeadingText;
            this.Message = strMessage;
            this.Mode = strMode;            
        }

        internal void Show()
        {
            this.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowConfirmation", "ShowConfirmationBox();", true);
        }

        internal void Hide()
        {
            this.Visible = false;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "CloseConfirmation", "CloseConfirmationBox();", true);
        }
    }
}