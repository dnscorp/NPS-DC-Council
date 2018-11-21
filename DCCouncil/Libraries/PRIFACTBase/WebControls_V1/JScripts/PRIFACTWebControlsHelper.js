var lowerCaseLengthArray = new Array(7, 8, 7, 8, 7, 4, 7, 7, 3, 4, 8, 3, 11, 7, 8, 8, 8, 5, 6, 4, 7, 7, 11, 7, 7, 6);
var upperCaseLengthArray = new Array(11, 10, 11, 11, 9, 9, 11, 11, 5, 6, 12, 9, 14, 12, 12, 9, 12, 10, 9, 9, 11, 11, 15, 11, 11, 9);
var lowerCaseArray = new Array('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z');
var upperCaseArray = new Array('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z');

function GetCharLength(chr) {

    if (isLowerCase(chr)) {
        for (var i = 0; i < lowerCaseArray.length; i++) {
            if (chr == lowerCaseArray[i]) {
                return lowerCaseLengthArray[i];
            }
        }
    }
    else {
        for (var i = 0; i < upperCaseArray.length; i++) {
            if (chr == upperCaseArray[i]) {
                return upperCaseLengthArray[i];
            }
        }
    }

    //for space
    if (chr.replace(" ", "").length == 0)
        return 4

    //for number    
    return 8;
}

var SiteMaxZIndex = 20090;
var SuggestionsDivClassArray = null;

function AddToSuggestionsDivArray(id) {
    if (SuggestionsDivClassArray == null) {
        SuggestionsDivClassArray = new Array();
    }
    var bFound = false;
    for (var i = 0; i < SuggestionsDivClassArray.length; i++) {
        if (SuggestionsDivClassArray[i] == id) {
            bFound = true;
            break;
        }
    }

    if (!bFound) {
        SuggestionsDivClassArray[SuggestionsDivClassArray.length] = id;
    }
}

function HideAllSuggestionsDiv(id) {
    if (SuggestionsDivClassArray == null)
        return;

    for (var i = 0; i < SuggestionsDivClassArray.length; i++) {
        if (document.getElementById(SuggestionsDivClassArray[i]) != null && SuggestionsDivClassArray[i] != id && document.getElementById(SuggestionsDivClassArray[i]).style.display == "block") {
            document.getElementById(SuggestionsDivClassArray[i]).style.display = "none";
        }
    }
}
/**
* An autosuggest textbox control.
* @class
* @scope public
*/
function AutoSuggestControl(IsDisplayContainerStatic, containerid, outerscrolldivid, DivSuggestContainerID, isEnabled, oTextbox /*:HTMLInputElement*/, txtBoxClass, suggestionsOuterCss, txtBoxOnChangeFunctionName, strOnChangeFunctionParams, isReadOnly, oContainerDiv, oPenultimateDiv, oImage, SuggestionBoxHeight, HiddenValueField,
                            oProvider /*:SuggestionProvider*/, strFunctionName, IsSourceOneDimensional, IsFunctionAjax, MinimumPrefixLength, FilterResultsOnClient) {

    if (oTextbox == null)
        return;
    //debugger;

    //debugger;
    /**
    * The currently selected suggestions.
    * @scope private
    */
    this.cur /*:int*/ = -1;

    this.IsDisplayContainerStatic = IsDisplayContainerStatic;

    this.RealContainerID = DivSuggestContainerID;
    this.DivScrollContainer = outerscrolldivid;
    this.ConstOffsetLeft = -1;
    this.ConstOffsetTop = 17;

    this.isEnabled = isEnabled;

    this.DivSuggestContainerID = DivSuggestContainerID;

    /**
    * The dropdown list layer.
    * @scope private
    */
    this.layer = null;

    /**
    * Suggestion provider for the autosuggest feature.
    * @scope private.
    */
    this.provider /*:SuggestionProvider*/ = oProvider;

    this.HiddenValueField = HiddenValueField;

    this.ReadOnly = isReadOnly;

    /**
    * The textbox to capture.
    * @scope private
    */
    this.textbox /*:HTMLInputElement*/ = oTextbox;

    this.txtBoxOnChangeFunctionName = txtBoxOnChangeFunctionName;
    this.OnChangeFunctionParams = strOnChangeFunctionParams;

    this.textbox.className = txtBoxClass;

    this.CBSuggestionsOuterDivCssClass = suggestionsOuterCss;

    this.DivContainer = oContainerDiv;

    this.PenultimateDiv = oPenultimateDiv;

    this.img = oImage;

    /*safari fix*/

//    if (this.img != null && navigator.appVersion.indexOf("Safari") != -1) {
//        this.img.style.margin = "6px 0px 0px 0px";
//    }
    /*safari fix*/

    this.SuggestionBoxHeight = SuggestionBoxHeight;

    this.FunctionName = strFunctionName;

    this.IsFunctionAjax = IsFunctionAjax;

    this.MinimumPrefixLength = MinimumPrefixLength;

    this.IsSourceOneDimensional = IsSourceOneDimensional;

    this.FilterResultsOnClient = FilterResultsOnClient;

    this.MaxItemsToShowAtATime = 5;

    this.Timer = null;

    //initialize the control
    this.init();
}

/**
* Autosuggests one or more suggestions for what the user has typed.
* If no suggestions are passed in, then no autosuggest occurs.
* @scope private
* @param aSuggestions An array of suggestion strings.
* @param bTypeAhead If the control should provide a type ahead suggestion.
*/
AutoSuggestControl.prototype.autosuggest = function(aSuggestions /*:Array*/,
                                                     bTypeAhead /*:boolean*/, bShowOnUI) {
    //added by arun onblur june 6 2009, top fix the issue with relatives where only one suggestion present did', select the node upon clicking the down arrow
    this.cur = -1;
    //make sure there's at least one suggestion
    if (aSuggestions.length > 0) {
        if (bTypeAhead) {
            this.typeAhead(aSuggestions[0]);
        }
        this.showSuggestions(aSuggestions, bShowOnUI);
    } else {
        this.hideSuggestions();
    }
};

/**
* Creates the dropdown layer to display multiple suggestions.
* @scope private
*/
AutoSuggestControl.prototype.createDropDown = function() {
    if (document.getElementById(this.RealContainerID) != null) {
        this.layer = document.getElementById(this.RealContainerID);
        return;
    }
    var oThis = this;
    //create the layer and assign styles
    this.layer = document.createElement("span");
    this.layer.className = this.CBSuggestionsOuterDivCssClass;
    this.layer.style.overflowX = "hidden";
    this.layer.style.overflowY = "auto";
    //this.layer.style.height = this.SuggestionBoxHeight + "px";
    this.layer.style.position = "absolute";
    this.layer.style.whiteSpace = "nowrap";
    this.layer.id = this.RealContainerID; //this.DivContainer.id + "_cblayer";
    //this.layer.style.padding = "3px";

    //this.layer.style.width = this.textbox.offsetWidth +20 + 'px';

    if (this.FunctionName != null) {
        //this.layer.style.width = this.textbox.offsetWidth + 100 + 'px';
    }

    //when the user clicks on the a suggestion, get the text (innerHTML)
    //and place it into a textbox
    this.layer.onmousedown =
    this.layer.onmouseup =
    this.layer.onmouseover = function(oEvent) {
        oEvent = oEvent || window.event;
        oTarget = oEvent.target || oEvent.srcElement;

        if (oEvent.type == "mousedown") {
            //oThis.textbox.value = oTarget.firstChild.nodeValue;
            //oThis.hideSuggestions();
        } else if (oEvent.type == "mouseover") {
            oThis.highlightSuggestion(oTarget);
        }
        else {
            oThis.textbox.focus();
        }

    };

    //this.layer.style.width = this.layer.offsetWidth + "px";
    //this.GetLayerContainer().appendChild(this.layer);
    this.RegisterLayer();
};

AutoSuggestControl.prototype.RegisterLayer = function() {

    if (this.layer.attachEvent) {
        this.layer.onclick = new Function("event.cancelBubble = true;");
    }
    else {
        this.layer.setAttribute("onclick", "event.cancelBubble = true;");
    }
    document.body.appendChild(this.layer);

    AddToContainerArray(this.DivScrollContainer, this.layer.id, this.textbox.id, this.ConstOffsetLeft, this.ConstOffsetTop, this.IsDisplayContainerStatic);
    AddToSuggestionsDivArray(this.layer.id);
};

/**
* Gets the left coordinate of the textbox.
* @scope private
* @return The left coordinate of the textbox in pixels.
*/
AutoSuggestControl.prototype.getLeft = function() /*:int*/{

    //    var oNode = this.textbox;
    //    var iLeft = 0;
    //    
    //    while(oNode.tagName != "BODY") {
    //        iLeft += oNode.offsetLeft;
    //        oNode = oNode.offsetParent;        
    //    }
    //    if(navigator.appName.indexOf("Microsoft") != -1){
    //    return iLeft;
    //    }
    //    else{
    //    return iLeft;
    //    }

    var obj = this.DivContainer;
    var curleft = 0;
    if (obj.offsetParent) {
        while (obj.offsetParent) {
            curleft += obj.offsetLeft
            obj = obj.offsetParent;
        }
    }
    else if (obj.x)
        curleft += obj.x;

    if (navigator.appName == "Microsoft Internet Explorer") {
        curleft += 1;
    }
    return curleft;
};

/**
* Gets the top coordinate of the textbox.
* @scope private
* @return The top coordinate of the textbox in pixels.
*/
AutoSuggestControl.prototype.getTop = function() /*:int*/{
    var obj = this.DivContainer;
    var curtop = 0;
    if (obj.offsetParent) {
        while (obj.offsetParent) {
            curtop += obj.offsetTop
            obj = obj.offsetParent;
        }
    }
    else if (obj.y)
        curtop += obj.y;

    if (navigator.appName == "Microsoft Internet Explorer") {
        curtop += 1;
    }
    return curtop;
    //    var oNode = this.textbox;
    //    var iTop = 0;
    //    
    //    while(oNode.tagName != "BODY") {
    //        iTop += oNode.offsetTop;
    //        oNode = oNode.offsetParent;
    //    }
    //    if(navigator.appName.indexOf("Microsoft") != -1){
    //    return iTop + 5;
    //    }
    //    else{
    //    return iTop + 5;
    //    }
};

AutoSuggestControl.prototype.stopEvent = function(event /*:Event*/) {
    event = event || window.event;

    if (event) {
        if (event.stopPropagation) event.stopPropagation();
        if (event.preventDefault) event.preventDefault();

        if (typeof event.cancelBubble != "undefined") {
            event.cancelBubble = true;
            event.returnValue = false;
        }
    }

    return false;
};

/**
* Handles three keydown events.
* @scope private
* @param oEvent The event object for the keydown event.
*/
AutoSuggestControl.prototype.handleKeyDown = function(oEvent /*:Event*/) {
    //debugger;
    if (this.layer == null) {
        this.createDropDown();
        //this.cur = -1;
        if (this.provider != null) {
            this.provider.allSuggestions(this, false, false);
        }
    }
    //alert('hello');
    HideAllSuggestionsDiv(this.layer.id);

//    if (this.textbox.value == "Select") {
//        this.textbox.value = '';
//    }

    //debugger;
    var iKeyCodeVal = oEvent.keyCode;
    //alert(this.cur);
    if (oEvent.altKey && iKeyCodeVal == 40) {
        if (this.layer == null || this.layer.style.display == "none") {
            HidePRIFACTMenus();
            this.textbox.focus();

            this.provider.allSuggestions(this, false, true);
        }
        else {
            this.hideSuggestions();
        }
    }
    else if (iKeyCodeVal == 38) {
        if (this.cur == -1)
            return;
        this.previousSuggestion();
        var cSuggestionNodes = this.layer.childNodes;
        if (cSuggestionNodes.length > 0 && this.cur < cSuggestionNodes.length) {
            var oNode = cSuggestionNodes[this.cur];
            oTarget = oEvent.target || oEvent.srcElement;
            if (this.HiddenValueField != null) {
                this.HiddenValueField.value = oNode.getAttribute("ItemID");
            }
        }

        this.stopEvent(oEvent);
    }
    else if (iKeyCodeVal == 40) {
        this.nextSuggestion();
        var cSuggestionNodes = this.layer.childNodes;
        if (cSuggestionNodes.length > 0 && this.cur < cSuggestionNodes.length) {
            var oNode = cSuggestionNodes[this.cur];
            oTarget = oEvent.target || oEvent.srcElement;
            if (this.HiddenValueField != null) {
                this.HiddenValueField.value = oNode.getAttribute("ItemID");
            }
        }

        this.stopEvent(oEvent);
    }
    else if (iKeyCodeVal == 13) {
        var cSuggestionNodes = this.layer.childNodes;
        if (cSuggestionNodes.length > 0 && this.cur < cSuggestionNodes.length) {
            var oNode = cSuggestionNodes[this.cur];
            oTarget = oEvent.target || oEvent.srcElement;
            if (this.HiddenValueField != null) {
                this.HiddenValueField.value = oNode.getAttribute("ItemID");
            }
        }

        if (this.txtBoxOnChangeFunctionName.length > 0) {
            if (this.OnChangeFunctionParams.length > 0) {
                eval(this.txtBoxOnChangeFunctionName)(this.OnChangeFunctionParams, oNode.getAttribute("ItemID"));
            }
            else {
                eval(this.txtBoxOnChangeFunctionName)(this.textbox);
            }
        }

        this.hideSuggestions();
    }
    else if (iKeyCodeVal == 9) {
        this.hideSuggestions();
    }
    else {
        if (this.FunctionName == null) {
            this.ScrollIntoSuggestion(oEvent);
        }
    }
    //    switch(oEvent.keyCode) {
    //        case 38: //up arrow
    //            this.previousSuggestion();
    //            break;
    //        case 40: //down arrow 
    //            this.nextSuggestion();
    //            break;
    //        case 13: //enter
    //            this.hideSuggestions();
    //            break;
    //    }
};

AutoSuggestControl.prototype.handleImageClick = function(oEvent /*:Event*/) {
    //debugger;
    if (this.layer == null || this.layer.style.display == "none") {
        HidePRIFACTMenus();
        this.textbox.focus();
        //this.nextSuggestion();

        this.provider.allSuggestions(this, false, true);
    }
    else {
        this.hideSuggestions();
    }
};

/**
* Handles keyup events.
* @scope private
* @param oEvent The event object for the keyup event.
*/
AutoSuggestControl.prototype.handleKeyUp = function(oEvent /*:Event*/) {
    //debugger;
    //HidePRIFACTMenus();
    var oThis = this;
    curObj = oThis;
    var iKeyCode = oEvent.keyCode;

    //for backspace (8) and delete (46), shows suggestions without typeahead
    if (iKeyCode == 8 || iKeyCode == 46) {
        if (oThis.FunctionName != null) {
            var sTextboxValue = oThis.textbox.value;
            if (sTextboxValue.length < oThis.MinimumPrefixLength) {
                oThis.hideSuggestions();
                return;
            }
            if (oThis.IsFunctionAjax) {
                this.cur = -1;
                eval(oThis.FunctionName)(sTextboxValue, oThis.onSuccess, oThis.onError);
            }
            else {
                var strTempArr = eval(oThis.FunctionName)(sTextboxValue);
                oThis.provider = new StateSuggestions(strTempArr);
                oThis.provider.requestSuggestions(oThis, false);
            }
        }
        else {
            this.provider.requestSuggestions(this, false);
        }

        //make sure not to interfere with non-character keys
    } else if (iKeyCode < 32 || (iKeyCode >= 33 && iKeyCode < 46) || (iKeyCode >= 112 && iKeyCode <= 123)) {
        //ignore
    } else {
        //request suggestions from the suggestion provider with typeahead
        //arun
        if (oThis.FunctionName != null) {
            var sTextboxValue = oThis.textbox.value;
            if (sTextboxValue.length < oThis.MinimumPrefixLength) {
                oThis.hideSuggestions();
                return;
            }
            if (oThis.IsFunctionAjax) {
                eval(oThis.FunctionName)(sTextboxValue, oThis.onSuccess, oThis.onError);
            }
            else {
                var strTempArr = eval(oThis.FunctionName)(sTextboxValue);
                oThis.provider = new StateSuggestions(strTempArr);
                oThis.provider.requestSuggestions(oThis, false);
            }
        }
        else {
            this.provider.requestSuggestions(this, false);
        }
    }
};
var curObj = null;
AutoSuggestControl.prototype.onSuccess = function(result, userContext, methodName) {
    oThis = curObj;

    if (methodName == 'HelloWorld') {
        //Update a section of the UI   
    }

    if (userContext == 'abc') {
        //Do a specific action if it for New York   
    }
    oThis.provider = new StateSuggestions(result);
    oThis.provider.requestSuggestions(oThis, false);
};

AutoSuggestControl.prototype.onError = function(exception, userContext, methodName) {
    /*  
    We can also perform different actions like the  
    succeededCallback handler based upon the methodName and userContext  
    */

};

/**
* Hides the suggestion dropdown.
* @scope private
*/
AutoSuggestControl.prototype.hideSuggestions = function() {
    if (this.layer != null) {
        this.layer.style.display = "none";
    }
    if (document.getElementById(this.DivSuggestContainerID) == null) {
        //this.layer.parentNode.style.position = "static";
    }
    this.cur /*:int*/ = -1;
};

