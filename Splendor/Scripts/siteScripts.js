/* To search tables on key up */

function searchGames(link) {
    var search = $("#Search").val().replaceAll(" ", "_");
    $('#AllTables').load(link + '?searchString=' + search)
}

var searchRequest = null;

function searchLogs(e, link, focusOnId) {
    if (e.keyCode >= 16 && e.keyCode <= 40) {
        console.log("Do not search");
    }
    else {
        var searchType = $("#SearchType").val().replaceAll(" ", "_");
        var searchUserName = $("#SearchUserName").val().replaceAll(" ", "_");
        var searchGameName = $("#SearchGameName").val().replaceAll(" ", "_");
        var searchMessage = $("#SearchMessage").val().replaceAll(" ", "_");

        if (searchRequest != null)
            searchRequest.abort();
        searchRequest = $.ajax({
            type: "GET",
            url: link,
            data: {
                'searchType': searchType,
                'searchUserName': searchUserName,
                'searchGameName': searchGameName,
                'searchMessage': searchMessage
            },
            dataType: "text",
            success: function (msg) {
                //we need to check if the value is the same
                if (searchType == $("#SearchType").val().replaceAll(" ", "_") &&
                    searchUserName == $("#SearchUserName").val().replaceAll(" ", "_") &&
                    searchGameName == $("#SearchGameName").val().replaceAll(" ", "_") &&
                    searchMessage == $("#SearchMessage").val().replaceAll(" ", "_")) {
                    var carretPosition = getCaretPosition(focusOnId);
                    document.getElementById("LogsTable").innerHTML = msg;
                    // $("#" + focusOnId).focus();
                    setCaretPosition(focusOnId, carretPosition);
                }
            }
        });
    }
}

/* To slide players' area */

function slideArea(id) {
    if (typeof id !== 'undefined') {
        $('#' + id).addClass("slide");
    }
    saveState();
}

function clearSlides() {
    $('*').removeClass("slide");
    saveState();
}

/* To save classes between refreshes of ParialView #GamePlay */

var slideClasses = {};
var savedCardInfo = {};
var savedEndInfoClass;

function saveState() {
    for (var i = 0; i < arguments.length; ++i) {
        slideClasses[arguments[i]] = document.getElementById(arguments[i]).classList.contains("slide");
    }
    var cardInfo = document.getElementById("CardInfo");
    if (cardInfo != null) {
        savedCardInfo.InnerHtml = cardInfo.innerHTML;
        savedCardInfo.Class = cardInfo.className;
        savedCardInfo.StyleLeft = cardInfo.style.left;
        savedCardInfo.StyleTop = cardInfo.style.top;
    }
    var endInfo = document.getElementById("EndInfo");
    if (endInfo != null) {
        savedEndInfoClass = endInfo.className;
    }
}

function loadState() {
    for (key in slideClasses) {
        if (slideClasses[key]) {
            document.getElementById(key).className += " slide";
        }
    }
    var cardInfo = document.getElementById("CardInfo");
    if (cardInfo != null && savedCardInfo != null) {
        cardInfo.innerHTML = savedCardInfo.InnerHtml;
        cardInfo.className = savedCardInfo.Class == null ? "non-active-info" : savedCardInfo.Class;
        cardInfo.style.left = savedCardInfo.StyleLeft;
        cardInfo.style.top = savedCardInfo.StyleTop;
    }
    var endInfo = document.getElementById("EndInfo");
    if (endInfo != null && savedEndInfoClass != null) {
        endInfo.className = savedEndInfoClass;
    }
}

var x, y;

function clearInfo() {
    var cardInfo = document.getElementById("CardInfo");
    cardInfo.className = "non-active-info";
    /*var aristocratInfo = document.getElementById("aristocratInfo");
    aristocratInfo.className = "non-active-info";
    var gemInfo = document.getElementById("gemInfo");
    gemInfo.className = "non-active-info";*/
}

function moveInfo(e) {
    x = e.clientX + "px";
    y = e.clientY + "px";
    var cardInfo = document.getElementById("CardInfo");
    cardInfo.style.left = x;
    cardInfo.style.top = y;
    /*var aristocratInfo = document.getElementById("aristocratInfo");
    aristocratInfo.style.left = x;
    aristocratInfo.style.top = y;
    var gemInfo = document.getElementById("gemInfo");
    gemInfo.style.left = x;
    gemInfo.style.top = y;*/
}

function createCardInfo(green, white, blue, black, red, gold) {
    createGemInfo(green, white, blue, black, red, gold, "CardInfo")
}

function createGemInfo(green, white, blue, black, red, gold, id) {
    var cardInfo = document.getElementById(id);
    cardInfo.className = "active-info";
    cardInfo.innerHTML = "";
    for (var i = 0; i < white; ++i) {
        cardInfo.innerHTML += '<img src="/Images/Gem/gem1.png" alt="white" />'
    }
    for (var i = 0; i < blue; ++i) {
        cardInfo.innerHTML += '<img src="/Images/Gem/gem2.png" alt="blue" />'
    }
    for (var i = 0; i < green; ++i) {
        cardInfo.innerHTML += '<img src="/Images/Gem/gem0.png" alt="green" />'
    }
    for (var i = 0; i < red; ++i) {
        cardInfo.innerHTML += '<img src="/Images/Gem/gem4.png" alt="red" />'
    }
    for (var i = 0; i < black; ++i) {
        cardInfo.innerHTML += '<img src="/Images/Gem/gem3.png" alt="black" />'
    }
    for (var i = 0; i < gold; ++i) {
        cardInfo.innerHTML += '<img src="/Images/Gem/gem5.png" alt="gold" />'
    }
}

/* Popups */

function hidePopup(id) {
    $(id).addClass("hidden");
}

/* Solution from http://stackoverflow.com/questions/2897155/get-cursor-position-in-characters-within-a-text-input-field */

/*
** Returns the caret (cursor) position of the specified text field.
** Return value range is 0-oField.value.length.
*/
function getCaretPosition(elemId) {
    var oField = document.getElementById(elemId);

    var iCaretPos = 0; // Initialize

    if (document.selection) { // IE Support
        oField.focus(); // Set focus on the element
        var oSel = document.selection.createRange(); // To get cursor position, get empty selection range
        oSel.moveStart('character', -oField.value.length); // Move selection start to 0 position
        iCaretPos = oSel.text.length; // The caret position is selection length
    }
    else if (oField.selectionStart || oField.selectionStart == '0') // Firefox support
        iCaretPos = oField.selectionStart;

    return iCaretPos; // Return results
}

/* Solution from http://stackoverflow.com/questions/512528/set-cursor-position-in-html-textbox */

function setCaretPosition(elemId, caretPos) {
    var el = document.getElementById(elemId);

    el.value = el.value;
    // ^ this is used to not only get "focus", but
    // to make sure we don't have it everything -selected-
    // (it causes an issue in chrome, and having it doesn't hurt any other browser)

    if (el !== null) {

        if (el.createTextRange) {
            var range = el.createTextRange();
            range.move('character', caretPos);
            range.select();
            return true;
        }

        else {
            // (el.selectionStart === 0 added for Firefox bug)
            if (el.selectionStart || el.selectionStart === 0) {
                el.focus();
                el.setSelectionRange(caretPos, caretPos);
                return true;
            }

            else { // fail city, fortunately this never happens (as far as I've tested) :)
                el.focus();
                return false;
            }
        }
    }
}

/* Solution from http://stackoverflow.com/questions/1144783/replacing-all-occurrences-of-a-string-in-javascript */

String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.replace(new RegExp(search, 'g'), replacement);
};