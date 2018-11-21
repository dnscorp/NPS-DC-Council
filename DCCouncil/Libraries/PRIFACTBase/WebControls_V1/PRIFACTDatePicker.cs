using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Globalization;


namespace PRIFACT.PRIFACTBase.WebControls_V1
{
    [ToolboxData("<{0}:PRIFACTDatePicker runat=server></{0}:PRIFACTDatePicker>")]
    [ValidationProperty("Text")]
    [Designer("System.Web.UI.Design.WebControls.ListControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class PRIFACTDatePicker : TextBox, IPostBackDataHandler
    {
        public enum CalendarDateFormat
        {
            MMDDYYYY,
            DDMMYYYY
        }

        public ApproximateDate ApproximateDate
        {
            get
            {
                ApproximateDate appDate = null;
                string strSep = "/";
                if (Text.IndexOf(".") != -1)
                {
                    strSep = ".";
                }
                else if (Text.IndexOf("-") != -1)
                {
                    strSep = "-";
                }

                string[] strArray = Array.FindAll(Text.Split(strSep.ToCharArray()), i => i.Length > 0);

                int? iMonth = null;
                int? iDay = null;
                int? iYear = null;
                if (strArray.Length == 3)
                {
                    int iMonthOut = 0;
                    bool bMonth = Int32.TryParse(strArray[0], out iMonthOut);
                    if (bMonth && iMonthOut > 0 && iMonthOut <= 12)
                    {
                        iMonth = iMonthOut;
                    }

                    int iYearOut = 0;
                    bool bYear = Int32.TryParse(strArray[2], out iYearOut);
                    if (bYear && iYearOut >= 1900 && iYearOut <= 9999)
                    {
                        iYear = iYearOut;
                    }

                    int iDayOut = 0;
                    bool bDay = Int32.TryParse(strArray[1], out iDayOut);
                    if (bDay && iDayOut > 0 && iDayOut <= 31)
                    {
                        iDay = iDayOut;
                    }

                    if (iMonth.HasValue && iDay.HasValue && iYear.HasValue)
                    {
                        appDate = new ApproximateDate(iYear.Value, iMonth.Value, iDay.Value);
                    }
                }
                else if (strArray.Length == 2)
                {
                    int iMonthOut = 0;
                    bool bMonth = Int32.TryParse(strArray[0], out iMonthOut);
                    if (bMonth && iMonthOut > 0 && iMonthOut <= 12)
                    {
                        iMonth = iMonthOut;
                    }

                    int iYearOut = 0;
                    bool bYear = Int32.TryParse(strArray[1], out iYearOut);
                    if (bYear && iYearOut >= 1900 && iYearOut <= 9999)
                    {
                        iYear = iYearOut;
                    }

                    if (iMonth.HasValue && iYear.HasValue)
                    {
                        appDate = new ApproximateDate(iYear.Value, iMonth.Value);
                    }
                }
                else if (strArray.Length == 1)
                {
                    int iYearOut = 0;
                    bool bYear = Int32.TryParse(strArray[0], out iYearOut);
                    if (bYear && iYearOut >= 1900 && iYearOut <= 9999)
                    {
                        iYear = iYearOut;
                    }

                    if (iYear.HasValue)
                    {
                        appDate = new ApproximateDate(iYear.Value);
                    }
                }

                if (appDate == null && Date.HasValue)
                {
                    appDate = new ApproximateDate(Date.Value.Year, Date.Value.Month, Date.Value.Day);
                }

                return appDate;
            }
        }

        private string CBOuterDivID
        {
            get
            {
                return String.Format("{0}_CBOuterDivID", this.ClientID);
            }
        }

        private string CBDateContainerID
        {
            get
            {
                return String.Format("{0}_CBDateContainerID", this.ClientID);
            }
        }

        private string CBImageID
        {
            get
            {
                return String.Format("{0}_CBImageID", this.ClientID);
            }
        }

        public string CalObjectID
        {
            get
            {
                return String.Format("{0}_CalObjectID", this.ClientID);
            }
        }

        public string CBTextBoxID
        {
            get
            {
                return String.Format("{0}_CBTextBoxID", this.ClientID);
            }
        }

        public CalendarDateFormat DateFormat
        {
            get
            {
                object obj = ViewState["CBCalendarDateFormat"];
                CalendarDateFormat dtFormat = (obj == null) ? (CalendarDateFormat)Enum.Parse(typeof(CalendarDateFormat), System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern) : (CalendarDateFormat)Enum.Parse(typeof(CalendarDateFormat), obj.ToString());

                return dtFormat;
            }
            set
            {
                ViewState["CBCalendarDateFormat"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public override string Text
        {
            get
            {
                object obj = ViewState["CBSelectedValue"];
                string strText = (obj == null) ? string.Empty : (string)obj;
                if (strText.Length == 0)
                {
                    return string.Empty;
                }

                return strText;
            }
            set
            {
                ViewState["CBSelectedValue"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public string CBDivScrollContainer
        {
            get
            {
                object obj = ViewState["CBDivScrollContainer"];
                string strText = (obj == null) ? string.Empty : (string)obj;
                if (strText.Length == 0)
                {
                    return string.Empty;
                }
                return strText;
            }
            set
            {
                ViewState["CBDivScrollContainer"] = value;
            }
        }


        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public string TipText
        {
            get
            {
                object obj = ViewState["CBTipText"];
                string strText = (obj == null) ? string.Empty : (string)obj;
                if (strText.Length == 0)
                {
                    return string.Empty;
                }
                return strText;
            }
            set
            {
                ViewState["CBTipText"] = value;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [Themeable(false)]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool CBAppendToBody
        {
            get
            {
                object obj = ViewState["CBAppendToBody"];
                return (obj == null) ? true : (bool)obj;
            }
            set
            {
                ViewState["CBAppendToBody"] = value;
            }
        }


        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public string TipTextClass
        {
            get
            {
                object obj = ViewState["CBTipTextClass"];
                string strText = (obj == null) ? string.Empty : (string)obj;
                if (strText.Length == 0)
                {
                    return string.Empty;
                }
                return strText;
            }
            set
            {
                ViewState["CBTipTextClass"] = value;
            }
        }



        [Bindable(true)]
        [Browsable(true)]
        [Themeable(false)]
        public DateTime? Date
        {
            get
            {
                object obj = ViewState["DBSelectedDate"];
                if (obj == null)
                {
                    return null;
                }
                try
                {
                    return Convert.ToDateTime(obj);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["DBSelectedDate"] = value;
                // || Text.Length == 0
                if (value.HasValue)
                {
                    string strMonth = value.Value.Month.ToString();
                    if (value.Value.Month < 10)
                    {
                        strMonth = "0" + strMonth;
                    }

                    string strDay = value.Value.Day.ToString();
                    if (value.Value.Day < 10)
                    {
                        strDay = "0" + strDay;
                    }
                    if (DateFormat == CalendarDateFormat.DDMMYYYY)
                    {
                        Text = strDay + "/" + strMonth + "/" + value.Value.Year;
                    }
                    else
                    {
                        Text = strMonth + "/" + strDay + "/" + value.Value.Year;
                    }
                }
                else
                {
                    Text = string.Empty;
                    //if (Text.Length > 0)
                    //{
                    //    try
                    //    {
                    //        ViewState["DBSelectedDate"] = Convert.ToDateTime(Text);
                    //    }
                    //    catch
                    //    {
                    //    }
                    //}
                }
            }
        }

        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public DateTime? DefaultDate
        {
            get
            {
                object obj = ViewState["DefaultDate"];
                if (obj == null)
                {
                    return null;
                }
                try
                {
                    return Convert.ToDateTime(obj);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["DefaultDate"] = value;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [Themeable(false)]
        public DateTime? MinDate
        {
            get
            {
                object obj = ViewState["MinDate"];
                if (obj == null)
                {
                    return null;
                }
                try
                {
                    return Convert.ToDateTime(obj);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["MinDate"] = value;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [Themeable(false)]
        public DateTime? MaxDate
        {
            get
            {
                object obj = ViewState["MaxDate"];
                if (obj == null)
                {
                    return null;
                }
                try
                {
                    return Convert.ToDateTime(obj);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["MaxDate"] = value;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [Themeable(false)]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool CBReadOnly
        {
            get
            {
                object obj = ViewState["CBReadOnly"];
                return (obj == null) ? false : (bool)obj;
            }
            set
            {
                ViewState["CBReadOnly"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public string OnChangeFunction
        {
            get
            {
                object obj = ViewState["OnChangeFunction"];
                string strText = (obj == null) ? string.Empty : (string)obj;
                if (strText.Length == 0)
                {
                    return string.Empty;
                }
                return strText;
            }
            set
            {
                ViewState["OnChangeFunction"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public string CBTextBoxCssClass
        {
            get
            {
                object obj = ViewState["CBTextBoxCssClass"];
                return (obj == null) ? string.Empty : (string)obj;
            }
            set
            {
                ViewState["CBTextBoxCssClass"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public string CBOuterDivCssClass
        {
            get
            {
                object obj = ViewState["CBOuterDivCssClass"];
                return (obj == null) ? "Custom_CalendarOuterDiv" : (string)obj;
            }
            set
            {
                ViewState["CBOuterDivCssClass"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(true)]
        [Themeable(true)]
        [DefaultValue("")]
        [Category("Style")]
        public string CBPenultimateDivCssClass
        {
            get
            {
                object obj = ViewState["CBPenultimateDivCssClass"];
                return (obj == null) ? "Custom_DropDownPenultimateDiv" : (string)obj;
            }
            set
            {
                ViewState["CBPenultimateDivCssClass"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(true)]
        [Themeable(true)]
        [DefaultValue("")]
        [Category("Style")]
        public int CBOuterWidth
        {
            get
            {
                object obj = ViewState["CBOuterWidth"];
                return (obj == null) ? 100 : (int)obj;
            }
            set
            {
                ViewState["CBOuterWidth"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(true)]
        [Themeable(true)]
        [DefaultValue("")]
        [Category("Style")]
        public int CBTextBoxWidth
        {
            get
            {
                object obj = ViewState["CBTextBoxWidth"];
                return (obj == null) ? 90 : (int)obj;
            }
            set
            {
                ViewState["CBTextBoxWidth"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(true)]
        [Themeable(true)]
        [DefaultValue("")]
        [Category("Style")]
        public int CBSuggestionBoxHeight
        {
            get
            {
                object obj = ViewState["CBSuggestionBoxHeight"];
                return (obj == null) ? 100 : (int)obj;
            }
            set
            {
                ViewState["CBSuggestionBoxHeight"] = value;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [Themeable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [UrlProperty]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        public string CBImageUrl
        {
            get
            {
                object obj = ViewState["CBImageUrl"];
                return (obj == null) ? Page.ClientScript.GetWebResourceUrl(this.GetType(), "PRIFACTBase.WebControls_V1.images.btn_calendar.gif") : obj.ToString();
            }
            set
            {
                ViewState["CBImageUrl"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(true)]
        [Themeable(true)]
        [DefaultValue("")]
        [Category("Style")]
        public string CBImageCssClass
        {
            get
            {
                object obj = ViewState["CBImageCssClass"];
                return (obj == null) ? "Custom_DropDownImage" : (string)obj;
            }
            set
            {
                ViewState["CBImageCssClass"] = value;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [Themeable(false)]
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool IsDisplayContainerStatic
        {
            get
            {
                object obj = ViewState["CBIsDisplayContainerStatic"];
                return (obj == null) ? false : (bool)obj;
            }
            set
            {
                ViewState["CBIsDisplayContainerStatic"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public string YesLink
        {
            get
            {
                object obj = ViewState["YesLink"];
                string strText = (obj == null) ? string.Empty : (string)obj;
                if (strText.Length == 0)
                {
                    return string.Empty;
                }
                return strText;
            }
            set
            {
                ViewState["YesLink"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public string NoLink
        {
            get
            {
                object obj = ViewState["NoLink"];
                string strText = (obj == null) ? string.Empty : (string)obj;
                if (strText.Length == 0)
                {
                    return string.Empty;
                }
                return strText;
            }
            set
            {
                ViewState["NoLink"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public string YesSelClassName
        {
            get
            {
                object obj = ViewState["YesSelClassName"];
                string strText = (obj == null) ? string.Empty : (string)obj;
                if (strText.Length == 0)
                {
                    return string.Empty;
                }
                return strText;
            }
            set
            {
                ViewState["YesSelClassName"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public string NoSelClassName
        {
            get
            {
                object obj = ViewState["NoSelClassName"];
                string strText = (obj == null) ? string.Empty : (string)obj;
                if (strText.Length == 0)
                {
                    return string.Empty;
                }
                return strText;
            }
            set
            {
                ViewState["NoSelClassName"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public string NoClassName
        {
            get
            {
                object obj = ViewState["NoClassName"];
                string strText = (obj == null) ? string.Empty : (string)obj;
                if (strText.Length == 0)
                {
                    return string.Empty;
                }
                return strText;
            }
            set
            {
                ViewState["NoClassName"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public string YesClassName
        {
            get
            {
                object obj = ViewState["YesClassName"];
                string strText = (obj == null) ? string.Empty : (string)obj;
                if (strText.Length == 0)
                {
                    return string.Empty;
                }
                return strText;
            }
            set
            {
                ViewState["YesClassName"] = value;
            }
        }

        [Bindable(false)]
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue("")]
        public string HidYesNoId
        {
            get
            {
                object obj = ViewState["HidYesNoId"];
                string strText = (obj == null) ? string.Empty : (string)obj;
                if (strText.Length == 0)
                {
                    return string.Empty;
                }
                return strText;
            }
            set
            {
                ViewState["HidYesNoId"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "PRIFACTWebControlsHelper", System.Web.HttpUtility.HtmlEncode(Page.ClientScript.GetWebResourceUrl(this.GetType(), "PRIFACTBase.WebControls_V1.JScripts.PRIFACTWebControlsHelper.js")));

            // create the style sheet control and put it in the document header
            string csslink = "<link rel='stylesheet' type='text/css' href='" + System.Web.HttpUtility.HtmlEncode(Page.ClientScript.GetWebResourceUrl(this.GetType(), "PRIFACTBase.WebControls_V1.StyleSheet.css")) + "' />";
            LiteralControl include = new LiteralControl(csslink);
            include.ID = "EmbeddedCss";
            bool bFound = false;
            foreach (Control ctrl in this.Page.Header.Controls)
            {
                if (ctrl.ID == include.ID)
                {
                    bFound = true;
                }
            }

            if (!bFound)
            {
                this.Page.Header.Controls.Add(include);
            }
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //safari image position fix
            string strContainer = this.CBAppendToBody ? "null" : string.Format("'{0}'", this.CBDateContainerID);
            string strMinDate = MinDate.HasValue ? "new Date(" + MinDate.Value.Year.ToString() + "," + (MinDate.Value.Month - 1) + "," + MinDate.Value.Day.ToString() + ")" : "null";
            string strMaxDate = MaxDate.HasValue ? "new Date(" + MaxDate.Value.Year.ToString() + "," + (MaxDate.Value.Month - 1) + "," + MaxDate.Value.Day.ToString() + ")" : "null";
            string strDefaultDate = DefaultDate.HasValue ? "new Date(" + DefaultDate.Value.Year.ToString() + "," + (DefaultDate.Value.Month - 1) + "," + DefaultDate.Value.Day.ToString() + ")" : "null";
            string strOnChangeFunction = string.IsNullOrEmpty(this.OnChangeFunction) ? "null" : this.OnChangeFunction;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "JS_DatePicker" + this.ClientID, "var " + this.CalObjectID
                + " = new Calendar('" + DateFormat.ToString() + "'," + IsDisplayContainerStatic.ToString().ToLower() + ",'" + this.CalObjectID + "','" + this.Text
                + "', '" + this.CBTextBoxID + "','" + this.CBImageID + "'," + strContainer + ",'"
                + this.CBDivScrollContainer + "'," + strDefaultDate + "," + strMinDate + "," + strMaxDate + ",'" + this.YesLink + "','" + this.NoLink + "','" + this.YesSelClassName + "','" + this.NoSelClassName + "','" + this.YesClassName + "','" + this.NoClassName + "','" + this.HidYesNoId + "',\"" + strOnChangeFunction + "\");", true);

            //register that LoadPostdata will be called even if control's UniqueID is not on post data collection
            Page.RegisterRequiresPostBack(this);

            if (Page.IsPostBack)
            {
                //this.Text = HttpContext.Current.Request[this.CBTextBoxID] == null ? string.Empty : HttpContext.Current.Request[this.CBTextBoxID];
            }
        }

        /// <summary>
        /// Load data from POST (load checked from hidden input)
        /// </summary>
        /// <param name="postDataKey">Key</param>
        /// <param name="postCollection">Data</param>
        /// <returns>True when value changed</returns>
        public bool LoadPostData(string postDataKey,
            System.Collections.Specialized.NameValueCollection postCollection)
        {
            string postedValue = postCollection[postDataKey];

            this.Text = HttpContext.Current.Request[this.CBTextBoxID] == null ? string.Empty : HttpContext.Current.Request[this.CBTextBoxID];
            string str = Text;
            if (!String.IsNullOrEmpty(Text))
            {
                try
                {
                    if (DateFormat == CalendarDateFormat.DDMMYYYY)
                    {
                        string[] strDt = str.Split("/".ToCharArray());
                        if (strDt.Length == 3)
                        {
                            this.Date = new DateTime(Convert.ToInt32(strDt[2]), Convert.ToInt32(strDt[1]), Convert.ToInt32(strDt[0]));
                        }
                        else
                        {
                            this.Date = null;
                            Text = str;
                        }
                    }
                    else
                    {
                        this.Date = Convert.ToDateTime(str);
                    }
                    Text = str;
                }
                catch
                {
                    this.Date = null;
                    Text = str;
                }
            }
            else
            {
                this.Date = null;
            }
            return true;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                EnsureChildControls();
            }


            if (this.TipText != null && this.TipText.Length > 0)
            {
                string showTipJS = "Tip('" + this.TipText + "', BALLOON, true, ABOVE, true, OFFSETX, -17, PADDING, 8);";
                string hideTipJS = "UnTip();";
                writer.AddAttribute("onmouseover", showTipJS);
                writer.AddAttribute("onmouseout", hideTipJS);
            }


            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "event.cancelBubble = true;");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.CBOuterDivID);
            //writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.CBOuterWidth.ToString() + "px");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CBOuterDivCssClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Span);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CBPenultimateDivCssClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Span);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.CBTextBoxID);
            if (!Enabled)
            {
                CBReadOnly = true;
            }
            if (CBReadOnly)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "readonly");
            }

            if (Enabled)
            {
                writer.AddAttribute("onkeyup", this.CalObjectID + ".IsValidDate();");
                writer.AddAttribute("onkeydown", this.CalObjectID + ".ProcessKeyDown(event);");
                writer.AddAttribute("onfocus", this.CalObjectID + ".IsValidDate();");
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.CBTextBoxID);
            //writer.AddAttribute(HtmlTextWriterAttribute.Id, this.CBTextBoxID);
            writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, this.TabIndex.ToString());
            if (Width.IsEmpty)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "70px");
            }
            else
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, string.Format("{0}px", Width.Value));
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, this.Text);
            if (!Enabled)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return false;");
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "ShowCalendar(event,'" + this.CalObjectID + "');");
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            if (!Enabled)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return false;");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "cursor:default;");
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "ShowCalendar(event,'" + this.CalObjectID + "');");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "cursor:pointer;");
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.CBImageID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CBImageCssClass);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, this.CBImageUrl);
            writer.AddAttribute(HtmlTextWriterAttribute.Alt, string.Empty);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();

            writer.RenderEndTag();

            writer.RenderEndTag();
        }
    }

    public class ApproximateDate
    {
        // Fields
        private int? _day;
        private int? _month;
        private int _year;

        // Methods
        public ApproximateDate()
        {
            this._year = DateTime.Now.Year;
        }

        public ApproximateDate(int year)
        {
            this._year = DateTime.Now.Year;
            this.Year = year;
        }

        public ApproximateDate(int year, int month)
            : this(year)
        {
            this.Month = new int?(month);
        }

        public ApproximateDate(int year, int month, int day)
            : this(year, month)
        {
            this.Day = new int?(day);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(15);
            if (this.Month.HasValue)
            {
                builder.AppendFormat("{0:D2}-", this.Month);
                if (this.Day.HasValue)
                {
                    builder.AppendFormat("{0:D2}-", this.Day);
                }
            }
            builder.AppendFormat("{0:D4}", this.Year);
            return builder.ToString();
        }

        // Properties
        public int? Day
        {
            get
            {
                return this._day;
            }
            set
            {
                int? nullable = value;
                if (((nullable.GetValueOrDefault() < 1) && nullable.HasValue) || (((nullable = value).GetValueOrDefault() > 0x1f) && nullable.HasValue))
                {
                    throw new ArgumentOutOfRangeException("value", "The day must be between 1 and 31.");
                }
                this._day = value;
            }
        }

        public int? Month
        {
            get
            {
                return this._month;
            }
            set
            {
                int? nullable = value;
                if (((nullable.GetValueOrDefault() < 1) && nullable.HasValue) || (((nullable = value).GetValueOrDefault() > 12) && nullable.HasValue))
                {
                    throw new ArgumentOutOfRangeException("value", "The month must be between 1 and 12.");
                }
                this._month = value;
            }
        }

        public int Year
        {
            get
            {
                return this._year;
            }
            set
            {
                if ((value < 0x3e8) || (value > 0x270f))
                {
                    throw new ArgumentOutOfRangeException("value", "The year must be between 1000 and 9999.");
                }
                this._year = value;
            }
        }
    }
}