/**
* Highlights the given node in the suggestions dropdown.
* @scope private
* @param oSuggestionNode The node representing a suggestion in the dropdown.
*/
AutoSuggestControl.prototype.highlightSuggestion = function(oSuggestionNode) {
    var oThis = this;

    for (var i = 0; i < this.layer.childNodes.length; i++) {
        var oNode = this.layer.childNodes[i];
        if (oNode == oSuggestionNode) {
            //oNode.className = "current"
            oNode.style.color = "#FFFFFF";
            oNode.style.backgroundColor = "#08246B"; //"#3366cc";
            this.cur = i;
            oNode.onmousedown = function(oEvent) {
                oEvent = oEvent || window.event;
                oTarget = oEvent.target || oEvent.srcElement;
                if (oEvent.type == "mousedown") {
                    //alert(oTarget.firstChild.nodeValue);
                    //alert(oTarget.firstChild.parentNode.getAttribute("ItemID"));
                    if (oThis.HiddenValueField != null) {
                        oThis.HiddenValueField.value = oTarget.firstChild.parentNode.getAttribute("ItemID");
                        document.getElementById(oThis.HiddenValueField.id).value = oTarget.firstChild.parentNode.getAttribute("ItemID");
                    }
                    //alert(oThis.textbox.value);
                    document.getElementById(oThis.textbox.id).value = oTarget.firstChild.nodeValue;
                    //oThis.textbox.value = oTarget.firstChild.nodeValue;
                    if (oThis.txtBoxOnChangeFunctionName.length > 0) {
                        if (oThis.OnChangeFunctionParams.length > 0) {
                            eval(oThis.txtBoxOnChangeFunctionName)(oThis.OnChangeFunctionParams, oTarget.firstChild.parentNode.getAttribute("ItemID"));
                        }
                        else {
                            eval(oThis.txtBoxOnChangeFunctionName)(oThis.textbox);
                        }
                    }
                    oThis.hideSuggestions();
                    //alert(oTarget.firstChild.parentNode.getAttribute("ItemID"));
                    //alert(oThis.textbox.value);
                }
            }
        } else {

            oNode.style.color = "#000000";
            oNode.style.backgroundColor = "#FFFFFF";
        }
    }
};

/**
* Initializes the textbox with event handlers for
* auto suggest functionality.
* @scope private
*/
AutoSuggestControl.prototype.init = function() {

    if (!this.isEnabled)
        return;
    //save a reference to this object
    var oThis = this;

    //debugger;
    //alert(this.textbox.getAttribute("readonly"));

    if (this.textbox.getAttribute("readonly") == false || this.textbox.getAttribute("readonly") == null) {
        //assign the onkeyup event handler
        this.textbox.onkeyup = function(oEvent) {
            //check for the proper location of the event object
            if (!oEvent) {
                oEvent = window.event;
            }

            //call the handleKeyUp() method with the event object
            oThis.handleKeyUp(oEvent);
        };
    }

    //assign onkeydown event handler
    this.textbox.onkeydown = function(oEvent) {

        //check for the proper location of the event object
        if (!oEvent) {
            oEvent = window.event;
        }

        //call the handleKeyDown() method with the event object
        oThis.handleKeyDown(oEvent);

        var bIsLayerVisible = false;
        if (oThis.layer != null && oThis.layer.style != null && oThis.layer.style.display == "block") {
            bIsLayerVisible = true;
        }

        if (oEvent.keyCode == 13) {
            return false;
        }
    };

    if (this.textbox.getAttribute("readonly") != null && (this.textbox.getAttribute("readonly") == "readonly" || this.textbox.getAttribute("readonly") == true)) {
        this.textbox.style.cursor = "pointer";
    }

    if (!this.IsFunctionAjax) {
        this.textbox.onclick = function(oEvent) {

            //check for the proper location of the event object
            if (!oEvent) {
                oEvent = window.event;
            }
            //call the handleKeyDown() method with the event object
            oThis.handleImageClick(oEvent);
        };
    }

    //assign onkeydown event handler
    if (this.img != null) {
        this.img.onclick = function(oEvent) {

            //check for the proper location of the event object
            if (!oEvent) {
                oEvent = window.event;
            }
            //call the handleKeyDown() method with the event object
            oThis.handleImageClick(oEvent);
        };
    }

    //assign onblur event handler (hides suggestions)    
    this.textbox.onblur = function() {
        //oThis.hideSuggestions();
    };

    //create the suggestions dropdown
    //this.createDropDown();
};

AutoSuggestControl.prototype.ScrollIntoSuggestion = function(oEvent) {
    //alert('hello');
    if (this.layer == null) {
        this.createDropDown();
    }

    var cSuggestionNodes = this.layer.childNodes;
    var typedChar = String.fromCharCode(oEvent.keyCode);

    var currentNode = null;
    if (this.cur != -1) {
        currentNode = cSuggestionNodes[this.cur];
    }
    var currentNodeVal = currentNode != null ? currentNode.firstChild.nodeValue : "";
    var bNodeFound = false;

    if (this.cur + 1 < cSuggestionNodes.length) {
        for (var i = this.cur + 1; i < cSuggestionNodes.length; i++) {
            var oCorrectNode = cSuggestionNodes[i];
            if (oCorrectNode.firstChild.nodeValue.charAt(0).toLowerCase() == typedChar.toLowerCase()) {
                bNodeFound = true;
                break;
            }
        }
    }

    if (!bNodeFound) {
        for (var i = 0; i < cSuggestionNodes.length; i++) {
            var oCorrectNode = cSuggestionNodes[i];
            if (oCorrectNode.firstChild.nodeValue.charAt(0).toLowerCase() == typedChar.toLowerCase()) {
                bNodeFound = true;
                break;
            }
        }
    }

    if (bNodeFound && oCorrectNode.firstChild.nodeValue.charAt(0).toLowerCase() == typedChar.toLowerCase()) {
        //oNode.scrollIntoView(true);
        this.layer.scrollTop = oCorrectNode.offsetTop;
        this.highlightSuggestion(oCorrectNode);

        if (this.ReadOnly) {
            this.textbox.value = oCorrectNode.firstChild.nodeValue;
            if (this.FunctionName == null && this.HiddenValueField != null) {
                this.HiddenValueField.value = oCorrectNode.getAttribute("ItemID");
            }
            
            if (this.txtBoxOnChangeFunctionName.length > 0) {
                if (this.OnChangeFunctionParams.length > 0) {
                    eval(this.txtBoxOnChangeFunctionName)(this.OnChangeFunctionParams, oCorrectNode.firstChild.nodeValue);
                }
                else {
                    eval(this.txtBoxOnChangeFunctionName)(this.textbox);
                }
            }
        }

        this.cur = i;
    }
};

AutoSuggestControl.prototype.ScrollIntoTextNode = function() {
    var cSuggestionNodes = this.layer.childNodes;
    //alert(cSuggestionNodes.length);
    for (var i = 0; i < cSuggestionNodes.length; i++) {
        var oNode = cSuggestionNodes[i];
        if (oNode.firstChild.nodeValue.toLowerCase() == this.textbox.value.toLowerCase()) {
            this.layer.scrollTop = oNode.offsetTop;
            //alert(this.layer.scrollTop);
            this.highlightSuggestion(oNode);
            this.cur = i;
            //alert(this.textbox.value);
        }
    }
    clearTimeout(this.Timer);
    this.Timer = null;
}

/**
* Highlights the next suggestion in the dropdown and
* places the suggestion into the textbox.
* @scope private
*/
AutoSuggestControl.prototype.nextSuggestion = function() {
    var cSuggestionNodes = this.layer.childNodes;
    //alert(this.cur);
    //alert(cSuggestionNodes.length);
    if (cSuggestionNodes.length > 0 && this.cur < cSuggestionNodes.length - 1) {
        var oNode = cSuggestionNodes[++this.cur];
        //oNode.scrollIntoView(true);
        this.highlightSuggestion(oNode);
        //alert(oNode.offsetTop + " : " + oNode.offsetHeight + " : " + this.layer.scrollTop + " : " + this.layer.offsetHeight);
        if (navigator.appVersion.indexOf("MSIE") != -1) {
            if ((oNode.offsetTop + oNode.offsetHeight) > (this.layer.offsetHeight)) {
                var ndx = this.cur - this.MaxItemsToShowAtATime + 1;
                if (ndx > 0)
                    this.layer.scrollTop = this.layer.scrollTop + cSuggestionNodes[ndx].offsetTop;
            }
        }
        else {
            if ((oNode.offsetTop + oNode.offsetHeight) > (this.layer.scrollTop + this.layer.offsetHeight)) {
                var ndx = this.cur - this.MaxItemsToShowAtATime + 1;
                if (ndx > 0)
                    this.layer.scrollTop = cSuggestionNodes[ndx].offsetTop;
            }
        }
        this.textbox.value = oNode.firstChild.nodeValue;
        if (this.FunctionName == null && this.HiddenValueField != null) {
            this.HiddenValueField.value = oNode.getAttribute("ItemID");
        }
        this.textbox.focus();
    }
};

/**
* Highlights the previous suggestion in the dropdown and
* places the suggestion into the textbox.
* @scope private
*/
AutoSuggestControl.prototype.previousSuggestion = function() {
    var cSuggestionNodes = this.layer.childNodes;

    if (cSuggestionNodes.length > 0 && this.cur > 0) {
        var oNode = cSuggestionNodes[--this.cur];
        if (oNode != null) {
            //oNode.scrollIntoView(true);
            if (navigator.appVersion.indexOf("MSIE") != -1) {
                if (oNode.offsetTop < this.layer.scrollTop)
                    this.layer.scrollTop = this.layer.scrollTop + oNode.offsetTop;
            }
            else {
                if (oNode.offsetTop < this.layer.scrollTop)
                    this.layer.scrollTop = oNode.offsetTop;
            }
            this.highlightSuggestion(oNode);
            this.textbox.value = oNode.firstChild.nodeValue;
            if (this.FunctionName == null && this.HiddenValueField != null) {
                this.HiddenValueField.value = oNode.getAttribute("ItemID");
            }
        }
    }
};

/**
* Selects a range of text in the textbox.
* @scope public
* @param iStart The start index (base 0) of the selection.
* @param iLength The number of characters to select.
*/
AutoSuggestControl.prototype.selectRange = function(iStart /*:int*/, iLength /*:int*/) {

    //use text ranges for Internet Explorer
    if (this.textbox.createTextRange) {
        var oRange = this.textbox.createTextRange();
        oRange.moveStart("character", iStart);
        oRange.moveEnd("character", iLength - this.textbox.value.length);
        oRange.select();

        //use setSelectionRange() for Mozilla
    } else if (this.textbox.setSelectionRange) {
        this.textbox.setSelectionRange(iStart, iLength);
    }

    //set focus back to the textbox
    this.textbox.focus();
};

/**
* Builds the suggestion layer contents, moves it into position,
* and displays the layer.
* @scope private
* @param aSuggestions An array of suggestions for the control.
*/
AutoSuggestControl.prototype.showSuggestions = function(aSuggestions /*:Array*/, bShowOnUI) {

    //debugger;
    HideContainerArrayItems();
    var oDiv = null;
    //alert(this.provider.states.length);
    if (this.layer == null) {
        this.createDropDown();
    }
    this.layer.innerHTML = "";  //clear contents of the layer
    if (document.getElementById(this.DivSuggestContainerID) == null) {
        //this.layer.parentNode.style.position = "relative";
    }

    for (var i = 0; i < aSuggestions.length; i++) {
        oDiv = document.createElement("span");
        oDiv.style.cursor = "pointer";
        oDiv.id = "div_inner_" + i;
        oDiv.style.padding = "0px 3px";
        oDiv.style.color = "black";
        oDiv.style.fontWeight = "normal";
        oDiv.style.backgroundColor = "white";
        if (this.IsSourceOneDimensional) {
            oDiv.setAttribute("ItemID", aSuggestions[i]);
            oDiv.appendChild(document.createTextNode(aSuggestions[i]));
        }
        else {
            oDiv.setAttribute("ItemID", aSuggestions[i][1]);
            oDiv.appendChild(document.createTextNode(aSuggestions[i][0]));
        }

        this.layer.appendChild(oDiv);
    }


    //this.layer.style.left = "13px";
    var lMaxWidth = 0;
    var strMaxString = "";
    for (var i = 0; i < aSuggestions.length; i++) {
        if (this.IsSourceOneDimensional) {
            if (aSuggestions[i].length > lMaxWidth) {
                lMaxWidth = aSuggestions[i].length;
                strMaxString = aSuggestions[i];
            }
        }
        else {
            if (aSuggestions[i][0].length > lMaxWidth) {
                lMaxWidth = aSuggestions[i][0].length;
                strMaxString = aSuggestions[i][0];
            }
        }
    }

    var divWidth = 0;
    for (var i = 0; i < strMaxString.length; i++) {
        //debugger;
        divWidth += GetCharLength(strMaxString.charAt(i));
        //        if (isLowerCase(strMaxString.charAt(i))) {
        //            divWidth += 7;
        //        }
        //        else {
        //            divWidth += 11;
        //        }
    }


    //this.layer.style.top = this.DivContainer.offsetHeight + "px";

    var layerWidth = divWidth + 20;
    //alert(this.textbox.offsetWidth);
    if (layerWidth < this.DivContainer.offsetWidth) {
        layerWidth = this.DivContainer.offsetWidth - 2;
    }

    this.layer.style.width = layerWidth + "px";


    if (document.getElementById(this.DivSuggestContainerID) != null) {
        //this.layer.style.left = this.getLeft() + "px";
        //this.layer.style.top = (this.getTop() + this.DivContainer.offsetHeight) + "px";
    }
    if (SiteMaxZIndex) {
        this.layer.style.zIndex = SiteMaxZIndex;
        SiteMaxZIndex += 10;
    }
    else {
        this.layer.style.zIndex = 7000;
    }

    //this.layer.style.width = this.textbox.offsetWidth +20 + 'px';
    //this.layer.style.width = this.layer.style.width;
    //this.layer.style.position = "absolute";
    //this.layer.style.zIndex = 30000;
    SetLeftTopOfDiv(this.DivScrollContainer, this.layer.id, this.textbox.id, this.ConstOffsetLeft, this.ConstOffsetTop, this.IsDisplayContainerStatic);
    HideAllSuggestionsDiv(this.layer.id);


    this.layer.style.display = "block";

    var real_height = 0;
    for (var i = 0; i < this.layer.childNodes.length; i++) {
        var oNode = this.layer.childNodes[i];
        real_height += oNode.offsetHeight;
    }

    if (aSuggestions.length > this.MaxItemsToShowAtATime) {
        real_height = 0;
        for (var i = 0; i < this.MaxItemsToShowAtATime; i++) {
            var oNode = this.layer.childNodes[i];
            real_height += oNode.offsetHeight;
        }
        this.layer.style.height = String(real_height) + "px";
    }
    else {
        this.layer.style.height = String(real_height) + "px";
    }

    if (!this.IsFunctionAjax) {
        var oThis = this;
        this.Timer = setTimeout(function() { oThis.ScrollIntoTextNode(); }, 25);

        if (!bShowOnUI) {
            this.layer.style.display = "none";
            this.ScrollIntoTextNode();
        }
    }
    var oThis = this;
    setTimeout(function() { SetLeftTopOfDiv(oThis.DivScrollContainer, oThis.layer.id, oThis.textbox.id, oThis.ConstOffsetLeft, oThis.ConstOffsetTop, oThis.IsDisplayContainerStatic); }, 25);
    //alert(arun_height);
    //alert(this.layer.style.left + " : " + this.layer.style.top);
};

/**
* Inserts a suggestion into the textbox, highlighting the 
* suggested part of the text.
* @scope private
* @param sSuggestion The suggestion for the textbox.
*/
AutoSuggestControl.prototype.typeAhead = function(sSuggestion /*:String*/) {

    //check for support of typeahead functionality
    if (this.textbox.createTextRange || this.textbox.setSelectionRange) {
        var iLen = this.textbox.value.length;
        this.textbox.value = sSuggestion[1];
        this.selectRange(iLen, sSuggestion.length);
    }
};

/*********************************************************************************/

/**
* Provides suggestions for state names (USA).
* @class
* @scope public
*/
function StateSuggestions(optionArray) {
    this.states = optionArray;
}

/**
* Request suggestions for the given autosuggest control. 
* @scope protected
* @param oAutoSuggestControl The autosuggest control to provide suggestions for.
*/
StateSuggestions.prototype.requestSuggestions = function(oAutoSuggestControl /*:AutoSuggestControl*/,
                                                          bTypeAhead /*:boolean*/) {
    var aSuggestions = [];
    var sTextboxValue = oAutoSuggestControl.textbox.value;

    if (sTextboxValue.length > 0) {
        //search for matching states
        for (var i = 0; i < this.states.length; i++) {
            if (!oAutoSuggestControl.FilterResultsOnClient) {
                aSuggestions.push(this.states[i]);
            }
            else {
                if (oAutoSuggestControl.IsSourceOneDimensional) {
                    if (this.states[i].toLowerCase().indexOf(sTextboxValue.toLowerCase()) == 0) {
                        aSuggestions.push(this.states[i]);
                    }
                }
                else {
                    if (this.states[i][0].toLowerCase().indexOf(sTextboxValue.toLowerCase()) == 0) {
                        aSuggestions.push(this.states[i]);
                    }
                }
            }
        }
    }

    //provide suggestions to the control
    oAutoSuggestControl.autosuggest(aSuggestions, bTypeAhead, true);
};

StateSuggestions.prototype.allSuggestions = function(oAutoSuggestControl /*:AutoSuggestControl*/,
                                                          bTypeAhead /*:boolean*/, bShowOnUI) {
    var aSuggestions = [];
    for (var i = 0; i < this.states.length; i++)
        aSuggestions.push(this.states[i]);

    //provide suggestions to the control
    oAutoSuggestControl.autosuggest(aSuggestions, bTypeAhead, bShowOnUI);
};
/*********************************************************************************/
/*************************************START DATE PICKER********************************************/
function FloatingDivClass(OuterDivID, DivContainerID, ElementID, ConstOffsetLeft, ConstOffsetTop, IsDisplayContainerStatic) {
    this.DivContainerID = DivContainerID;
    this.ElementID = ElementID;
    this.ConstOffsetLeft = ConstOffsetLeft;
    this.ConstOffsetTop = ConstOffsetTop;
    this.OuterDivID = OuterDivID;
    this.IsDisplayContainerStatic = IsDisplayContainerStatic;
}
/// <reference name="MicrosoftAjax.js"/>

/* This notice must be untouched at all times.
Copyright (c) 2009 Arun Mohan. All rights reserved.

DatePicker.js	 v. 1.1

Created May 14, 2009 by Arun Mohan
Last modified: May 16, 2009*/

function findPosX(obj) {
    var curleft = 0;
    if (obj.offsetParent)
        while (1) {
        curleft += obj.offsetLeft;
        if (!obj.offsetParent)
            break;
        obj = obj.offsetParent;
    }
    else if (obj.x)
        curleft += obj.x;
    return curleft;
}

