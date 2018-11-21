function ShowPopup(popupClientId) {
    obj = $("#" + popupClientId + "");
    parentPopup = $(obj).closest(".popup-parent");
    popuptrigger = $(parentPopup).children(".popup-trigger");
    popupcontrol = $(parentPopup).children(".popup-control");
    ctrldimmer = $(parentPopup).find(".ctrldimmer");
    $(popuptrigger).hide();
    $(popupcontrol).show();
}
function ClosePopup(popupClientId) {
    obj = $("#" + popupClientId + "");
    parentPopup = $(obj).closest(".popup-parent");
    popuptrigger = $(parentPopup).children(".popup-trigger");
    popupcontrol = $(parentPopup).children(".popup-control");
    ctrldimmer = $(parentPopup).find(".ctrldimmer");
    $(popuptrigger).show();
    $(popupcontrol).hide();
}
function ShowConfirmationBox() {
    var popup = $('#confirmationForm #popupConfirmation');
    $(popup).css({ zIndex: 20010, position: "fixed", left: ($(window).width() - $(popup).width()) / 2 + "px", top: ($(window).height() - $(popup).height()-20) / 2 + "px" });
    $(popup).fadeIn("fast");
    $(".bodydimmer").show();
    return false;
}
function CloseConfirmationBox() {
    $('#confirmationForm #popupConfirmation').hide();
    $(".bodydimmer").hide();
    return false;
}
$(document).ready(function () {
    $(document).mouseup(function (e) {
        $(".small-submenu").delay(100).hide();
    });


});


function actionClick(obj) {

    $(obj).parent(".submenu-container").children(".small-submenu").show();

}


try {
    if (Sys != 'undefined') {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(beginRequest);
        prm.add_endRequest(endRequest);
    }
}
catch (exe) {
}

//var searchTimeOut;
function doButtonClick(bttn) {
    //clearTimeout(searchTimeOut);
    //searchTimeOut = setTimeout(document.getElementById(bttn).click(), 500);
    document.getElementById(bttn).click();
}

//function pageLoaded(sender, args) {
//    if (sender._postBackSettings) {
//        var panelId = sender._postBackSettings.panelID.split('|')[0];
//        if (panelId == sender._scriptManagerID) {
//            var updatedPanels = args.get_panelsUpdated();
//            var affectedPanels = "Affected Panels:\n";
//            for (var x = 0; x < updatedPanels.length; x++)
//                affectedPanels += updatedPanels[x].id + "\n";
//            alert("Request initiated by ScriptManager\n\nMight be an async trigger, or child of an update panel with children as triggers set to false.\n\n" + affectedPanels);
//        }
//        else
//            alert("Request initiated by: " + panelId);
//    }
//}
//Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);

function beginRequest(sender, args) {
    var id = null;
    if (sender._postBackSettings.panelsToUpdate != null && sender._postBackSettings.panelsToUpdate.length > 0) {
        id = sender._postBackSettings.panelsToUpdate[0];
    }
    else {
        id = sender._postBackSettings.panelID.split('|')[0];
    }
    id = id.replace(/\$/gi, '_');
    args._request.ID = id;
    ShowHideDivMask(id, true);
}

function endRequest(sender, args) {
    var id = args._response._webRequest.ID;
    id = id.replace(/\$/gi, '_');
    ShowHideDivMask(id, false);    
}

function ShowHideDivMask(idDivToMark, bShow) {
    var obj = $("#" + idDivToMark + "");
    var parentPopup = $(obj).closest(".popup-parent");
    var searhAnim = $(parentPopup).find(".ajax-loader");
    
    if (bShow) {
        searhAnim.show();
       
    } else {
       searhAnim.hide();
    }
    
}