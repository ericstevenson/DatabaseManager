function autoCloseAlert(selector, delay) {
    var alert = $(selector).alert();
    window.setTimeout(function () { alert.alert('close') }, delay);
}


        function verifyDates() {
            var dateElems = document.getElementsByClassName("date-item");
            for (var x = 0; x < dateElems.length; x++) {
                if (!isValidDate(dateElems[x].value)) dateElems[x].value = "";
            }
        }

function isValidDate(str) {
    var t = str.match(/^(\d{4})\/(\d{2})\/(\d{2})$/);
    if (str === "") return true;
    if (t !== null) {
        var y = +t[1], m = +t[2], d = +t[3];
        var date = new Date(y, m - 1, d);
        if (date.getFullYear() === y && date.getMonth() === m - 1) {
            return true;
        }
    }
    return false;
}