function findPosY(obj) {
    var curtop = 0;
    if (obj.offsetParent)
        while (1) {
        curtop += obj.offsetTop;
        if (!obj.offsetParent)
            break;
        obj = obj.offsetParent;
    }
    else if (obj.y)
        curtop += obj.y;
    return curtop;
}

var weekday = new Array("S", "M", "T", "W", "T", "F", "S");

var monthArray = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");

var Mode_Dates = "Dates";
var Mode_Months = "Months";
var Mode_Years = "Years";

var ContainerArray = null;

function AddToContainerArray(OuterDivID, DivContainerID, ElementID, ConstOffsetLeft, ConstOffsetTop, IsDisplayContainerStatic) {
    if (ContainerArray == null) {
        ContainerArray = new Array();
    }
    var bFound = false;
    for (var i = 0; i < ContainerArray.length; i++) {
        if (ContainerArray[i].DivContainerID == DivContainerID) {
            bFound = true;
            break;
        }
    }

    if (!bFound) {
        ContainerArray[ContainerArray.length] = new FloatingDivClass(OuterDivID, DivContainerID, ElementID, ConstOffsetLeft, ConstOffsetTop, IsDisplayContainerStatic);
    }
}

function HideContainerArrayItems() {
    if (ContainerArray == null)
        return;

    for (var i = 0; i < ContainerArray.length; i++) {
        //alert(document.getElementById(ContainerArray[i].DivContainerID).style.display);
        if (document.getElementById(ContainerArray[i].DivContainerID) != null && document.getElementById(ContainerArray[i].DivContainerID).style.display == "block") {
            document.getElementById(ContainerArray[i].DivContainerID).style.display = "none";
        }
    }
}

function SetLeftTopOfDivAll() {
    if (ContainerArray == null)
        return;

    for (var i = 0; i < ContainerArray.length; i++) {
        if (document.getElementById(ContainerArray[i].DivContainerID) != null && document.getElementById(ContainerArray[i].DivContainerID).style.display == "block") {
            SetLeftTopOfDiv(ContainerArray[i].OuterDivID, ContainerArray[i].DivContainerID, ContainerArray[i].ElementID, ContainerArray[i].ConstOffsetLeft, ContainerArray[i].ConstOffsetTop, ContainerArray[i].IsDisplayContainerStatic);
        }
    }
}

//window.onresize = HandleResize;
//window.onscroll = HandleScroll;

if (window.attachEvent) {
    window.attachEvent('onresize', HandleResize);
    window.attachEvent('onscroll', HandleScroll);
}
else {
    window.addEventListener('resize', HandleResize, false);
    window.addEventListener('scroll', HandleScroll, false);
}


function HandleScroll() {
    //AdjustLeftTopOfAllPopupDiv();
    setTimeout("SetLeftTopOfDivAll();", 120);
    //HideContainerArrayItems();
}

function HandleResize() {
    setTimeout("SetLeftTopOfDivAll();", 120);
    //HideContainerArrayItems();
}
function getBodyScrollTop() {
    if (document.documentElement.scrollTop != null && document.documentElement.scrollTop != 0)
        return document.documentElement.scrollTop;
    else if (window.pageYOffset != null && window.pageYOffset != 0)
        return window.pageYOffset;
    else if (document.body.scrollTop != null && document.body.scrollTop != 0)
        return document.body.scrollTop;
    else
        return 0;
}

function SetLeftTopOfDiv(OuterDivID, DivContainerID, ElementID, ConstOffsetLeft, ConstOffsetTop, IsDisplayContainerStatic) {
    var scTop = 0;
    var scBodyTop = 0;
    if (document.getElementById(OuterDivID) != null) {
        scTop = document.getElementById(OuterDivID).scrollTop
    }
    //mileesh //
    //!!Emergency fix . Need permanent fix//
    if (IsDisplayContainerStatic) {
        scBodyTop = getBodyScrollTop();
    }

    //debugger;
    //alert('hello');
    var strLeft = String(findPosX(document.getElementById(ElementID)) + ConstOffsetLeft) + "px";
    var strTop = String(findPosY(document.getElementById(ElementID)) - scTop + scBodyTop + ConstOffsetTop) + "px";
    //alert(strLeft + " : " + strTop);
    document.getElementById(DivContainerID).style.left = strLeft;
    document.getElementById(DivContainerID).style.top = strTop;

    //alert(document.getElementById(DivContainerID).style.left + " : " + document.getElementById(DivContainerID).style.top);
}


function Calendar(DateFormat, IsDisplayContainerStatic, calobjectid, saveddate, txtboxid, caliconid, containerid, outerdivid, defaultdate, mindate, maxdate, yeslink, nolink, yesselclassname, noselclassname, yesclassname, noclassname, hidyesnoid, onchangefunction) {
    this.DateFormat = DateFormat;
    this.CurrentDate = null;
    this.TextBoxDate = null;
    //alert(calobjectid);
    this.DefaultDate = defaultdate;
    this.DatesToDisplay = null;
    this.MonthsToDisplay = null;
    this.DatesTwoDimensional = null;
    this.MonthsToDisplay = null;
    this.YearsToDisplay = null;
    this.Mode = Mode_Dates;
    this.Rows = 5;
    this.Columns = 7;
    this.ID = calobjectid;
    this.YearRangeLastValue = null;
    if (containerid == null) {
        this.ContainerID = "DivCalendar_" + calobjectid;
    }
    else {
        this.ContainerID = containerid;
    }
    this.TextBoxID = txtboxid;
    this.CalendarIconID = caliconid;
    this.Fade = false;
    this.FadeDelay = 500;
    this.FadeStart = 30;
    this.FadeEnd = 100;
    /*CSS*/
    this.DivHeaderClassName = "cal-header";
    this.PreviousLinkClassName = "prev";
    this.NextLinkClassName = "next";
    this.DivFooterClassName = "cal-footer";
    this.CloseLinkClassName = "close right";
    this.DivOuterClassName = "cal-ctrl";
    this.DivAClassName = "cal-top-shadow";
    this.DivBClassName = "cal-content-border";
    this.DivBInnerClassName = "cal-content";
    this.DivContentClassName = "cal-table"
    this.DivCClassName = "cal-bottom-shadow";
    this.DivDatesClassName = "day";
    this.DivMonthsClassName = "month";
    this.DivYearsClassName = "year";
    this.DivWeekNamesClassName = "cal-week-head";
    this.DivClearClassName = "clear-l";
    this.DivDateNotInThisMonthClassName = "notthismonth";
    this.DivThisMonthClassName = "thismonth";
    this.DivThisYearClassName = "thisyear";
    this.DivDateTodayClassName = "today";
    this.DivDateDisabledClassName = "disabled";
    this.DivDateDisabledNotThisMonthClassName = "disabled-notthismonth"
    this.OuterDivID = outerdivid;
    this.ConstOffsetLeft = -1;
    this.ConstOffsetTop = 17;
    this.IsDisplayContainerStatic = IsDisplayContainerStatic;
    if (mindate == null) {
        this.MinDate = new Date(1900, 0, 1);
    }
    else {
        this.MinDate = mindate;
    }
    if (maxdate == null) {
        this.MaxDate = new Date(2100, 11, 31);
    }
    else {
        this.MaxDate = maxdate;
    }

    this.YesLink = yeslink;
    this.NoLink = nolink;
    this.YesSelClassName = yesselclassname;
    this.NoSelClassName = noselclassname;
    this.YesClassName = yesclassname;
    this.NoClassName = noclassname;
    this.HidYesNoId = hidyesnoid;
    this.SavedDate = saveddate.length > 0 ? saveddate : null;
    this.OnChangeFunction = onchangefunction;
    /*CSS*/
    //this.Initialize();
}

Calendar.prototype.IsValidDate = function() {
    var dt = this.GetTextBoxDate();

    if (dt != null) {
        if (this.YesLink.length > 0 && this.NoLink.length > 0) {
            OnYesNoClicked(null, true, true, this.YesLink, this.NoLink, this.YesSelClassName, this.NoSelClassName, this.YesClassName, this.NoClassName, this.HidYesNoId, this.TextBoxID, eval(this.ID));
        }

        this.ShowCalendar();
    }
};

Calendar.prototype.DisableCalendar = function() {
    var txtBox = document.getElementById(this.TextBoxID);
    var calIcon = document.getElementById(this.CalendarIconID);

    if (txtBox.attachEvent) {
        txtBox.onclick = new Function("return false;");
    }
    else {
        txtBox.setAttribute("onclick", "return false;");
    }

    if (calIcon.attachEvent) {
        calIcon.onclick = new Function("return false;");
    }
    else {
        calIcon.setAttribute("onclick", "return false;");
    }

    calIcon.style.cursor = "default";
};

Calendar.prototype.EnableCalendar = function() {
    var txtBox = document.getElementById(this.TextBoxID);
    var calIcon = document.getElementById(this.CalendarIconID);

    if (txtBox.attachEvent) {
        txtBox.onclick = new Function("ShowCalendar(event," + this.ID + ");");
    }
    else {
        txtBox.setAttribute("onclick", "ShowCalendar(event," + this.ID + ");");
    }

    if (calIcon.attachEvent) {
        calIcon.onclick = new Function("ShowCalendar(event," + this.ID + ");");
    }
    else {
        calIcon.setAttribute("onclick", "ShowCalendar(event," + this.ID + ");");
    }

    calIcon.style.cursor = "pointer";
};

Calendar.prototype.GetCalendarContainer = function() {
    if (document.getElementById(this.ContainerID) == null) {
        var DivContainer = document.createElement("div");
        if (DivContainer.attachEvent) {
            DivContainer.onclick = new Function("StopEvent(event);");
        }
        else {
            DivContainer.setAttribute("onclick", "StopEvent(event);");
        }
        DivContainer.id = this.ContainerID;
        DivContainer.style.width = "163px";
        document.body.appendChild(DivContainer);
    }
    else {
        var DivContainer = document.getElementById(this.ContainerID);
        if (DivContainer.attachEvent) {
            DivContainer.onclick = new Function("StopEvent(event);");
        }
        else {
            DivContainer.setAttribute("onclick", "StopEvent(event);");
        }
    }

    AddToContainerArray(this.OuterDivID, this.ContainerID, this.TextBoxID, this.ConstOffsetLeft, this.ConstOffsetTop, this.IsDisplayContainerStatic);

    return DivContainer;
};

Calendar.prototype.Initialize = function() {
    if (this.Mode == Mode_Dates) {
        this.Rows = 5;
        this.Columns = 7;
        this.DatesToDisplay = this.GetDatesToDisplay(this.CurrentDate.getMonth() + 1, this.CurrentDate.getFullYear());
        if (this.DatesToDisplay.length > 35) {
            this.Rows = 6;
        }

        this.DatesTwoDimensional = new Array(this.Rows * this.Columns);
        var iCell = 0;
        for (iRow = 0; iRow < this.Rows; iRow++) {
            for (iColumn = 0; iColumn < this.Columns; iColumn++) {
                this.DatesTwoDimensional[iCell] = new Array(2);
                this.DatesTwoDimensional[iRow][iColumn] = this.DatesToDisplay[iCell];
                iCell++;
            }
        }
    }
    else if (this.Mode == Mode_Months) {
        this.MonthsToDisplay = this.GetMonthsToDisplay();
        this.Rows = 4;
        this.Columns = 3;
        var iCell = 0;
        for (iRow = 0; iRow < this.Rows; iRow++) {
            for (iColumn = 0; iColumn < this.Columns; iColumn++) {
                this.DatesTwoDimensional[iCell] = new Array(2);
                this.DatesTwoDimensional[iRow][iColumn] = this.MonthsToDisplay[iCell];
                iCell++;
            }
        }
    }
    else if (this.Mode == Mode_Years) {
        this.YearsToDisplay = this.GetYearsToDisplay();
        this.Rows = 4;
        this.Columns = 3;
        var iCell = 0;
        for (iRow = 0; iRow < this.Rows; iRow++) {
            for (iColumn = 0; iColumn < this.Columns; iColumn++) {
                this.DatesTwoDimensional[iCell] = new Array(2);
                this.DatesTwoDimensional[iRow][iColumn] = this.YearsToDisplay[iCell];
                iCell++;
            }
        }
    }
};

Calendar.prototype.RemoveEmptyArrayElements = function(strArray) {
    var retArray = new Array();
    for (var i = 0; i < strArray.length; i++) {
        if (strArray[i].length > 0) {
            retArray[retArray.length] = strArray[i];
        }
    }

    return retArray;
};

Calendar.prototype.GetTextBoxDate = function() {
    var dt = null;
    var txtVal = document.getElementById(this.TextBoxID).value;

    if (txtVal.length == 0 && this.SavedDate != null && this.SavedDate.length > 0) {
        txtVal = this.SavedDate;
    }

    var bDateFound = false;
    var strSeperator = '/';
    if (txtVal.indexOf(".") != -1) {
        strSeperator = '.';
    }
    else if (txtVal.indexOf("-") != -1) {
        strSeperator = '-';
    }

    if (txtVal.length > 0) {
        if (txtVal.indexOf(strSeperator) != -1) {
            var strValArray = this.RemoveEmptyArrayElements(txtVal.split(strSeperator));
            //alert(strValArray.length);
            if (strValArray.length == 3) {

                var iMonth = strValArray[0] * 1;
                var iDay = strValArray[1] * 1;
                
                if (this.DateFormat == "DDMMYYYY") {
                    iMonth = strValArray[1] * 1;
                    iDay = strValArray[0] * 1;
                }
                var iYear = strValArray[2] * 1;

                var iDaysInAMonth = this.DaysInAMonth(iMonth, iYear);

                if (String(iYear).length == 4 && String(iMonth).length <= 2 && iMonth > 0 && iMonth <= 12 && this.MinDate != null && iYear >= 1900 && iYear <= 9999 && iDay <= iDaysInAMonth) {
                    dt = new Date(iYear, iMonth - 1, iDay);
                }
            }
            else if (strValArray.length == 2) {
                var iMonth = strValArray[0] * 1;
                var iYear = strValArray[1] * 1;

                if (String(iYear).length == 4 && String(iMonth).length <= 2 && iMonth > 0 && iMonth <= 12 && iYear >= 1900 && iYear <= 9999) {
                    dt = new Date(iYear, iMonth - 1, 1);
                }
            }
        }
        else {
            if (txtVal.length == 4) {
                dt = new Date(txtVal, 0, 1);
            }
        }
    }

    //document.getElementById("mydt").value = dt;
    if (dt == null || dt == "Invalid Date" || dt == "NaN") {
        bDateFound = false;
    }
    else {
        bDateFound = true;
    }

    if (bDateFound) {
        return dt;
    }
    else {
        return null;
    }
};

Calendar.prototype.ShowCalendar = function() {

    var dt = this.GetTextBoxDate();
    //alert(dt);
    if (dt == null) {
        if (this.DefaultDate != null) {
            dt = this.DefaultDate;
            dt = this.DefaultDate;
        }
        else {
            dt = new Date();
            dt = new Date();
        }
    }
    else {

    }

    this.CurrentDate = dt;
    this.TextBoxDate = dt;
    //alert(this.DefaultDate);
    //alert(dt);
    //alert(this.CurrentDate);
    this.Mode = Mode_Dates;
    this.Initialize();

    this.RenderCalendar();

    this.GetCalendarContainer().style.position = "absolute";
    this.GetCalendarContainer().style.zIndex = 30000;
    SetLeftTopOfDiv(this.OuterDivID, this.ContainerID, this.TextBoxID, this.ConstOffsetLeft, this.ConstOffsetTop, this.IsDisplayContainerStatic);
    this.GetCalendarContainer().style.display = "block";
};


Calendar.prototype.HideCalendar = function() {
    var txtVal = document.getElementById(this.TextBoxID).value;
    this.GetCalendarContainer().style.display = "none";
};

Calendar.prototype.RenderCalendar = function() {
    /*
    <div class="cal-ctrl">
    <div class="cal-top-shadow">
    </div>
    <div class="cal-content-border">
    <div class="cal-content">
    </div>
    </div>
    <div class="cal-bottom-shadow">
    </div>
    </div>
    */

    var DivOuter = document.createElement("div");
    DivOuter.className = this.DivOuterClassName;

    var DivA = document.createElement("div");
    DivA.className = this.DivAClassName;
    DivOuter.appendChild(DivA);

    var DivB = document.createElement("div");
    DivB.className = this.DivBClassName;

    var DivBInner = document.createElement("div");
    DivBInner.className = this.DivBInnerClassName;

    var DivContent = document.createElement("div");
    DivContent.className = this.DivContentClassName;

    DivBInner.appendChild(this.CreateHeader());
    if (this.Mode == Mode_Dates) {
        DivContent.appendChild(this.RenderDates());
    }
    else if (this.Mode == Mode_Months) {
        DivContent.appendChild(this.RenderMonths());
    }
    else if (this.Mode == Mode_Years) {
        DivContent.appendChild(this.RenderYears());
    }

    DivBInner.appendChild(DivContent);

    DivBInner.appendChild(this.CreateFooter());

    DivB.appendChild(DivBInner);
    DivOuter.appendChild(DivB);

    var DivC = document.createElement("div");
    DivC.className = this.DivCClassName;
    DivOuter.appendChild(DivC);

    this.GetCalendarContainer().innerHTML = "";
    this.GetCalendarContainer().appendChild(DivOuter);

    if (this.Fade) {
        Fade(this.ContainerID, this.FadeStart, this.FadeEnd, this.FadeDelay);
    }

    //    if (this.GetCalendarContainer().attachEvent) {
    //        this.GetCalendarContainer().onblur = new Function("alert('blur');eval(" + this.ID + ").HideCalendar();");
    //    }
    //    else {
    //        this.GetCalendarContainer().setAttribute("onblur", "alert('blur');eval(" + this.ID + ").HideCalendar();");
    //    }
};

Calendar.prototype.ProcessKeyDown = function(event) {
    if (event.keyCode == 9) {
        this.HideCalendar();
        return false;
    }

    return true;
};

Calendar.prototype.SetMode = function(mode) {
    this.YearRangeLastValue = null;
    this.Mode = mode;
    this.Initialize();
    this.RenderCalendar();
};

Calendar.prototype.ShowNext = function() {
    if (this.Mode == Mode_Dates) {
        this.CurrentDate.setDate(1);
        this.CurrentDate.setMonth(this.CurrentDate.getMonth() + 1);
    }
    else if (this.Mode == Mode_Months) {
        this.CurrentDate.setDate(1);
        this.CurrentDate.setYear(this.CurrentDate.getFullYear() + 1);
    }
    else if (this.Mode == Mode_Years) {
    }

    this.Initialize();
    this.RenderCalendar();
};

Calendar.prototype.ShowPrevious = function() {
    if (this.Mode == Mode_Dates) {
        this.CurrentDate.setDate(1);
        this.CurrentDate.setMonth(this.CurrentDate.getMonth() - 1);
    }
    else if (this.Mode == Mode_Months) {
        this.CurrentDate.setDate(1);
        this.CurrentDate.setYear(this.CurrentDate.getFullYear() - 1);
    }
    else if (this.Mode == Mode_Years) {
        this.YearRangeLastValue -= 20;
    }
    this.Initialize();
    this.RenderCalendar();
};

Calendar.prototype.ShowThisMonth = function() {
    this.CurrentDate = new Date();
    this.Mode = Mode_Dates;
    this.YearRangeLastValue = null;
    //this.CurrentDate.setMonth(this.CurrentDate.getMonth() - 1);
    //alert(this.CurrentDate);
    this.Initialize();
    this.RenderCalendar();
};

Calendar.prototype.OnMonthClicked = function(month) {
    this.CurrentDate.setMonth(month);
    this.Mode = Mode_Dates;
    this.Initialize();
    this.RenderCalendar();
};

Calendar.prototype.OnYearClicked = function(year) {
    this.CurrentDate.setYear(year);
    this.Mode = Mode_Months;
    this.Initialize();
    this.RenderCalendar();
};

Calendar.prototype.OnSetDate = function(date) {
    //debugger;
    if (document.getElementById(this.TextBoxID) != null) {
        var dt = new Date(date);
        strDay = (dt.getDate()) < 10 ? '0' + String(dt.getDate()) : String(dt.getDate());
        var obj = eval(this.ID);
       
        strMonth = (dt.getMonth() + 1) < 10 ? '0' + String(dt.getMonth() + 1) : String(dt.getMonth() + 1);
        if (this.DateFormat == "DDMMYYYY") {
            document.getElementById(this.TextBoxID).value = strDay + "/" + strMonth + "/" + dt.getFullYear();
        }
        else {
            document.getElementById(this.TextBoxID).value = strMonth + "/" + strDay + "/" + dt.getFullYear();
        }
        this.GetCalendarContainer().style.display = "none";

        if (this.OnChangeFunction != null) {
            eval(this.OnChangeFunction);
        }
        if (this.YesLink.length > 0 && this.NoLink.length > 0) {
            OnYesNoClicked(null, true, true, this.YesLink, this.NoLink, this.YesSelClassName, this.NoSelClassName, this.YesClassName, this.NoClassName, this.HidYesNoId, this.TextBoxID, eval(this.ID));
        }
    }
}

Calendar.prototype.CreateHeader = function() {
    /*
    <div class="cal-header">
    <a href="#" class="prev" title="previous">Prev</a><a href="#" class="next" title="Next">
    Next</a> <a href="#">December, 2009</a>
    </div>
    */

    var DivHeader = document.createElement("div");
    DivHeader.className = this.DivHeaderClassName;

    var PrevLink = document.createElement("a");
    PrevLink.className = this.PreviousLinkClassName;
    PrevLink.href = "javascript:;";
    AttachCalEvent(PrevLink, "eval(" + this.ID + ").ShowPrevious();");
    //    var PrevLinkText = document.createTextNode("Prev");
    //    PrevLink.appendChild(PrevLinkText);
    DivHeader.appendChild(PrevLink);

    var NextLink = document.createElement("a");
    NextLink.className = this.NextLinkClassName;
    NextLink.href = "javascript:;";
    AttachCalEvent(NextLink, "eval(" + this.ID + ").ShowNext();");
    //    var NextLinkText = document.createTextNode("Next");
    //    NextLink.appendChild(NextLinkText);
    DivHeader.appendChild(NextLink);

    var HeadingLink = document.createElement("a");
    var strHeadingText = "";
    var strOnClick = "";
    if (this.Mode == Mode_Dates) {
        strHeadingText = monthArray[this.CurrentDate.getMonth()] + ", " + this.CurrentDate.getFullYear();
        strOnClick = "eval(" + this.ID + ").SetMode('Months');";
    }
    else if (this.Mode == Mode_Months) {
        strHeadingText = this.CurrentDate.getFullYear();
        strOnClick = "eval(" + this.ID + ").SetMode('Years');";
    }
    else if (this.Mode == Mode_Years) {
        strHeadingText = (this.YearsToDisplay[0] + 1) + " - " + (this.YearsToDisplay[this.YearsToDisplay.length - 1] - 1);
        strOnClick = "eval(" + this.ID + ").SetMode('Years');";
    }

    HeadingLink.href = "javascript:;";
    if (this.Mode != Mode_Years) {
        AttachCalEvent(HeadingLink, strOnClick);
    }
    var HeadingLinkText = document.createTextNode(strHeadingText);
    HeadingLink.appendChild(HeadingLinkText);
    DivHeader.appendChild(HeadingLink);

    return DivHeader;
};

Calendar.prototype.CreateFooter = function() {
    /*
    <div class="cal-footer">
    <a href="#" class="close right">close</a> <a href="#">This Month</a>
    </div>
    */

    var DivFooter = document.createElement("div");
    DivFooter.className = this.DivFooterClassName;

    var closeLink = document.createElement("a");
    closeLink.className = this.CloseLinkClassName;
    closeLink.href = "javascript:;";
    AttachCalEvent(closeLink, "eval(" + this.ID + ").HideCalendar();");
    var closeLinkText = document.createTextNode("close");
    closeLink.appendChild(closeLinkText);
    closeLink.className="popupclose";
    DivFooter.appendChild(closeLink);

    var ThisMonthLink = document.createElement("a");
    ThisMonthLink.href = "javascript:;";
    AttachCalEvent(ThisMonthLink, "eval(" + this.ID + ").ShowThisMonth();");
    var ThisMonthLinkText = document.createTextNode("This Month");
    ThisMonthLink.appendChild(ThisMonthLinkText);
    DivFooter.appendChild(ThisMonthLink);

    return DivFooter;
};

/*******************************Start Dates Specific*************************************/
Calendar.prototype.GetDatesToDisplay = function(month, year) {

    var dateArray = new Array();
    for (i = 1; i <= this.DaysInAMonth(month, year); i++) {
        dateArray[i - 1] = new Date(year, month - 1, i);
    }

    var compositeArray = new Array();

    var iCount = 0;
    var prevMonthArray = this.GetPreviousMonthDates(this.GetPreviousMonthDatesCount(dateArray[0]), month, year);
    //alert(prevMonthArray.length);
    for (i = 0; i < prevMonthArray.length; i++) {
        compositeArray[iCount] = prevMonthArray[i];
        iCount++;
    }

    for (i = 0; i < dateArray.length; i++) {
        compositeArray[iCount] = dateArray[i];
        iCount++;
    }

    var bDoubleCount = false;
    //alert(42 - iCount - 1);
    if (42 - iCount - 1 >= 7) {
        bDoubleCount = true;
    }

    //alert(this.GetNextMonthDatesCount(bDoubleCount, dateArray[dateArray.length - 1]));

    var nextMonthArray = this.GetNextMonthDates(42 - iCount, month, year);
    //alert(nextMonthArray.length);
    for (i = 0; i < nextMonthArray.length; i++) {
        compositeArray[iCount] = nextMonthArray[i];
        iCount++;
    }

    return compositeArray;
};

Calendar.prototype.GetPreviousMonthDates = function(count, month, year) {
    month -= 1;
    var dateArray = new Array();
    for (i = 1; i <= this.DaysInAMonth(month, year); i++) {
        dateArray[i - 1] = new Date(year, month - 1, i);
    }

    var prevDatesArray = new Array();
    var index = 0;
    for (i = dateArray.length - count; i < dateArray.length; i++) {
        prevDatesArray[index] = dateArray[i];
        index++;
    }

    return prevDatesArray;
};

Calendar.prototype.GetNextMonthDates = function(count, month, year) {
    month += 1;
    var dateArray = new Array();
    for (i = 1; i <= this.DaysInAMonth(month, year); i++) {
        dateArray[i - 1] = new Date(year, month - 1, i);
    }

    var nextDatesArray = new Array();
    for (i = 0; i < count; i++) {
        nextDatesArray[i] = dateArray[i];
    }

    return nextDatesArray;
};


Calendar.prototype.GetPreviousMonthDatesCount = function(date) {
    return date.getDay();
};

/******************Dates Specific*************************************/

Calendar.prototype.GetNextMonthDatesCount = function(bDoubleCount, date) {
    if (bDoubleCount)
        return (13 - date.getDay());
    else
        return (6 - date.getDay());
};

Calendar.prototype.DaysInAMonth = function(month, year) {
    var dd = new Date(year, month, 0);
    return dd.getDate();
};

Calendar.prototype.RenderDates = function() {
    /*
    <div class="day">
    <div class="cal-week-head">
    <span>S</span> <span>M</span> <span>T</span> <span>W</span> <span>T</span> <span>F</span>
    <span>S</span>
    </div>
    <a href="#">1</a> <a href="#">2</a> <a href="#">3</a> <a href="#">4</a> <a href="#">
    5</a> <a href="#">6</a> <a href="#">7</a> <a href="#">8</a> <a href="#">9</a>
    <a href="#">10</a> <a href="#">11</a> <a href="#">12</a> <a href="#">13</a> <a href="#">
    14</a> <a href="#">15</a> <a href="#">16</a> <a href="#" class="today">17</a>
    <a href="#">18</a> <a href="#">19</a> <a href="#">20</a> <a href="#">21</a> <a href="#">
    22</a> <a href="#">23</a> <a href="#">24</a> <a href="#">25</a> <a href="#">26</a>
    <a href="#">27</a> <a href="#">28</a> <a href="#">29</a> <a href="#">30</a> <a href="#">
    31</a> <a href="#" class="notthismonth">1</a> <a href="#" class="notthismonth">2</a>
    <a href="#" class="notthismonth">3</a> <a href="#" class="notthismonth">4</a>
    <div class="clear-l">
    </div>
    </div>
    */

    var DivDates = document.createElement("div");
    DivDates.className = this.DivDatesClassName;

    var DivWeekNames = document.createElement("div");
    DivWeekNames.className = this.DivWeekNamesClassName;

    for (iColumn = 0; iColumn < this.Columns; iColumn++) {
        var spanWeekName = document.createElement("span");
        spanWeekName.innerHTML = weekday[iColumn];
        DivWeekNames.appendChild(spanWeekName);
    }

    DivDates.appendChild(DivWeekNames);
    //alert(this.MinDate);
    //alert(this.MaxDate);
    for (var i = 0; i < this.DatesToDisplay.length; i++) {
        var dateLink = document.createElement("a");
        if (this.CurrentDate.getMonth() != this.DatesToDisplay[i].getMonth()) {
            dateLink.className = this.DivDateNotInThisMonthClassName;
        }

        if (this.TextBoxDate.getDate() == this.DatesToDisplay[i].getDate() && this.TextBoxDate.getMonth() == this.DatesToDisplay[i].getMonth() && this.TextBoxDate.getFullYear() == this.DatesToDisplay[i].getFullYear()) {
            dateLink.className = this.DivDateTodayClassName;
        }

        dateLink.href = "javascript:;";
        if (Date.parse(this.DatesToDisplay[i]) >= Date.parse(this.MinDate) && Date.parse(this.DatesToDisplay[i]) <= Date.parse(this.MaxDate)) {
            AttachCalEvent(dateLink, "eval(" + this.ID + ").OnSetDate('" + this.DatesToDisplay[i] + "');");
        }
        else {
            if (this.CurrentDate.getMonth() != this.DatesToDisplay[i].getMonth()) {
                dateLink.className = this.DivDateDisabledNotThisMonthClassName;
            }
            else {
                dateLink.className = this.DivDateDisabledClassName;
            }
        }
        //AttachCalEvent(dateLink, "eval(" + this.ID + ").OnSetDate('" + this.DatesToDisplay[i] + "');");
        var dateLinkText = document.createTextNode(this.DatesToDisplay[i].getDate());
        dateLink.appendChild(dateLinkText);
        DivDates.appendChild(dateLink);
    }

    var DivClear = document.createElement("div");
    DivClear.className = this.DivClearClassName;
    DivDates.appendChild(DivClear);

    return DivDates;
}

/*******************************Months***************************************/

Calendar.prototype.GetMonthsToDisplay = function(year) {
    var monthArray = new Array("0", "1", "2", "3", "4", "5", "6", "7",
                    "8", "9", "10", "11");
    return monthArray;
};

Calendar.prototype.RenderMonths = function() {

    /*
    <div class="month">
    <a href="#">Jan</a> <a href="#">Feb</a> <a href="#">Mar</a> <a href="#">Apr</a>
    <a href="#">May</a> <a href="#" class="thismonth">Jun</a> <a href="#">Jul</a> <a
    href="#">Aug</a> <a href="#">Sep</a> <a href="#">Oct</a> <a href="#">Nov</a>
    <a href="#">Dec</a>
    <div class="clear-l">
    </div>
    </div>
    */

    var DivMonths = document.createElement("div");
    DivMonths.className = this.DivMonthsClassName;

    for (var i = 0; i < this.MonthsToDisplay.length; i++) {
        var monthLink = document.createElement("a");
        //        if (this.CurrentDate.getMonth() == this.MonthsToDisplay[i] && this.CurrentDate.getFullYear() == (new Date()).getFullYear()) {
        //            monthLink.className = this.DivThisMonthClassName;
        //        }
        monthLink.href = "javascript:;";

        AttachCalEvent(monthLink, "eval(" + this.ID + ").OnMonthClicked('" + this.MonthsToDisplay[i] + "');");
        var monthLinkText = document.createTextNode(monthArray[this.MonthsToDisplay[i]]);
        monthLink.appendChild(monthLinkText);
        DivMonths.appendChild(monthLink);
    }

    var DivClear = document.createElement("div");
    DivClear.className = this.DivClearClassName;
    DivMonths.appendChild(DivClear);

    return DivMonths;
};

/***********************************End Month***************************************/

/***********************************Start Year***************************************/

Calendar.prototype.GetYearsToDisplay = function(year) {
    var year = this.YearRangeLastValue;

    if (year == null) {
        year = this.CurrentDate.getFullYear();
    }

    while (year % 10 != 0) {
        year--;
    }

    var yearArray = new Array();
    var index = 0;
    for (i = year - 1; i <= year + 10; i++) {
        yearArray[index] = i;
        index++;
    }

    this.YearRangeLastValue = yearArray[yearArray.length - 2] + 1;

    return yearArray;
};

Calendar.prototype.RenderYears = function() {
    /*
    <div class="year">
    <a href="#">1999</a> <a href="#">2000</a> <a href="#">2001</a> <a href="#">2002</a>
    <a href="#">2003</a> <a href="#">2004</a> <a href="#">2005</a> <a href="#">2006</a>
    <a href="#" class="thisyear">2007</a> <a href="#">2008</a> <a href="#">2009</a>
    <div class="clear-l">
    </div>
    </div>  
    */

    var DivYears = document.createElement("div");
    DivYears.className = this.DivYearsClassName;

    for (var i = 0; i < this.YearsToDisplay.length; i++) {
        var YearLink = document.createElement("a");
        //        if ((new Date()).getFullYear() == this.YearsToDisplay[i]) {
        //            YearLink.className = this.DivThisYearClassName;
        //        }
        YearLink.href = "javascript:;";

        AttachCalEvent(YearLink, "eval(" + this.ID + ").OnYearClicked('" + this.YearsToDisplay[i] + "');");
        var YearLinkText = document.createTextNode(this.YearsToDisplay[i]);
        YearLink.appendChild(YearLinkText);
        DivYears.appendChild(YearLink);
    }

    var DivClear = document.createElement("div");
    DivClear.className = this.DivClearClassName;
    DivYears.appendChild(DivClear);

    return DivYears;
};

/***********************************End Year***************************************/

function ShowCalendar(e, obj) {
    HideContainerArrayItems();
    eval(obj).ShowCalendar();
}

function Fade(id, opacStart, opacEnd, millisec) {
    //speed for each frame
    var speed = Math.round(millisec / 100);
    var timer = 0;

    //determine the direction for the blending, if start and end are the same nothing happens
    if (opacStart > opacEnd) {
        for (i = opacStart; i >= opacEnd; i--) {
            setTimeout("changeOpac(" + i + ",'" + id + "')", (timer * speed));
            timer++;
        }
    } else if (opacStart < opacEnd) {
        for (i = opacStart; i <= opacEnd; i++) {
            setTimeout("changeOpac(" + i + ",'" + id + "')", (timer * speed));
            timer++;
        }
    }
}

//change the opacity for different browsers
function changeOpac(opacity, id) {
    var object = document.getElementById(id).style;

    // IE/Win 
    object.filter = "alpha(opacity=" + opacity + ")";

    // Safari<1.2, Konqueror
    object.KhtmlOpacity = (opacity / 100);

    // Older Mozilla and Firefox
    object.MozOpacity = (opacity / 100);

    // Safari 1.2, newer Firefox and Mozilla, CSS3
    object.opacity = (opacity / 100);
}

function AttachCalEvent(domEle, jsFunction) {
    if (domEle.attachEvent) {
        domEle.onclick = new Function(jsFunction);
    }
    else {
        domEle.setAttribute("onclick", jsFunction);
    }
}
/*************************************END DATE PICKER********************************************/
/*************************************HIDE MENUS*************************************************/
document.onclick = HidePRIFACTMenus;
function HidePRIFACTMenus() {
    var cbdiv = document.getElementsByTagName("span");

    for (var i = 0; i < cbdiv.length; i++) {
        if (cbdiv[i].style != null
            && cbdiv[i].style.display != null
            && cbdiv[i].style.display == "block"
            && cbdiv[i].id.indexOf("_cblayer") != -1) {
            cbdiv[i].style.display = "none";
            cbdiv[i].parentNode.style.position = "static";
        }
        //added by arun to fix the tooltip issue
        if (cbdiv[i].id.indexOf("PRIFACT_webcontrols_tip_div_") != -1) {
            cbdiv[i].style.display = "none";
        }
    }

    HideContainerArrayItems();

    var cbdp = document.getElementsByTagName("div");

    //    for (var i = 0; i < cbdp.length; i++) {
    //        if (cbdp[i].style != null
    //            && cbdp[i].style.visibility != null
    //            && cbdp[i].style.visibility == "visible"
    //            && cbdp[i].id.indexOf(datePickerDivID) != -1) {
    //            cbdp[i].style.visibility = "hidden";
    //            cbdp[i].style.display = "none";
    //        }
    //    }

    var cbdpi = document.getElementsByTagName("iframe");

    for (var i = 0; i < cbdpi.length; i++) {
        if (cbdpi[i].style != null
            && cbdpi[i].style.visibility != null
            && cbdpi[i].style.visibility == "visible"
            && cbdpi[i].id.indexOf(iFrameDivID) != -1) {
            cbdpi[i].style.visibility = "hidden";
            cbdpi[i].style.display = "none";
        }
    }

}
/*************************************HIDE MENUS*************************************************/



/*************************************TIPS FOR CONTROLS*************************************************/

function SetPRIFACTCBValue(theImg, HiddenValueFieldName, DivArrayName, SelImageUrl, NoSelImageUrl, strOnClientClick) {
    if (theImg.src.toLowerCase().indexOf(NoSelImageUrl.toLowerCase()) != -1) {
        theImg.src = SelImageUrl;
        document.getElementById(HiddenValueFieldName).value = '1';
        //debugger;
        if (strOnClientClick.length > 0) {
            eval(strOnClientClick)(true);
        }
    }
    else {
        theImg.src = NoSelImageUrl;
        document.getElementById(HiddenValueFieldName).value = '0';
        if (strOnClientClick.length > 0) {
            eval(strOnClientClick)(false);
        }
    }

    for (i = 0; i < DivArrayName.length; i++) {
        var theVal = DivArrayName[i];
        if (theImg.getAttribute('id') != theVal)
            document.getElementById(theVal).src = NoSelImageUrl;
    }
}

function SetPRIFACTRBLValue(theImg, HiddenValueFieldName, DivArrayName, SelImageUrl, NoSelImageUrl, strOnClientClick) {
    document.getElementById(HiddenValueFieldName).value = theImg.getAttribute('value');

    if (theImg.src.toLowerCase().indexOf(NoSelImageUrl.toLowerCase()) != -1) {
        theImg.src = SelImageUrl;
        if (strOnClientClick.length > 0) {
            eval(strOnClientClick)(true);
        }
    }
    else {
    }

    for (i = 0; i < DivArrayName.length; i++) {
        var theVal = DivArrayName[i];
        if (theImg.getAttribute('id') != theVal)
            document.getElementById(theVal).src = NoSelImageUrl;

        if (strOnClientClick.length > 0) {
            eval(strOnClientClick)(false);
        }
    }
}

/*Start Time Conversions*/

function InitializeTimeControl(HidItemID, TxtBoxID) {
    var ObjHidItem = document.getElementById(HidItemID);
    var ObjTxtBox = document.getElementById(TxtBoxID);

    if (ObjHidItem != null && ObjTxtBox != null) {
        ObjHidItem.value = GetFormattedTimeString(GetHourHalfHourAdjustedTime());
        ObjTxtBox.value = GetFormattedTimeToShow(GetHourHalfHourAdjustedTime());
    }
}

function GetHourHalfHourAdjustedTime() {
    var dt = new Date();

    var iMinutes = dt.getMinutes();
    var iHours = dt.getHours();
    if (iMinutes <= 15) {
        iMinutes = 0;
    }
    else if (iMinutes >= 45) {
        iMinutes = 0;
        if (iHours == 23) {
            iMinutes = 30;
        }
        else {
            iHours += 1;
        }
    }
    else {
        iMinutes = 30;
    }


    dt.setHours(iHours, iMinutes, 0, 0);


    return dt;
}

function GetFormattedTimeToShow(dt) {
    var dtGFTTS = new Date(dt);

    var iMinutes = dtGFTTS.getMinutes();
    var iHours = dtGFTTS.getHours();

    if (iHours == 0) {
        if (iMinutes < 10) {
            return "12" + ":0" + iMinutes + " AM";
        }
        else {
            return "12" + ":" + iMinutes + " AM";
        }
    }
    else if (iHours < 10) {
        if (iMinutes < 10) {
            return "0" + iHours + ":0" + iMinutes + " AM";
        }
        else {
            return "0" + iHours + ":" + iMinutes + " AM";
        }
    }
    else if (iHours < 12) {
        if (iMinutes < 10) {
            return iHours + ":0" + iMinutes + " AM";
        }
        else {
            return iHours + ":" + iMinutes + " AM";
        }
    }
    else if (iHours == 12) {
        if (iMinutes < 10) {
            return iHours + ":0" + iMinutes + " PM";
        }
        else {
            return iHours + ":" + iMinutes + " PM";
        }
    }
    else if (iHours == 23) {
        if (iMinutes < 10) {
            return (iHours - 12) + ":0" + iMinutes + " PM";
        }
        else {
            return (iHours - 12) + ":" + iMinutes + " PM";
        }
    }
    else {
        var hNew = iHours - 12
        if (hNew < 10) {
            hNew = "0" + hNew;
        }
        if (iMinutes < 10) {
            return hNew + ":0" + iMinutes + " PM";
        }
        else {
            return hNew + ":" + iMinutes + " PM";
        }
    }
}


function GetFormattedTimeString(dt) {
    dtGFTS = new Date(dt);

    var iMinutes = dtGFTS.getMinutes();
    var iHours = dtGFTS.getHours();

    if (iHours < 10) {
        if (dt.Minute < 10) {
            return "0" + iHours + ":0" + iMinutes;
        }
        else {
            return "0" + iHours + ":" + iMinutes;
        }
    }
    else {
        if (dt.Minute < 10) {
            return iHours + ":0" + iMinutes;
        }
        else {
            return iHours + ":" + iMinutes;
        }

    }
}
/*End Time Conversions*/

function IsBrowserFixedPositionCompatible() {
    if (navigator.appName != "Microsoft Internet Explorer") {
        return true;
    }

    if (navigator.appVersion.indexOf("MSIE 4.") != -1 || navigator.appVersion.indexOf("MSIE 5.") != -1 || navigator.appVersion.indexOf("MSIE 6.") != -1) {
        return false;
    }

    return true;
}


/*wz_tooltip.js*/
/* This notice must be untouched at all times.
Copyright (c) 2002-2008 Walter Zorn. All rights reserved.

wz_tooltip.js	 v. 5.31

The latest version is available at
http://www.walterzorn.com
or http://www.devira.com
or http://www.walterzorn.de

Created 1.12.2002 by Walter Zorn (Web: http://www.walterzorn.com )
Last modified: 7.11.2008

Easy-to-use cross-browser tooltips.
Just include the script at the beginning of the <body> section, and invoke
Tip('Tooltip text') to show and UnTip() to hide the tooltip, from the desired
HTML eventhandlers. Example:
<a onmouseover="Tip('Some text')" onmouseout="UnTip()" href="index.htm">My home page</a>
No container DIV required.
By default, width and height of tooltips are automatically adapted to content.
Is even capable of dynamically converting arbitrary HTML elements to tooltips
by calling TagToTip('ID_of_HTML_element_to_be_converted') instead of Tip(),
which means you can put important, search-engine-relevant stuff into tooltips.
Appearance & behaviour of tooltips can be individually configured
via commands passed to Tip() or TagToTip().

Tab Width: 4
LICENSE: LGPL

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License (LGPL) as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

For more details on the GNU Lesser General Public License,
see http://www.gnu.org/copyleft/lesser.html
*/

var config = new Object();


//===================  GLOBAL TOOLTIP CONFIGURATION  =========================//
var tt_Debug = true		// false or true - recommended: false once you release your page to the public
var tt_Enabled = true		// Allows to (temporarily) suppress tooltips, e.g. by providing the user with a button that sets this global variable to false
var TagsToTip = true		// false or true - if true, HTML elements to be converted to tooltips via TagToTip() are automatically hidden;
// if false, you should hide those HTML elements yourself

// For each of the following config variables there exists a command, which is
// just the variablename in uppercase, to be passed to Tip() or TagToTip() to
// configure tooltips individually. Individual commands override global
// configuration. Order of commands is arbitrary.
// Example: onmouseover="Tip('Tooltip text', LEFT, true, BGCOLOR, '#FF9900', FADEIN, 400)"

config.Above = false		// false or true - tooltip above mousepointer
config.BgColor = '#E2E7FF'	// Background colour (HTML colour value, in quotes)
config.BgImg = ''		// Path to background image, none if empty string ''
config.BorderColor = '#003099'
config.BorderStyle = 'solid'	// Any permitted CSS value, but I recommend 'solid', 'dotted' or 'dashed'
config.BorderWidth = 1
config.CenterMouse = false		// false or true - center the tip horizontally below (or above) the mousepointer
config.ClickClose = false		// false or true - close tooltip if the user clicks somewhere
config.ClickSticky = false		// false or true - make tooltip sticky if user left-clicks on the hovered element while the tooltip is active
config.CloseBtn = false		// false or true - closebutton in titlebar
config.CloseBtnColors = ['#990000', '#FFFFFF', '#DD3333', '#FFFFFF']	// [Background, text, hovered background, hovered text] - use empty strings '' to inherit title colours
config.CloseBtnText = '&nbsp;X&nbsp;'	// Close button text (may also be an image tag)
config.CopyContent = true		// When converting a HTML element to a tooltip, copy only the element's content, rather than converting the element by its own
config.Delay = 400		// Time span in ms until tooltip shows up
config.Duration = 0			// Time span in ms after which the tooltip disappears; 0 for infinite duration, < 0 for delay in ms _after_ the onmouseout until the tooltip disappears
config.Exclusive = false		// false or true - no other tooltip can appear until the current one has actively been closed
config.FadeIn = 100		// Fade-in duration in ms, e.g. 400; 0 for no animation
config.FadeOut = 100
config.FadeInterval = 30		// Duration of each fade step in ms (recommended: 30) - shorter is smoother but causes more CPU-load
config.Fix = null		// Fixated position, two modes. Mode 1: x- an y-coordinates in brackets, e.g. [210, 480]. Mode 2: Show tooltip at a position related to an HTML element: [ID of HTML element, x-offset, y-offset from HTML element], e.g. ['SomeID', 10, 30]. Value null (default) for no fixated positioning.
config.FollowMouse = true		// false or true - tooltip follows the mouse
config.FontColor = '#000044'
config.FontFace = 'Verdana,Geneva,sans-serif'
config.FontSize = '8pt'		// E.g. '9pt' or '12px' - unit is mandatory
config.FontWeight = 'normal'	// 'normal' or 'bold';
config.Height = 0			// Tooltip height; 0 for automatic adaption to tooltip content, < 0 (e.g. -100) for a maximum for automatic adaption
config.JumpHorz = false		// false or true - jump horizontally to other side of mouse if tooltip would extend past clientarea boundary
config.JumpVert = true		// false or true - jump vertically		"
config.Left = false		// false or true - tooltip on the left of the mouse
config.OffsetX = 14		// Horizontal offset of left-top corner from mousepointer
config.OffsetY = 8			// Vertical offset
config.Opacity = 100		// Integer between 0 and 100 - opacity of tooltip in percent
config.Padding = 3			// Spacing between border and content
config.Shadow = false		// false or true
config.ShadowColor = '#C0C0C0'
config.ShadowWidth = 5
config.Sticky = false		// false or true - fixate tip, ie. don't follow the mouse and don't hide on mouseout
config.TextAlign = 'left'	// 'left', 'right' or 'justify'
config.Title = ''		// Default title text applied to all tips (no default title: empty string '')
config.TitleAlign = 'left'	// 'left' or 'right' - text alignment inside the title bar
config.TitleBgColor = ''		// If empty string '', BorderColor will be used
config.TitleFontColor = '#FFFFFF'	// Color of title text - if '', BgColor (of tooltip body) will be used
config.TitleFontFace = ''		// If '' use FontFace (boldified)
config.TitleFontSize = ''		// If '' use FontSize
config.TitlePadding = 2
config.Width = 0			// Tooltip width; 0 for automatic adaption to tooltip content; < -1 (e.g. -240) for a maximum width for that automatic adaption;
// -1: tooltip width confined to the width required for the titlebar
//=======  END OF TOOLTIP CONFIG, DO NOT CHANGE ANYTHING BELOW  ==============//




//=====================  PUBLIC  =============================================//
function Tip() {
    tt_Tip(arguments, null);
}
function TagToTip() {
    var t2t = tt_GetElt(arguments[0]);
    if (t2t)
        tt_Tip(arguments, t2t);
}
function UnTip() {
    tt_OpReHref();
    if (tt_aV[DURATION] < 0 && (tt_iState & 0x2))
        tt_tDurt.Timer("tt_HideInit()", -tt_aV[DURATION], true);
    else if (!(tt_aV[STICKY] && (tt_iState & 0x2)))
        tt_HideInit();
}

//==================  PUBLIC PLUGIN API	 =====================================//
// Extension eventhandlers currently supported:
// OnLoadConfig, OnCreateContentString, OnSubDivsCreated, OnShow, OnMoveBefore,
// OnMoveAfter, OnHideInit, OnHide, OnKill

var tt_aElt = new Array(10), // Container DIV, outer title & body DIVs, inner title & body TDs, closebutton SPAN, shadow DIVs, and IFRAME to cover windowed elements in IE
tt_aV = new Array(), // Caches and enumerates config data for currently active tooltip
tt_sContent, 		// Inner tooltip text or HTML
tt_t2t, tt_t2tDad, 	// Tag converted to tip, and its DOM parent element
tt_musX, tt_musY,
tt_over,
tt_x, tt_y, tt_w, tt_h; // Position, width and height of currently displayed tooltip

function tt_Extension() {
    tt_ExtCmdEnum();
    tt_aExt[tt_aExt.length] = this;
    return this;
}
function tt_SetTipPos(x, y) {
    var css = tt_aElt[0].style;

    tt_x = x;
    tt_y = y;
    css.left = x + "px";
    css.top = y + "px";
    if (tt_ie56) {
        var ifrm = tt_aElt[tt_aElt.length - 1];
        if (ifrm) {
            ifrm.style.left = css.left;
            ifrm.style.top = css.top;
        }
    }
}
function tt_HideInit() {
    if (tt_iState) {
        tt_ExtCallFncs(0, "HideInit");
        tt_iState &= ~(0x4 | 0x8);
        if (tt_flagOpa && tt_aV[FADEOUT]) {
            tt_tFade.EndTimer();
            if (tt_opa) {
                var n = Math.round(tt_aV[FADEOUT] / (tt_aV[FADEINTERVAL] * (tt_aV[OPACITY] / tt_opa)));
                tt_Fade(tt_opa, tt_opa, 0, n);
                return;
            }
        }
        tt_tHide.Timer("tt_Hide();", 1, false);
    }
}
function tt_Hide() {
    if (tt_db && tt_iState) {
        tt_OpReHref();
        if (tt_iState & 0x2) {
            tt_aElt[0].style.visibility = "hidden";
            tt_ExtCallFncs(0, "Hide");
        }
        tt_tShow.EndTimer();
        tt_tHide.EndTimer();
        tt_tDurt.EndTimer();
        tt_tFade.EndTimer();
        if (!tt_op && !tt_ie) {
            tt_tWaitMov.EndTimer();
            tt_bWait = false;
        }
        if (tt_aV[CLICKCLOSE] || tt_aV[CLICKSTICKY])
            tt_RemEvtFnc(document, "mouseup", tt_OnLClick);
        tt_ExtCallFncs(0, "Kill");
        // In case of a TagToTip tip, hide converted DOM node and
        // re-insert it into DOM
        if (tt_t2t && !tt_aV[COPYCONTENT])
            tt_UnEl2Tip();
        tt_iState = 0;
        tt_over = null;
        tt_ResetMainDiv();
        if (tt_aElt[tt_aElt.length - 1])
            tt_aElt[tt_aElt.length - 1].style.display = "none";
    }
}
function tt_GetElt(id) {
    return (document.getElementById ? document.getElementById(id)
			: document.all ? document.all[id]
			: null);
}
function tt_GetDivW(el) {
    return (el ? (el.offsetWidth || el.style.pixelWidth || 0) : 0);
}
function tt_GetDivH(el) {
    return (el ? (el.offsetHeight || el.style.pixelHeight || 0) : 0);
}
function tt_GetScrollX() {
    return (window.pageXOffset || (tt_db ? (tt_db.scrollLeft || 0) : 0));
}
function tt_GetScrollY() {
    return (window.pageYOffset || (tt_db ? (tt_db.scrollTop || 0) : 0));
}
function tt_GetClientW() {
    return tt_GetWndCliSiz("Width");
}
function tt_GetClientH() {
    return tt_GetWndCliSiz("Height");
}
function tt_GetEvtX(e) {
    return (e ? ((typeof (e.pageX) != tt_u) ? e.pageX : (e.clientX + tt_GetScrollX())) : 0);
}
function tt_GetEvtY(e) {
    return (e ? ((typeof (e.pageY) != tt_u) ? e.pageY : (e.clientY + tt_GetScrollY())) : 0);
}
function tt_AddEvtFnc(el, sEvt, PFnc) {
    if (el) {
        if (el.addEventListener)
            el.addEventListener(sEvt, PFnc, false);
        else
            el.attachEvent("on" + sEvt, PFnc);
    }
}
function tt_RemEvtFnc(el, sEvt, PFnc) {
    if (el) {
        if (el.removeEventListener)
            el.removeEventListener(sEvt, PFnc, false);
        else
            el.detachEvent("on" + sEvt, PFnc);
    }
}
function tt_GetDad(el) {
    return (el.parentNode || el.parentElement || el.offsetParent);
}
function tt_MovDomNode(el, dadFrom, dadTo) {
    if (dadFrom)
        dadFrom.removeChild(el);
    if (dadTo)
        dadTo.appendChild(el);
}

//======================  PRIVATE  ===========================================//
var tt_aExt = new Array(), // Array of extension objects

tt_db, tt_op, tt_ie, tt_ie56, tt_bBoxOld, // Browser flags
tt_body,
tt_ovr_, 			// HTML element the mouse is currently over
tt_flagOpa, 			// Opacity support: 1=IE, 2=Khtml, 3=KHTML, 4=Moz, 5=W3C
tt_maxPosX, tt_maxPosY,
tt_iState = 0, 		// Tooltip active |= 1, shown |= 2, move with mouse |= 4, exclusive |= 8
tt_opa, 				// Currently applied opacity
tt_bJmpVert, tt_bJmpHorz, // Tip temporarily on other side of mouse
tt_elDeHref, 		// The tag from which we've removed the href attribute
// Timer
tt_tShow = new Number(0), tt_tHide = new Number(0), tt_tDurt = new Number(0),
tt_tFade = new Number(0), tt_tWaitMov = new Number(0),
tt_bWait = false,
tt_u = "undefined";


function tt_Init() {
    tt_MkCmdEnum();
    // Send old browsers instantly to hell
    if (!tt_Browser() || !tt_MkMainDiv())
        return;
    tt_IsW3cBox();
    tt_OpaSupport();
    tt_AddEvtFnc(document, "mousemove", tt_Move);
    // In Debug mode we search for TagToTip() calls in order to notify
    // the user if they've forgotten to set the TagsToTip config flag
    if (TagsToTip || tt_Debug)
        tt_SetOnloadFnc();
    // Ensure the tip be hidden when the page unloads
    tt_AddEvtFnc(window, "unload", tt_Hide);
}
// Creates command names by translating config variable names to upper case
function tt_MkCmdEnum() {
    var n = 0;
    for (var i in config)
        eval("window." + i.toString().toUpperCase() + " = " + n++);
    tt_aV.length = n;
}
function tt_Browser() {
    var n, nv, n6, w3c;

    n = navigator.userAgent.toLowerCase(),
	nv = navigator.appVersion;
    tt_op = (document.defaultView && typeof (eval("w" + "indow" + "." + "o" + "p" + "er" + "a")) != tt_u);
    tt_ie = n.indexOf("msie") != -1 && document.all && !tt_op;
    if (tt_ie) {
        var ieOld = (!document.compatMode || document.compatMode == "BackCompat");
        tt_db = !ieOld ? document.documentElement : (document.body || null);
        if (tt_db)
            tt_ie56 = parseFloat(nv.substring(nv.indexOf("MSIE") + 5)) >= 5.5
					&& typeof document.body.style.maxHeight == tt_u;
    }
    else {
        tt_db = document.documentElement || document.body ||
				(document.getElementsByTagName ? document.getElementsByTagName("body")[0]
				: null);
        if (!tt_op) {
            n6 = document.defaultView && typeof document.defaultView.getComputedStyle != tt_u;
            w3c = !n6 && document.getElementById;
        }
    }
    tt_body = (document.getElementsByTagName ? document.getElementsByTagName("body")[0]
				: (document.body || null));
    if (tt_ie || n6 || tt_op || w3c) {
        if (tt_body && tt_db) {
            if (document.attachEvent || document.addEventListener)
                return true;
        }
        else
            tt_Err("wz_tooltip.js must be included INSIDE the body section,"
					+ " immediately after the opening <body> tag.", false);
    }
    tt_db = null;
    return false;
}
function tt_MkMainDiv() {
    // Create the tooltip DIV
    if (tt_body.insertAdjacentHTML)
        tt_body.insertAdjacentHTML("afterBegin", tt_MkMainDivHtm());
    else if (typeof tt_body.innerHTML != tt_u && document.createElement && tt_body.appendChild)
        tt_body.appendChild(tt_MkMainDivDom());
    if (window.tt_GetMainDivRefs /* FireFox Alzheimer */ && tt_GetMainDivRefs())
        return true;
    tt_db = null;
    return false;
}
function tt_MkMainDivHtm() {
    return (
		'<div id="WzTtDiV"></div>' +
		(tt_ie56 ? ('<iframe id="WzTtIfRm" src="javascript:false" scrolling="no" frameborder="0" style="filter:Alpha(opacity=0);position:absolute;top:0px;left:0px;display:none;"></iframe>')
		: '')
	);
}
function tt_MkMainDivDom() {
    var el = document.createElement("div");
    if (el)
        el.id = "WzTtDiV";
    return el;
}
function tt_GetMainDivRefs() {
    tt_aElt[0] = tt_GetElt("WzTtDiV");
    if (tt_ie56 && tt_aElt[0]) {
        tt_aElt[tt_aElt.length - 1] = tt_GetElt("WzTtIfRm");
        if (!tt_aElt[tt_aElt.length - 1])
            tt_aElt[0] = null;
    }
    if (tt_aElt[0]) {
        var css = tt_aElt[0].style;

        css.visibility = "hidden";
        css.position = "absolute";
        css.overflow = "hidden";
        return true;
    }
    return false;
}
function tt_ResetMainDiv() {
    tt_SetTipPos(0, 0);
    tt_aElt[0].innerHTML = "";
    tt_aElt[0].style.width = "0px";
    tt_h = 0;
}
function tt_IsW3cBox() {
    var css = tt_aElt[0].style;

    css.padding = "10px";
    css.width = "40px";
    tt_bBoxOld = (tt_GetDivW(tt_aElt[0]) == 40);
    css.padding = "0px";
    tt_ResetMainDiv();
}
function tt_OpaSupport() {
    var css = tt_body.style;

    tt_flagOpa = (typeof (css.KhtmlOpacity) != tt_u) ? 2
				: (typeof (css.KHTMLOpacity) != tt_u) ? 3
				: (typeof (css.MozOpacity) != tt_u) ? 4
				: (typeof (css.opacity) != tt_u) ? 5
				: (typeof (css.filter) != tt_u) ? 1
				: 0;
}
// Ported from http://dean.edwards.name/weblog/2006/06/again/
// (Dean Edwards et al.)
function tt_SetOnloadFnc() {
    tt_AddEvtFnc(document, "DOMContentLoaded", tt_HideSrcTags);
    tt_AddEvtFnc(window, "load", tt_HideSrcTags);
    if (tt_body.attachEvent)
        tt_body.attachEvent("onreadystatechange",
			function() {
			    if (tt_body.readyState == "complete")
			        tt_HideSrcTags();
			});
    if (/WebKit|KHTML/i.test(navigator.userAgent)) {
        var t = setInterval(function() {
            if (/loaded|complete/.test(document.readyState)) {
                clearInterval(t);
                tt_HideSrcTags();
            }
        }, 10);
    }
}
function tt_HideSrcTags() {
    if (!window.tt_HideSrcTags || window.tt_HideSrcTags.done)
        return;
    window.tt_HideSrcTags.done = true;
    if (!tt_HideSrcTagsRecurs(tt_body))
        tt_Err("There are HTML elements to be converted to tooltips.\nIf you"
				+ " want these HTML elements to be automatically hidden, you"
				+ " must edit wz_tooltip.js, and set TagsToTip in the global"
				+ " tooltip configuration to true.", true);
}
function tt_HideSrcTagsRecurs(dad) {
    var ovr, asT2t;
    // Walk the DOM tree for tags that have an onmouseover or onclick attribute
    // containing a TagToTip('...') call.
    // (.childNodes first since .children is bugous in Safari)
    var a = dad.childNodes || dad.children || null;

    for (var i = a ? a.length : 0; i; ) {
        --i;
        if (!tt_HideSrcTagsRecurs(a[i]))
            return false;
        ovr = a[i].getAttribute ? (a[i].getAttribute("onmouseover") || a[i].getAttribute("onclick"))
				: (typeof a[i].onmouseover == "function") ? (a[i].onmouseover || a[i].onclick)
				: null;
        if (ovr) {
            asT2t = ovr.toString().match(/TagToTip\s*\(\s*'[^'.]+'\s*[\),]/);
            if (asT2t && asT2t.length) {
                if (!tt_HideSrcTag(asT2t[0]))
                    return false;
            }
        }
    }
    return true;
}
function tt_HideSrcTag(sT2t) {
    var id, el;

    // The ID passed to the found TagToTip() call identifies an HTML element
    // to be converted to a tooltip, so hide that element
    id = sT2t.replace(/.+'([^'.]+)'.+/, "$1");
    el = tt_GetElt(id);
    if (el) {
        if (tt_Debug && !TagsToTip)
            return false;
        else
            el.style.display = "none";
    }
    else
        tt_Err("Invalid ID\n'" + id + "'\npassed to TagToTip()."
				+ " There exists no HTML element with that ID.", true);
    return true;
}
function tt_Tip(arg, t2t) {
    if (!tt_db || (tt_iState & 0x8))
        return;
    if (tt_iState)
        tt_Hide();
    if (!tt_Enabled)
        return;
    tt_t2t = t2t;
    if (!tt_ReadCmds(arg))
        return;
    tt_iState = 0x1 | 0x4;
    tt_AdaptConfig1();
    tt_MkTipContent(arg);
    tt_MkTipSubDivs();
    tt_FormatTip();
    tt_bJmpVert = false;
    tt_bJmpHorz = false;
    tt_maxPosX = tt_GetClientW() + tt_GetScrollX() - tt_w - 1;
    tt_maxPosY = tt_GetClientH() + tt_GetScrollY() - tt_h - 1;
    tt_AdaptConfig2();
    // Ensure the tip be shown and positioned before the first onmousemove
    tt_OverInit();
    tt_ShowInit();
    tt_Move();
}
function tt_ReadCmds(a) {
    var i;

    // First load the global config values, to initialize also values
    // for which no command is passed
    i = 0;
    for (var j in config)
        tt_aV[i++] = config[j];
    // Then replace each cached config value for which a command is
    // passed (ensure the # of command args plus value args be even)
    if (a.length & 1) {
        for (i = a.length - 1; i > 0; i -= 2)
            tt_aV[a[i - 1]] = a[i];
        return true;
    }
    tt_Err("Incorrect call of Tip() or TagToTip().\n"
			+ "Each command must be followed by a value.", true);
    return false;
}
function tt_AdaptConfig1() {
    tt_ExtCallFncs(0, "LoadConfig");
    // Inherit unspecified title formattings from body
    if (!tt_aV[TITLEBGCOLOR].length)
        tt_aV[TITLEBGCOLOR] = tt_aV[BORDERCOLOR];
    if (!tt_aV[TITLEFONTCOLOR].length)
        tt_aV[TITLEFONTCOLOR] = tt_aV[BGCOLOR];
    if (!tt_aV[TITLEFONTFACE].length)
        tt_aV[TITLEFONTFACE] = tt_aV[FONTFACE];
    if (!tt_aV[TITLEFONTSIZE].length)
        tt_aV[TITLEFONTSIZE] = tt_aV[FONTSIZE];
    if (tt_aV[CLOSEBTN]) {
        // Use title colours for non-specified closebutton colours
        if (!tt_aV[CLOSEBTNCOLORS])
            tt_aV[CLOSEBTNCOLORS] = new Array("", "", "", "");
        for (var i = 4; i; ) {
            --i;
            if (!tt_aV[CLOSEBTNCOLORS][i].length)
                tt_aV[CLOSEBTNCOLORS][i] = (i & 1) ? tt_aV[TITLEFONTCOLOR] : tt_aV[TITLEBGCOLOR];
        }
        // Enforce titlebar be shown
        if (!tt_aV[TITLE].length)
            tt_aV[TITLE] = " ";
    }
    // Circumvents broken display of images and fade-in flicker in Geckos < 1.8
    if (tt_aV[OPACITY] == 100 && typeof tt_aElt[0].style.MozOpacity != tt_u && !Array.every)
        tt_aV[OPACITY] = 99;
    // Smartly shorten the delay for fade-in tooltips
    if (tt_aV[FADEIN] && tt_flagOpa && tt_aV[DELAY] > 100)
        tt_aV[DELAY] = Math.max(tt_aV[DELAY] - tt_aV[FADEIN], 100);
}
function tt_AdaptConfig2() {
    if (tt_aV[CENTERMOUSE]) {
        tt_aV[OFFSETX] -= ((tt_w - (tt_aV[SHADOW] ? tt_aV[SHADOWWIDTH] : 0)) >> 1);
        tt_aV[JUMPHORZ] = false;
    }
}
// Expose content globally so extensions can modify it
function tt_MkTipContent(a) {
    if (tt_t2t) {
        if (tt_aV[COPYCONTENT])
            tt_sContent = tt_t2t.innerHTML;
        else
            tt_sContent = "";
    }
    else
        tt_sContent = a[0];
    tt_ExtCallFncs(0, "CreateContentString");
}
function tt_MkTipSubDivs() {
    var sCss = 'position:relative;margin:0px;padding:0px;border-width:0px;left:0px;top:0px;line-height:normal;width:auto;',
	sTbTrTd = ' cellspacing="0" cellpadding="0" border="0" style="' + sCss + '"><tbody style="' + sCss + '"><tr><td ';

    tt_aElt[0].style.width = tt_GetClientW() + "px";
    tt_aElt[0].innerHTML =
		(''
		+ (tt_aV[TITLE].length ?
			('<div id="WzTiTl" style="position:relative;z-index:1;">'
			+ '<table id="WzTiTlTb"' + sTbTrTd + 'id="WzTiTlI" style="' + sCss + '">'
			+ tt_aV[TITLE]
			+ '</td>'
			+ (tt_aV[CLOSEBTN] ?
				('<td align="right" style="' + sCss
				+ 'text-align:right;">'
				+ '<span id="WzClOsE" style="position:relative;left:2px;padding-left:2px;padding-right:2px;'
				+ 'cursor:' + (tt_ie ? 'hand' : 'pointer')
				+ ';" onmouseover="tt_OnCloseBtnOver(1)" onmouseout="tt_OnCloseBtnOver(0)" onclick="tt_HideInit()">'
				+ tt_aV[CLOSEBTNTEXT]
				+ '</span></td>')
				: '')
			+ '</tr></tbody></table></div>')
			: '')
		+ '<div id="WzBoDy" style="position:relative;z-index:0;">'
		+ '<table' + sTbTrTd + 'id="WzBoDyI" style="' + sCss + '">'
		+ tt_sContent
		+ '</td></tr></tbody></table></div>'
		+ (tt_aV[SHADOW]
			? ('<div id="WzTtShDwR" style="position:absolute;overflow:hidden;"></div>'
				+ '<div id="WzTtShDwB" style="position:relative;overflow:hidden;"></div>')
			: '')
		);
    tt_GetSubDivRefs();
    // Convert DOM node to tip
    if (tt_t2t && !tt_aV[COPYCONTENT])
        tt_El2Tip();
    tt_ExtCallFncs(0, "SubDivsCreated");
}
function tt_GetSubDivRefs() {
    var aId = new Array("WzTiTl", "WzTiTlTb", "WzTiTlI", "WzClOsE", "WzBoDy", "WzBoDyI", "WzTtShDwB", "WzTtShDwR");

    for (var i = aId.length; i; --i)
        tt_aElt[i] = tt_GetElt(aId[i - 1]);
}
function tt_FormatTip() {
    var css, w, h, pad = tt_aV[PADDING], padT, wBrd = tt_aV[BORDERWIDTH],
	iOffY, iOffSh, iAdd = (pad + wBrd) << 1;

    //--------- Title DIV ----------
    if (tt_aV[TITLE].length) {
        padT = tt_aV[TITLEPADDING];
        css = tt_aElt[1].style;
        css.background = tt_aV[TITLEBGCOLOR];
        css.paddingTop = css.paddingBottom = padT + "px";
        css.paddingLeft = css.paddingRight = (padT + 2) + "px";
        css = tt_aElt[3].style;
        css.color = tt_aV[TITLEFONTCOLOR];
        if (tt_aV[WIDTH] == -1)
            css.whiteSpace = "nowrap";
        css.fontFamily = tt_aV[TITLEFONTFACE];
        css.fontSize = tt_aV[TITLEFONTSIZE];
        css.fontWeight = "bold";
        css.textAlign = tt_aV[TITLEALIGN];
        // Close button DIV
        if (tt_aElt[4]) {
            css = tt_aElt[4].style;
            css.background = tt_aV[CLOSEBTNCOLORS][0];
            css.color = tt_aV[CLOSEBTNCOLORS][1];
            css.fontFamily = tt_aV[TITLEFONTFACE];
            css.fontSize = tt_aV[TITLEFONTSIZE];
            css.fontWeight = "bold";
        }
        if (tt_aV[WIDTH] > 0)
            tt_w = tt_aV[WIDTH];
        else {
            tt_w = tt_GetDivW(tt_aElt[3]) + tt_GetDivW(tt_aElt[4]);
            // Some spacing between title DIV and closebutton
            if (tt_aElt[4])
                tt_w += pad;
            // Restrict auto width to max width
            if (tt_aV[WIDTH] < -1 && tt_w > -tt_aV[WIDTH])
                tt_w = -tt_aV[WIDTH];
        }
        // Ensure the top border of the body DIV be covered by the title DIV
        iOffY = -wBrd;
    }
    else {
        tt_w = 0;
        iOffY = 0;
    }

    //-------- Body DIV ------------
    css = tt_aElt[5].style;
    css.top = iOffY + "px";
    if (wBrd) {
        css.borderColor = tt_aV[BORDERCOLOR];
        css.borderStyle = tt_aV[BORDERSTYLE];
        css.borderWidth = wBrd + "px";
    }
    if (tt_aV[BGCOLOR].length)
        css.background = tt_aV[BGCOLOR];
    if (tt_aV[BGIMG].length)
        css.backgroundImage = "url(" + tt_aV[BGIMG] + ")";
    css.padding = pad + "px";
    css.textAlign = tt_aV[TEXTALIGN];
    if (tt_aV[HEIGHT]) {
        css.overflow = "auto";
        if (tt_aV[HEIGHT] > 0)
            css.height = (tt_aV[HEIGHT] + iAdd) + "px";
        else
            tt_h = iAdd - tt_aV[HEIGHT];
    }
    // TD inside body DIV
    css = tt_aElt[6].style;
    css.color = tt_aV[FONTCOLOR];
    css.fontFamily = tt_aV[FONTFACE];
    css.fontSize = tt_aV[FONTSIZE];
    css.fontWeight = tt_aV[FONTWEIGHT];
    css.textAlign = tt_aV[TEXTALIGN];
    if (tt_aV[WIDTH] > 0)
        w = tt_aV[WIDTH];
    // Width like title (if existent)
    else if (tt_aV[WIDTH] == -1 && tt_w)
        w = tt_w;
    else {
        // Measure width of the body's inner TD, as some browsers would expand
        // the container and outer body DIV to 100%
        w = tt_GetDivW(tt_aElt[6]);
        // Restrict auto width to max width
        if (tt_aV[WIDTH] < -1 && w > -tt_aV[WIDTH])
            w = -tt_aV[WIDTH];
    }
    if (w > tt_w)
        tt_w = w;
    tt_w += iAdd;

    //--------- Shadow DIVs ------------
    if (tt_aV[SHADOW]) {
        tt_w += tt_aV[SHADOWWIDTH];
        iOffSh = Math.floor((tt_aV[SHADOWWIDTH] * 4) / 3);
        // Bottom shadow
        css = tt_aElt[7].style;
        css.top = iOffY + "px";
        css.left = iOffSh + "px";
        css.width = (tt_w - iOffSh - tt_aV[SHADOWWIDTH]) + "px";
        css.height = tt_aV[SHADOWWIDTH] + "px";
        css.background = tt_aV[SHADOWCOLOR];
        // Right shadow
        css = tt_aElt[8].style;
        css.top = iOffSh + "px";
        css.left = (tt_w - tt_aV[SHADOWWIDTH]) + "px";
        css.width = tt_aV[SHADOWWIDTH] + "px";
        css.background = tt_aV[SHADOWCOLOR];
    }
    else
        iOffSh = 0;

    //-------- Container DIV -------
    tt_SetTipOpa(tt_aV[FADEIN] ? 0 : tt_aV[OPACITY]);
    tt_FixSize(iOffY, iOffSh);
}
// Fixate the size so it can't dynamically change while the tooltip is moving.
function tt_FixSize(iOffY, iOffSh) {
    var wIn, wOut, h, add, pad = tt_aV[PADDING], wBrd = tt_aV[BORDERWIDTH], i;

    tt_aElt[0].style.width = tt_w + "px";
    tt_aElt[0].style.pixelWidth = tt_w;
    wOut = tt_w - ((tt_aV[SHADOW]) ? tt_aV[SHADOWWIDTH] : 0);
    // Body
    wIn = wOut;
    if (!tt_bBoxOld)
        wIn -= (pad + wBrd) << 1;
    tt_aElt[5].style.width = wIn + "px";
    // Title
    if (tt_aElt[1]) {
        wIn = wOut - ((tt_aV[TITLEPADDING] + 2) << 1);
        if (!tt_bBoxOld)
            wOut = wIn;
        tt_aElt[1].style.width = wOut + "px";
        tt_aElt[2].style.width = wIn + "px";
    }
    // Max height specified
    if (tt_h) {
        h = tt_GetDivH(tt_aElt[5]);
        if (h > tt_h) {
            if (!tt_bBoxOld)
                tt_h -= (pad + wBrd) << 1;
            tt_aElt[5].style.height = tt_h + "px";
        }
    }
    tt_h = tt_GetDivH(tt_aElt[0]) + iOffY;
    // Right shadow
    if (tt_aElt[8])
        tt_aElt[8].style.height = (tt_h - iOffSh) + "px";
    i = tt_aElt.length - 1;
    if (tt_aElt[i]) {
        tt_aElt[i].style.width = tt_w + "px";
        tt_aElt[i].style.height = tt_h + "px";
    }
}
function tt_DeAlt(el) {
    var aKid;

    if (el) {
        if (el.alt)
            el.alt = "";
        if (el.title)
            el.title = "";
        aKid = el.childNodes || el.children || null;
        if (aKid) {
            for (var i = aKid.length; i; )
                tt_DeAlt(aKid[--i]);
        }
    }
}
// This hack removes the native tooltips over links in Opera
function tt_OpDeHref(el) {
    if (!tt_op)
        return;
    if (tt_elDeHref)
        tt_OpReHref();
    while (el) {
        if (el.hasAttribute && el.hasAttribute("href")) {
            el.t_href = el.getAttribute("href");
            el.t_stats = window.status;
            el.removeAttribute("href");
            el.style.cursor = "hand";
            tt_AddEvtFnc(el, "mousedown", tt_OpReHref);
            window.status = el.t_href;
            tt_elDeHref = el;
            break;
        }
        el = tt_GetDad(el);
    }
}
function tt_OpReHref() {
    if (tt_elDeHref) {
        tt_elDeHref.setAttribute("href", tt_elDeHref.t_href);
        tt_RemEvtFnc(tt_elDeHref, "mousedown", tt_OpReHref);
        window.status = tt_elDeHref.t_stats;
        tt_elDeHref = null;
    }
}
function tt_El2Tip() {
    var css = tt_t2t.style;

    // Store previous positioning
    tt_t2t.t_cp = css.position;
    tt_t2t.t_cl = css.left;
    tt_t2t.t_ct = css.top;
    tt_t2t.t_cd = css.display;
    // Store the tag's parent element so we can restore that DOM branch
    // when the tooltip is being hidden
    tt_t2tDad = tt_GetDad(tt_t2t);
    tt_MovDomNode(tt_t2t, tt_t2tDad, tt_aElt[6]);
    css.display = "block";
    css.position = "static";
    css.left = css.top = css.marginLeft = css.marginTop = "0px";
}
function tt_UnEl2Tip() {
    // Restore positioning and display
    var css = tt_t2t.style;

    css.display = tt_t2t.t_cd;
    tt_MovDomNode(tt_t2t, tt_GetDad(tt_t2t), tt_t2tDad);
    css.position = tt_t2t.t_cp;
    css.left = tt_t2t.t_cl;
    css.top = tt_t2t.t_ct;
    tt_t2tDad = null;
}
function tt_OverInit() {
    if (window.event)
        tt_over = window.event.target || window.event.srcElement;
    else
        tt_over = tt_ovr_;
    tt_DeAlt(tt_over);
    tt_OpDeHref(tt_over);
}
function tt_ShowInit() {
    tt_tShow.Timer("tt_Show()", tt_aV[DELAY], true);
    if (tt_aV[CLICKCLOSE] || tt_aV[CLICKSTICKY])
        tt_AddEvtFnc(document, "mouseup", tt_OnLClick);
}
function tt_Show() {
    var css = tt_aElt[0].style;

    // Override the z-index of the topmost wz_dragdrop.js D&D item
    css.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010);
    css.zIndex = String(SiteMaxZIndex + 20);
    if (tt_aV[STICKY] || !tt_aV[FOLLOWMOUSE])
        tt_iState &= ~0x4;
    if (tt_aV[EXCLUSIVE])
        tt_iState |= 0x8;
    if (tt_aV[DURATION] > 0)
        tt_tDurt.Timer("tt_HideInit()", tt_aV[DURATION], true);
    tt_ExtCallFncs(0, "Show")
    css.visibility = "visible";
    tt_iState |= 0x2;
    if (tt_aV[FADEIN])
        tt_Fade(0, 0, tt_aV[OPACITY], Math.round(tt_aV[FADEIN] / tt_aV[FADEINTERVAL]));
    tt_ShowIfrm();
}
function tt_ShowIfrm() {
    if (tt_ie56) {
        var ifrm = tt_aElt[tt_aElt.length - 1];
        if (ifrm) {
            var css = ifrm.style;
            css.zIndex = tt_aElt[0].style.zIndex - 1;
            css.zIndex = String(SiteMaxZIndex + 20);
            css.display = "block";
        }
    }
}
function tt_Move(e) {
    if (e)
        tt_ovr_ = e.target || e.srcElement;
    e = e || window.event;
    if (e) {
        tt_musX = tt_GetEvtX(e);
        tt_musY = tt_GetEvtY(e);
    }
    if (tt_iState & 0x4) {
        // Prevent jam of mousemove events
        if (!tt_op && !tt_ie) {
            if (tt_bWait)
                return;
            tt_bWait = true;
            tt_tWaitMov.Timer("tt_bWait = false;", 1, true);
        }
        if (tt_aV[FIX]) {
            tt_iState &= ~0x4;
            tt_PosFix();
        }
        else if (!tt_ExtCallFncs(e, "MoveBefore"))
            tt_SetTipPos(tt_Pos(0), tt_Pos(1));
        tt_ExtCallFncs([tt_musX, tt_musY], "MoveAfter")
    }
}
function tt_Pos(iDim) {
    var iX, bJmpMod, cmdAlt, cmdOff, cx, iMax, iScrl, iMus, bJmp;

    // Map values according to dimension to calculate
    if (iDim) {
        bJmpMod = tt_aV[JUMPVERT];
        cmdAlt = ABOVE;
        cmdOff = OFFSETY;
        cx = tt_h;
        iMax = tt_maxPosY;
        iScrl = tt_GetScrollY();
        iMus = tt_musY;
        bJmp = tt_bJmpVert;
    }
    else {
        bJmpMod = tt_aV[JUMPHORZ];
        cmdAlt = LEFT;
        cmdOff = OFFSETX;
        cx = tt_w;
        iMax = tt_maxPosX;
        iScrl = tt_GetScrollX();
        iMus = tt_musX;
        bJmp = tt_bJmpHorz;
    }
    if (bJmpMod) {
        if (tt_aV[cmdAlt] && (!bJmp || tt_CalcPosAlt(iDim) >= iScrl + 16))
            iX = tt_PosAlt(iDim);
        else if (!tt_aV[cmdAlt] && bJmp && tt_CalcPosDef(iDim) > iMax - 16)
            iX = tt_PosAlt(iDim);
        else
            iX = tt_PosDef(iDim);
    }
    else {
        iX = iMus;
        if (tt_aV[cmdAlt])
            iX -= cx + tt_aV[cmdOff] - (tt_aV[SHADOW] ? tt_aV[SHADOWWIDTH] : 0);
        else
            iX += tt_aV[cmdOff];
    }
    // Prevent tip from extending past clientarea boundary
    if (iX > iMax)
        iX = bJmpMod ? tt_PosAlt(iDim) : iMax;
    // In case of insufficient space on both sides, ensure the left/upper part
    // of the tip be visible
    if (iX < iScrl)
        iX = bJmpMod ? tt_PosDef(iDim) : iScrl;
    return iX;
}
function tt_PosDef(iDim) {
    if (iDim)
        tt_bJmpVert = tt_aV[ABOVE];
    else
        tt_bJmpHorz = tt_aV[LEFT];
    return tt_CalcPosDef(iDim);
}
function tt_PosAlt(iDim) {
    if (iDim)
        tt_bJmpVert = !tt_aV[ABOVE];
    else
        tt_bJmpHorz = !tt_aV[LEFT];
    return tt_CalcPosAlt(iDim);
}
function tt_CalcPosDef(iDim) {
    return iDim ? (tt_musY + tt_aV[OFFSETY]) : (tt_musX + tt_aV[OFFSETX]);
}
function tt_CalcPosAlt(iDim) {
    var cmdOff = iDim ? OFFSETY : OFFSETX;
    var dx = tt_aV[cmdOff] - (tt_aV[SHADOW] ? tt_aV[SHADOWWIDTH] : 0);
    if (tt_aV[cmdOff] > 0 && dx <= 0)
        dx = 1;
    return ((iDim ? (tt_musY - tt_h) : (tt_musX - tt_w)) - dx);
}
function tt_PosFix() {
    var iX, iY;

    if (typeof (tt_aV[FIX][0]) == "number") {
        iX = tt_aV[FIX][0];
        iY = tt_aV[FIX][1];
    }
    else {
        if (typeof (tt_aV[FIX][0]) == "string")
            el = tt_GetElt(tt_aV[FIX][0]);
        // First slot in array is direct reference to HTML element
        else
            el = tt_aV[FIX][0];
        iX = tt_aV[FIX][1];
        iY = tt_aV[FIX][2];
        // By default, vert pos is related to bottom edge of HTML element
        if (!tt_aV[ABOVE] && el)
            iY += tt_GetDivH(el);
        for (; el; el = el.offsetParent) {
            iX += el.offsetLeft || 0;
            iY += el.offsetTop || 0;
        }
    }
    // For a fixed tip positioned above the mouse, use the bottom edge as anchor
    // (recommended by Christophe Rebeschini, 31.1.2008)
    if (tt_aV[ABOVE])
        iY -= tt_h;
    tt_SetTipPos(iX, iY);
}
function tt_Fade(a, now, z, n) {
    if (n) {
        now += Math.round((z - now) / n);
        if ((z > a) ? (now >= z) : (now <= z))
            now = z;
        else
            tt_tFade.Timer(
				"tt_Fade("
				+ a + "," + now + "," + z + "," + (n - 1)
				+ ")",
				tt_aV[FADEINTERVAL],
				true
			);
    }
    now ? tt_SetTipOpa(now) : tt_Hide();
}
function tt_SetTipOpa(opa) {
    // To circumvent the opacity nesting flaws of IE, we set the opacity
    // for each sub-DIV separately, rather than for the container DIV.
    tt_SetOpa(tt_aElt[5], opa);
    if (tt_aElt[1])
        tt_SetOpa(tt_aElt[1], opa);
    if (tt_aV[SHADOW]) {
        opa = Math.round(opa * 0.8);
        tt_SetOpa(tt_aElt[7], opa);
        tt_SetOpa(tt_aElt[8], opa);
    }
}
function tt_OnCloseBtnOver(iOver) {
    var css = tt_aElt[4].style;

    iOver <<= 1;
    css.background = tt_aV[CLOSEBTNCOLORS][iOver];
    css.color = tt_aV[CLOSEBTNCOLORS][iOver + 1];
}
function tt_OnLClick(e) {
    //  Ignore right-clicks
    e = e || window.event;
    if (!((e.button && e.button & 2) || (e.which && e.which == 3))) {
        if (tt_aV[CLICKSTICKY] && (tt_iState & 0x4)) {
            tt_aV[STICKY] = true;
            tt_iState &= ~0x4;
        }
        else if (tt_aV[CLICKCLOSE])
            tt_HideInit();
    }
}
function tt_Int(x) {
    var y;

    return (isNaN(y = parseInt(x)) ? 0 : y);
}
Number.prototype.Timer = function(s, iT, bUrge) {
    if (!this.value || bUrge)
        this.value = window.setTimeout(s, iT);
}
Number.prototype.EndTimer = function() {
    if (this.value) {
        window.clearTimeout(this.value);
        this.value = 0;
    }
}
function tt_GetWndCliSiz(s) {
    var db, y = window["inner" + s], sC = "client" + s, sN = "number";
    if (typeof y == sN) {
        var y2;
        return (
        // Gecko or Opera with scrollbar
        // ... quirks mode
			((db = document.body) && typeof (y2 = db[sC]) == sN && y2 && y2 <= y) ? y2
        // ... strict mode
			: ((db = document.documentElement) && typeof (y2 = db[sC]) == sN && y2 && y2 <= y) ? y2
        // No scrollbar, or clientarea size == 0, or other browser (KHTML etc.)
			: y
		);
    }
    // IE
    return (
    // document.documentElement.client+s functional, returns > 0
		((db = document.documentElement) && (y = db[sC])) ? y
    // ... not functional, in which case document.body.client+s 
    // is the clientarea size, fortunately
		: document.body[sC]
	);
}
function tt_SetOpa(el, opa) {
    var css = el.style;

    tt_opa = opa;
    if (tt_flagOpa == 1) {
        if (opa < 100) {
            // Hacks for bugs of IE:
            // 1.) Once a CSS filter has been applied, fonts are no longer
            // anti-aliased, so we store the previous 'non-filter' to be
            // able to restore it
            if (typeof (el.filtNo) == tt_u)
                el.filtNo = css.filter;
            // 2.) A DIV cannot be made visible in a single step if an
            // opacity < 100 has been applied while the DIV was hidden
            var bVis = css.visibility != "hidden";
            // 3.) In IE6, applying an opacity < 100 has no effect if the
            //	   element has no layout (position, size, zoom, ...)
            css.zoom = "100%";
            if (!bVis)
                css.visibility = "visible";
            css.filter = "alpha(opacity=" + opa + ")";
            if (!bVis)
                css.visibility = "hidden";
        }
        else if (typeof (el.filtNo) != tt_u)
        // Restore 'non-filter'
            css.filter = el.filtNo;
    }
    else {
        opa /= 100.0;
        switch (tt_flagOpa) {
            case 2:
                css.KhtmlOpacity = opa; break;
            case 3:
                css.KHTMLOpacity = opa; break;
            case 4:
                css.MozOpacity = opa; break;
            case 5:
                css.opacity = opa; break;
        }
    }
}
function tt_Err(sErr, bIfDebug) {
    if (tt_Debug || !bIfDebug)
        alert("Tooltip Script Error Message:\n\n" + sErr);
}

//============  EXTENSION (PLUGIN) MANAGER  ===============//
function tt_ExtCmdEnum() {
    var s;

    // Add new command(s) to the commands enum
    for (var i in config) {
        s = "window." + i.toString().toUpperCase();
        if (eval("typeof(" + s + ") == tt_u")) {
            eval(s + " = " + tt_aV.length);
            tt_aV[tt_aV.length] = null;
        }
    }
}
function tt_ExtCallFncs(arg, sFnc) {
    var b = false;
    for (var i = tt_aExt.length; i; ) {
        --i;
        var fnc = tt_aExt[i]["On" + sFnc];
        // Call the method the extension has defined for this event
        if (fnc && fnc(arg))
            b = true;
    }
    return b;
}

tt_Init();

/*End wz_tooltip.js*/

/*tip_balloon.js*/
/*
tip_balloon.js  v. 1.81

The latest version is available at
http://www.walterzorn.com
or http://www.devira.com
or http://www.walterzorn.de

Initial author: Walter Zorn
Last modified: 2.2.2009

Extension for the tooltip library wz_tooltip.js.
Implements balloon tooltips.
*/

// Make sure that the core file wz_tooltip.js is included first
if (typeof config == "undefined")
    alert("Error:\nThe core tooltip script file 'wz_tooltip.js' must be included first, before the plugin files!");

// Here we define new global configuration variable(s) (as members of the
// predefined "config." class).
// From each of these config variables, wz_tooltip.js will automatically derive
// a command which can be passed to Tip() or TagToTip() in order to customize
// tooltips individually. These command names are just the config variable
// name(s) translated to uppercase,
// e.g. from config. Balloon a command BALLOON will automatically be
// created.

//===================  GLOBAL TOOLTIP CONFIGURATION  =========================//
config.Balloon = false	// true or false - set to true if you want this to be the default behaviour
config.BalloonImgPath = "/images/tip_balloon/" // Path to images (border, corners, stem), in quotes. Path must be relative to your HTML file.
// Sizes of balloon images
config.BalloonEdgeSize = 6		// Integer - sidelength of quadratic corner images
config.BalloonStemWidth = 15	// Integer
config.BalloonStemHeight = 19	// Integer
config.BalloonStemOffset = -7	// Integer - horizontal offset of left stem edge from mouse (recommended: -stemwidth/2 to center the stem above the mouse)
config.BalloonImgExt = "gif"; // File name extension of default balloon images, e.g. "gif" or "png"
config.Width = 160
//=======  END OF TOOLTIP CONFIG, DO NOT CHANGE ANYTHING BELOW  ==============//


// Create a new tt_Extension object (make sure that the name of that object,
// here balloon, is unique amongst the extensions available for wz_tooltips.js):
var balloon = new tt_Extension();

// Implement extension eventhandlers on which our extension should react

balloon.OnLoadConfig = function() {
    if (tt_aV[BALLOON]) {
        // Turn off native style properties which are not appropriate
        balloon.padding = Math.max(tt_aV[PADDING] - tt_aV[BALLOONEDGESIZE], 0);
        balloon.width = tt_aV[WIDTH];
        //if(tt_bBoxOld)
        //	balloon.width += (balloon.padding << 1);
        tt_aV[BORDERWIDTH] = 0;
        tt_aV[WIDTH] = 0;
        tt_aV[PADDING] = 0;
        tt_aV[BGCOLOR] = "";
        tt_aV[BGIMG] = "";
        tt_aV[SHADOW] = false;
        // Append slash to img path if missing
        if (tt_aV[BALLOONIMGPATH].charAt(tt_aV[BALLOONIMGPATH].length - 1) != '/')
            tt_aV[BALLOONIMGPATH] += "/";
        return true;
    }
    return false;
};
balloon.OnCreateContentString = function() {
    if (!tt_aV[BALLOON])
        return false;

    var aImg, sImgZ, sCssCrn, sVaT, sVaB, sCss0;

    // Cache balloon images in advance:
    // Either use the pre-cached default images...
    if (tt_aV[BALLOONIMGPATH] == config.BalloonImgPath)
        aImg = balloon.aDefImg;
    // ...or load images from different directory
    else
        aImg = Balloon_CacheImgs(tt_aV[BALLOONIMGPATH], tt_aV[BALLOONIMGEXT]);
    sCss0 = 'padding:0;margin:0;border:0;line-height:0;overflow:hidden;';
    sCssCrn = ' style="position:relative;width:' + tt_aV[BALLOONEDGESIZE] + 'px;' + sCss0 + 'overflow:hidden;';
    sVaT = 'vertical-align:top;" valign="top"';
    sVaB = 'vertical-align:bottom;" valign="bottom"';
    sImgZ = '" style="' + sCss0 + '" />';

    tt_sContent = '<table border="0" cellpadding="0" cellspacing="0" style="width:auto;padding:0;margin:0;left:0;top:0;"><tr>'
    // Left-top corner
		+ '<td' + sCssCrn + sVaB + '>'
		+ '<img src="' + aImg[1].src + '" width="' + tt_aV[BALLOONEDGESIZE] + '" height="' + tt_aV[BALLOONEDGESIZE] + sImgZ
		+ '</td>'
    // Top border
		+ '<td valign="bottom" style="position:relative;' + sCss0 + '">'
		+ '<img id="bALlOOnT" style="position:relative;top:1px;z-index:1;display:none;' + sCss0 + '" src="' + aImg[9].src + '" width="' + tt_aV[BALLOONSTEMWIDTH] + '" height="' + tt_aV[BALLOONSTEMHEIGHT] + '" />'
		+ '<div style="position:relative;z-index:0;top:0;' + sCss0 + 'width:auto;height:' + tt_aV[BALLOONEDGESIZE] + 'px;background-image:url(' + aImg[2].src + ');">'
		+ '</div>'
		+ '</td>'
    // Right-top corner
		+ '<td' + sCssCrn + sVaB + '>'
		+ '<img src="' + aImg[3].src + '" width="' + tt_aV[BALLOONEDGESIZE] + '" height="' + tt_aV[BALLOONEDGESIZE] + sImgZ
		+ '</td>'
		+ '</tr><tr>'
    // Left border (background-repeat fix courtesy Dirk Schnitzler)
		+ '<td style="position:relative;background-repeat:repeat;' + sCss0 + 'width:' + tt_aV[BALLOONEDGESIZE] + 'px;background-image:url(' + aImg[8].src + ');">'
    // Redundant image for bugous old Geckos which won't auto-expand TD height to 100%
		+ '<img width="' + tt_aV[BALLOONEDGESIZE] + '" height="100%" src="' + aImg[8].src + sImgZ
		+ '</td>'
    // Content
		+ '<td id="bALlO0nBdY" style="position:relative;line-height:normal;background-repeat:repeat;'
		+ ';background-image:url(' + aImg[0].src + ')'
		+ ';color:' + tt_aV[FONTCOLOR]
		+ ';font-family:' + tt_aV[FONTFACE]
		+ ';font-size:' + tt_aV[FONTSIZE]
		+ ';font-weight:' + tt_aV[FONTWEIGHT]
		+ ';text-align:' + tt_aV[TEXTALIGN]
		+ ';padding:' + balloon.padding + 'px'
		+ ';width:' + ((balloon.width > 0) ? (balloon.width + 'px') : 'auto')
		+ ';">' + tt_sContent + '</td>'
    // Right border
		+ '<td style="position:relative;background-repeat:repeat;' + sCss0 + 'width:' + tt_aV[BALLOONEDGESIZE] + 'px;background-image:url(' + aImg[4].src + ');">'
    // Image redundancy for bugous old Geckos that won't auto-expand TD height to 100%
		+ '<img width="' + tt_aV[BALLOONEDGESIZE] + '" height="100%" src="' + aImg[4].src + sImgZ
		+ '</td>'
		+ '</tr><tr>'
    // Left-bottom corner
		+ '<td' + sCssCrn + sVaT + '>'
		+ '<img src="' + aImg[7].src + '" width="' + tt_aV[BALLOONEDGESIZE] + '" height="' + tt_aV[BALLOONEDGESIZE] + sImgZ
		+ '</td>'
    // Bottom border
		+ '<td valign="top" style="position:relative;' + sCss0 + '">'
		+ '<div style="position:relative;left:0;top:0;' + sCss0 + 'width:auto;height:' + tt_aV[BALLOONEDGESIZE] + 'px;background-image:url(' + aImg[6].src + ');"></div>'
		+ '<img id="bALlOOnB" style="position:relative;top:-1px;left:2px;z-index:1;display:none;' + sCss0 + '" src="' + aImg[10].src + '" width="' + tt_aV[BALLOONSTEMWIDTH] + '" height="' + tt_aV[BALLOONSTEMHEIGHT] + '" />'
		+ '</td>'
    // Right-bottom corner
		+ '<td' + sCssCrn + sVaT + '>'
		+ '<img src="' + aImg[5].src + '" width="' + tt_aV[BALLOONEDGESIZE] + '" height="' + tt_aV[BALLOONEDGESIZE] + sImgZ
		+ '</td>'
		+ '</tr></table>'; //alert(tt_sContent);
    return true;
};
balloon.OnSubDivsCreated = function() {
    if (tt_aV[BALLOON]) {
        var bdy = tt_GetElt("bALlO0nBdY");

        // Insert a TagToTip() HTML element into the central body TD
        if (tt_t2t && !tt_aV[COPYCONTENT] && bdy)
            tt_MovDomNode(tt_t2t, tt_GetDad(tt_t2t), bdy);
        balloon.iStem = tt_aV[ABOVE] * 1;
        balloon.aStem = [tt_GetElt("bALlOOnT"), tt_GetElt("bALlOOnB")];
        balloon.aStem[balloon.iStem].style.display = "inline";
        if (balloon.width < -1)
            Balloon_MaxW(bdy);
        return true;
    }
    return false;
};
// Display the stem appropriately
balloon.OnMoveAfter = function() {
    if (tt_aV[BALLOON]) {
        var iStem = (tt_aV[ABOVE] != tt_bJmpVert) * 1;

        // Tooltip position vertically flipped?
        if (iStem != balloon.iStem) {
            // Display opposite stem
            balloon.aStem[balloon.iStem].style.display = "none";
            balloon.aStem[iStem].style.display = "inline";
            balloon.iStem = iStem;
        }

        balloon.aStem[iStem].style.left = Balloon_CalcStemX() + "px";
        return true;
    }
    return false;
};
function Balloon_CalcStemX() {
    var x = tt_musX - tt_x + tt_aV[BALLOONSTEMOFFSET] - tt_aV[BALLOONEDGESIZE];
    return Math.max(Math.min(x, tt_w - tt_aV[BALLOONSTEMWIDTH] - (tt_aV[BALLOONEDGESIZE] << 1) - 2), 2);
}
function Balloon_CacheImgs(sPath, sExt) {
    var asImg = ["background", "lt", "t", "rt", "r", "rb", "b", "lb", "l", "stemt", "stemb"],
	n = asImg.length,
	aImg = new Array(n),
	img;

    while (n) {
        --n;
        img = aImg[n] = new Image();
        img.src = sPath + asImg[n] + "." + sExt;
    }
    return aImg;
}
function Balloon_MaxW(bdy) {
    if (bdy) {
        var iAdd = tt_bBoxOld ? (balloon.padding << 1) : 0, w = tt_GetDivW(bdy);
        if (w > -balloon.width + iAdd)
            bdy.style.width = (-balloon.width + iAdd) + "px";
    }
}
// This mechanism pre-caches the default images specified by
// congif.BalloonImgPath, so, whenever a balloon tip using these default images
// is created, no further server connection is necessary.
function Balloon_PreCacheDefImgs() {
    // Append slash to img path if missing
    if (config.BalloonImgPath.charAt(config.BalloonImgPath.length - 1) != '/')
        config.BalloonImgPath += "/";
    // Preload default images into array
    balloon.aDefImg = Balloon_CacheImgs(config.BalloonImgPath, config.BalloonImgExt);
}
Balloon_PreCacheDefImgs();

/*end tip_balloon.js*/

/*tip_centerwindow.js*/
/*
tip_centerwindow.js  v. 1.21

The latest version is available at
http://www.walterzorn.com
or http://www.devira.com
or http://www.walterzorn.de

Initial author: Walter Zorn
Last modified: 3.6.2008

Extension for the tooltip library wz_tooltip.js.
Centers a sticky tooltip in the window's visible clientarea,
optionally even if the window is being scrolled or resized.
*/

// Make sure that the core file wz_tooltip.js is included first
if (typeof config == "undefined")
    alert("Error:\nThe core tooltip script file 'wz_tooltip.js' must be included first, before the plugin files!");

// Here we define new global configuration variable(s) (as members of the
// predefined "config." class).
// From each of these config variables, wz_tooltip.js will automatically derive
// a command which can be passed to Tip() or TagToTip() in order to customize
// tooltips individually. These command names are just the config variable
// name(s) translated to uppercase,
// e.g. from config. CenterWindow a command CENTERWINDOW will automatically be
// created.

//===================  GLOBAL TOOLTIP CONFIGURATION  =========================//
config.CenterWindow = false	// true or false - set to true if you want this to be the default behaviour
config.CenterAlways = false	// true or false - recenter if window is resized or scrolled
//=======  END OF TOOLTIP CONFIG, DO NOT CHANGE ANYTHING BELOW  ==============//


// Create a new tt_Extension object (make sure that the name of that object,
// here ctrwnd, is unique amongst the extensions available for
// wz_tooltips.js):
var ctrwnd = new tt_Extension();

// Implement extension eventhandlers on which our extension should react
ctrwnd.OnLoadConfig = function() {
    if (tt_aV[CENTERWINDOW]) {
        // Permit CENTERWINDOW only if the tooltip is sticky
        if (tt_aV[STICKY]) {
            if (tt_aV[CENTERALWAYS]) {
                // IE doesn't support style.position "fixed"
                if (tt_ie)
                    tt_AddEvtFnc(window, "scroll", Ctrwnd_DoCenter);
                else
                    tt_aElt[0].style.position = "fixed";
                tt_AddEvtFnc(window, "resize", Ctrwnd_DoCenter);
            }
            return true;
        }
        tt_aV[CENTERWINDOW] = false;
    }
    return false;
};
// We react on the first OnMouseMove event to center the tip on that occasion
ctrwnd.OnMoveBefore = Ctrwnd_DoCenter;
ctrwnd.OnKill = function() {
    if (tt_aV[CENTERWINDOW] && tt_aV[CENTERALWAYS]) {
        tt_RemEvtFnc(window, "resize", Ctrwnd_DoCenter);
        if (tt_ie)
            tt_RemEvtFnc(window, "scroll", Ctrwnd_DoCenter);
        else
            tt_aElt[0].style.position = "absolute";
    }
    return false;
};
// Helper function
function Ctrwnd_DoCenter() {
    if (tt_aV[CENTERWINDOW]) {
        var x, y, dx, dy;

        // Here we use some functions and variables (tt_w, tt_h) which the
        // extension API of wz_tooltip.js provides for us
        if (tt_ie || !tt_aV[CENTERALWAYS]) {
            dx = tt_GetScrollX();
            dy = tt_GetScrollY();
        }
        else {
            dx = 0;
            dy = 0;
        }
        // Position the tip, offset from the center by OFFSETX and OFFSETY
        x = (tt_GetClientW() - tt_w) / 2 + dx + tt_aV[OFFSETX];
        y = (tt_GetClientH() - tt_h) / 2 + dy + tt_aV[OFFSETY];
        tt_SetTipPos(x, y);
        return true;
    }
    return false;
}

/*end tip_centerwindow.js*/

/*tip_followscroll.js*/
/*
tip_followscroll.js	v. 1.11

The latest version is available at
http://www.walterzorn.com
or http://www.devira.com
or http://www.walterzorn.de

Initial author: Walter Zorn
Last modified: 3.6.2008

Extension for the tooltip library wz_tooltip.js.
Lets a "sticky" tooltip keep its position inside the clientarea if the window
is scrolled.
*/

// Make sure that the core file wz_tooltip.js is included first
if (typeof config == "undefined")
    alert("Error:\nThe core tooltip script file 'wz_tooltip.js' must be included first, before the plugin files!");

// Here we define new global configuration variable(s) (as members of the
// predefined "config." class).
// From each of these config variables, wz_tooltip.js will automatically derive
// a command which can be passed to Tip() or TagToTip() in order to customize
// tooltips individually. These command names are just the config variable
// name(s) translated to uppercase,
// e.g. from config. FollowScroll a command FOLLOWSCROLL will automatically be
// created.

//===================	GLOBAL TOOLTIP CONFIGURATION	======================//
config.FollowScroll = false		// true or false - set to true if you want this to be the default behaviour
//=======	END OF TOOLTIP CONFIG, DO NOT CHANGE ANYTHING BELOW	==============//


// Create a new tt_Extension object (make sure that the name of that object,
// here fscrl, is unique amongst the extensions available for
// wz_tooltips.js):
var fscrl = new tt_Extension();

// Implement extension eventhandlers on which our extension should react
fscrl.OnShow = function() {
    if (tt_aV[FOLLOWSCROLL]) {
        // Permit FOLLOWSCROLL only if the tooltip is sticky
        if (tt_aV[STICKY]) {
            var x = tt_x - tt_GetScrollX(), y = tt_y - tt_GetScrollY();

            if (tt_ie) {
                fscrl.MoveOnScrl.offX = x;
                fscrl.MoveOnScrl.offY = y;
                fscrl.AddRemEvtFncs(tt_AddEvtFnc);
            }
            else {
                tt_SetTipPos(x, y);
                tt_aElt[0].style.position = "fixed";
            }
            return true;
        }
        tt_aV[FOLLOWSCROLL] = false;
    }
    return false;
};
fscrl.OnHide = function() {
    if (tt_aV[FOLLOWSCROLL]) {
        if (tt_ie)
            fscrl.AddRemEvtFncs(tt_RemEvtFnc);
        else
            tt_aElt[0].style.position = "absolute";
    }
};
// Helper functions (encapsulate in the class to avoid conflicts with other
// extensions)
fscrl.MoveOnScrl = function() {
    tt_SetTipPos(fscrl.MoveOnScrl.offX + tt_GetScrollX(), fscrl.MoveOnScrl.offY + tt_GetScrollY());
};
fscrl.AddRemEvtFncs = function(PAddRem) {
    PAddRem(window, "resize", fscrl.MoveOnScrl);
    PAddRem(window, "scroll", fscrl.MoveOnScrl);
};


/*end tip_followscroll.js*/
function isLowerCase(aCharacter) {
    return (aCharacter >= 'a') && (aCharacter <= 'z')
};


function isUpperCase(aCharacter) {
    return (aCharacter >= 'A') && (aCharacter <= 'Z')
};


function StopEvent(event) {
    event = event || window.event;

    if (event) {
        if (event.stopPropagation) event.stopPropagation();
        if (event.preventDefault) event.preventDefault();

        if (typeof event.cancelBubble != "undefined") {
            event.cancelBubble = true;
            event.returnValue = false;
        }
    }

    return false;
}